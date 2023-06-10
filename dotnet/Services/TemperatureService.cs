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

namespace SPIN.Core.Services;

public sealed class TemperatureService : ITemperatureService
{
    private ITemperatureSensor? _temperatureSensor;

    public TemperatureService(ITemperatureSensor? temperatureSensor = null)
    {
        _temperatureSensor = temperatureSensor;
    }

    public async Task<GetTemperatureResponse> Handle(GetTemperatureRequest request, CancellationToken cancellationToken = default(CancellationToken))
    {
        Console.WriteLine("Requested temperature");

        var noTemperatureSensor = (_temperatureSensor is null);
        if (noTemperatureSensor)
        {
            return new GetTemperatureResponse
            {
                RequestId = request.Id,
                Status = SensorResponseStatus.NoSensor
            };
        }

        try
        {
            var temperature = await _temperatureSensor!.GetTemperatureAsync();

            return new GetTemperatureResponse
            {
                RequestId = request.Id,
                Status = SensorResponseStatus.Ok,
                Value = temperature
            };
        }
        catch
        {
            return new GetTemperatureResponse
            {
                RequestId = request.Id,
                Status = SensorResponseStatus.SensorProblem
            };
        }
    }
}
