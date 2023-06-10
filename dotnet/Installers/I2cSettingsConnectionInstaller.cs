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

using System.Text.RegularExpressions;

namespace SPIN.Core.Installers;

public sealed class I2cConnectionSettingsInstaller : IInstaller
{
    private static readonly Regex s_i2CRegexDetector = new Regex(@"(/dev/i2c)(-|/)(?<busId>\d)");
    private List<int> _busIds = new();

    public bool CanInstall
    {
        get
        {
            IEnumerable<string> filesInDev = Directory.EnumerateFiles(
                "/dev/",
                "*.*",
                SearchOption.AllDirectories);

            foreach (string file in filesInDev)
            {
                var notAnI2cFile = s_i2CRegexDetector.IsMatch(file);
                if (notAnI2cFile)
                {
                    continue;
                }

                Match matches = s_i2CRegexDetector.Match(file);
                var didNotParse = int.TryParse(matches.Groups["busId"].Value, out int busId);
                if (didNotParse)
                {
                    continue;
                }

                _busIds.Add(busId);
            }

            return _busIds.Count != 0;
        }
    }

    public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        foreach (int busId in _busIds)
        {
            serviceCollection.AddTransient<I2cConnectionSettings>((serviceProvider) =>
                new I2cConnectionSettings(busId, 0));
        }
    }
}
