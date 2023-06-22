using Iot.Device.Bmxx80;

namespace SPIN.Core.Sensors;

public sealed class Bme680Sensor :
    IPressureSensor,
    IRelativeHumiditySensor,
    ITemperatureSensor,
    IDisposable
{
    private I2cDevice _i2cDevice;
    private Bme680 _bme680;

    public Bme680Sensor(I2cConnectionSettings i2cConnectionSettings)
    {
        _i2cDevice = I2cDevice.Create(i2cConnectionSettings);
        try
        {
            _bme680 = new Bme680(_i2cDevice);
        }
        catch
        {
            _i2cDevice?.Dispose();
        }
    }

    public async Task<Pressure> GetPressureAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bme680.TryReadPressure(out var pressure);
        return pressure;
    }

    public async Task<RelativeHumidity> GetRelativeHumidityAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bme680.TryReadHumidity(out var humidity);
        return humidity;
    }

    public async Task<Temperature> GetTemperatureAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bme680.TryReadTemperature(out var temperature);
        return temperature;
    }

    public void Dispose()
    {
        _i2cDevice?.Dispose();
        _bme680?.Dispose();
    }
}
