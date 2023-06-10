using Iot.Device.Bmxx80;

namespace SPIN.Core.Sensors;

public sealed class Bme280Sensor :
    IPressureSensor,
    IRelativeHumiditySensor,
    ITemperatureSensor,
    IDisposable
{
    private I2cDevice _i2cDevice;
    private Bme280 _bme280;

    public Bme280Sensor(IEnumerable<I2cConnectionSettings> i2cConnectionSettings)
    {
        IEnumerable<I2cConnectionSettings> i2CConnectionSettingsEnumerable = i2cConnectionSettings as
            I2cConnectionSettings[] ??
            i2cConnectionSettings.ToArray();

        var noI2cConnectionSettings = !i2CConnectionSettingsEnumerable.Any();
        if (noI2cConnectionSettings)
        {
            throw new ArgumentException(nameof(i2cConnectionSettings));
        }

        for (var i = 0; i < i2CConnectionSettingsEnumerable.Count(); i++)
        {
            var i2cConnectionSetting = i2CConnectionSettingsEnumerable.ElementAt(i);
            i2cConnectionSetting = new(
                i2cConnectionSetting.DeviceAddress,
                Bmx280Base.DefaultI2cAddress);

            _i2cDevice = I2cDevice.Create(i2cConnectionSetting);
            try
            {
                _bme280 = new Bme280(_i2cDevice);
            }
            catch
            {
                _i2cDevice?.Dispose();
            }
        }
    }

    public async Task<Pressure> GetPressureAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bme280.TryReadPressure(out var pressure);
        return pressure;
    }

    public async Task<RelativeHumidity> GetRelativeHumidityAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bme280.TryReadHumidity(out var humidity);
        return humidity;
    }

    public async Task<Temperature> GetTemperatureAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bme280.TryReadTemperature(out var temperature);
        return temperature;
    }

    public void Dispose()
    {
        _i2cDevice?.Dispose();
        _bme280?.Dispose();
    }
}
