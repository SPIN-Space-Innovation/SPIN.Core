/**
 * MIT License
 *
 * Copyright (c) 2023 SPIN - Space Innovation
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Iot.Device.Bmxx80;
using Iot.Device.Board;

using SPIN.Core.Extensions;
using SPIN.Core.Sensors;

namespace SPIN.Core.Installers;

public sealed class Bme280SensorInstaller : IInstaller
{
    private static readonly byte s_bme280ChipId = 0x60;

    // ReSharper disable once HeapView.ObjectAllocation.Evident
    private readonly List<I2cConnectionSettings> _bme280Locations = new();
    private List<Bme280Sensor?> _bme280Sensors = null!;

    private static bool TestForChipId(I2cDevice i2CDevice)
    {
        // ReSharper disable once RedundantCast
        Span<byte> writeSpan = stackalloc[] { (byte)0xD0 };
        Span<byte> readSpan = stackalloc byte[1];

        try
        {
            i2CDevice.WriteRead(writeSpan, readSpan);
        }
        catch
        {
            return false;
        }

        bool foundBme280ChipId = readSpan[0] == s_bme280ChipId;

        return foundBme280ChipId;
    }

    // ReSharper disable once UnusedParameter.Local
    private Bme280Sensor RegisterBme280Sensor(IServiceProvider serviceProvider, int position)
    {
        bool sensorIsNull = _bme280Sensors[position] is null;
        if (sensorIsNull)
        {
            _bme280Sensors[position] = new Bme280Sensor(_bme280Locations[position]);
        }

        return _bme280Sensors[position]!;
     }
    public ulong Priority => 1024;

    public Task<bool> CanInstallAsync(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // ReSharper disable once InconsistentNaming
        IEnumerable<I2cBus> i2cBuses = serviceCollection.GetServices<I2cBus>();

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once PossibleMultipleEnumeration
        bool noI2cBuses = !i2cBuses.Any();
        if (noI2cBuses)
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
            _bme280Sensors = new List<Bme280Sensor?>();
            return Task.FromResult(false);
        }

        // ReSharper disable once PossibleMultipleEnumeration
        for (int i = 0; i < i2cBuses.Count(); i++)
        {
            // ReSharper disable once InconsistentNaming
            // ReSharper disable once PossibleMultipleEnumeration
            I2cBus i2cBus = i2cBuses.ElementAt(i);
            List<int> deviceIds = i2cBus.PerformBusScan();
            bool noBme280DeviceId = !deviceIds.Contains(Bmx280Base.DefaultI2cAddress) &&
                                    !deviceIds.Contains(Bmx280Base.SecondaryI2cAddress);
            if (noBme280DeviceId)
            {
                continue;
            }

            // ReSharper disable once InconsistentNaming
            using I2cDevice i2cDevicePrimary = i2cBus.CreateDevice(Bmx280Base.DefaultI2cAddress);
            // ReSharper disable once InconsistentNaming
            using I2cDevice i2cDeviceSecondary = i2cBus.CreateDevice(Bmx280Base.SecondaryI2cAddress);

            bool foundBme280Primary = TestForChipId(i2cDevicePrimary);
            bool foundBme280Secondary = TestForChipId(i2cDeviceSecondary);

            if (foundBme280Primary)
            {
                _bme280Locations.Add(i2cDevicePrimary.ConnectionSettings);
            }

            if (foundBme280Secondary)
            {
                _bme280Locations.Add(i2cDeviceSecondary.ConnectionSettings);
            }
        }

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        _bme280Sensors = new List<Bme280Sensor?>(_bme280Locations.Count);

        return Task.FromResult(_bme280Locations.Any());
    }

    public async Task InstallServiceAsync(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        bool bme280sensorsListIsNull = _bme280Sensors is null;
        if (bme280sensorsListIsNull)
        {
            await CanInstallAsync(serviceCollection, configuration);
        }

        foreach (int i in Enumerable.Range(0, _bme280Locations.Count))
        {
            serviceCollection.AddSingleton<IPressureSensor, Bme280Sensor>(provider =>
                RegisterBme280Sensor(provider, i));
            serviceCollection.AddSingleton<IRelativeHumiditySensor, Bme280Sensor>(provider =>
                RegisterBme280Sensor(provider, i));
            serviceCollection.AddSingleton<ITemperatureSensor, Bme280Sensor>(provider =>
                RegisterBme280Sensor(provider, i));
        }
    }
}
