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

using SPIN.Core.Sensors;

namespace SPIN.Core.Installers;

public sealed class Bme280Installer : IInstaller
{
    private Bme280Sensor? _bme280 = null;

    private Bme280Sensor RegisterBme280Sensor(IServiceProvider serviceProvider)
    {
        if (_bme280 is not null)
        {
            return _bme280;
        }

        // ReSharper disable once InconsistentNaming
        IEnumerable<I2cConnectionSettings> i2cConnectionSettings = serviceProvider
            .GetServices<I2cConnectionSettings>();

        // ReSharper disable once HeapView.ObjectAllocation.Evident
        _bme280 = new Bme280Sensor(i2cConnectionSettings);

        return _bme280;
    }

    public bool CanInstall { get; } = true;

    public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // ReSharper disable once HeapView.DelegateAllocation
        serviceCollection.AddSingleton<IPressureSensor, Bme280Sensor>(RegisterBme280Sensor);
        // ReSharper disable once HeapView.DelegateAllocation
        serviceCollection.AddSingleton<IRelativeHumiditySensor, Bme280Sensor>(RegisterBme280Sensor);
        // ReSharper disable once HeapView.DelegateAllocation
        serviceCollection.AddSingleton<ITemperatureSensor, Bme280Sensor>(RegisterBme280Sensor);
    }
}
