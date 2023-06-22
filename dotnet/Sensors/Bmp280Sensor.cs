using Iot.Device.Bmxx80;

namespace SPIN.Core.Sensors;

public sealed class Bmp280Sensor :
    IPressureSensor,
    ITemperatureSensor,
    IDisposable
{
    private I2cDevice _i2cDevice;
    private Bmp280 _bmp280;

    public Bmp280Sensor(I2cConnectionSettings i2cConnectionSettings)
    {
        _i2cDevice = I2cDevice.Create(i2cConnectionSettings);
        try
        {
            _bmp280 = new Bmp280(_i2cDevice);
        }
        catch
        {
            _i2cDevice?.Dispose();
        }
    }

    public async Task<Pressure> GetPressureAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bmp280.TryReadPressure(out var pressure);
        return pressure;
    }

    public async Task<Temperature> GetTemperatureAsync(
        CancellationToken cancellationToken = default(CancellationToken))
    {
        _bmp280.TryReadTemperature(out var temperature);
        return temperature;
    }

    public void Dispose()
    {
        _i2cDevice?.Dispose();
        _bmp280?.Dispose();
    }
}
