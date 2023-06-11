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

using System.Runtime.InteropServices;

namespace SPIN.Core.Installers;

// ReSharper disable once InconsistentNaming
public sealed class I2cConnectionSettingsInstaller : IInstaller
{
    // ReSharper disable once InconsistentNaming
    private static bool I2cBusExists(int busId)
    {
        // ReSharper disable once InconsistentNaming
        bool i2cBusExists = File.Exists($"/dev/i2c-{busId}") || File.Exists($"/dev/i2c/{busId}");

        return i2cBusExists;
    }

    public ulong Priority
    {
        get
        {
            return 1;
        }
    }

    public bool CanInstall(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        bool notLinux = !RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        if (notLinux)
        {
            return false;
        }

        for (int i = 0; i <= 255; i++)
        {
            // ReSharper disable once InconsistentNaming
            bool i2cBusExists = I2cBusExists(i);

            if (i2cBusExists)
            {
                return true;
            }
        }

        return false;
    }

    public void InstallService(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // ReSharper disable once HeapView.ClosureAllocation
        // ReSharper disable once HeapView.ObjectAllocation.Possible
        foreach (int busId in Enumerable.Range(0, 255))
        {
            // ReSharper disable once InconsistentNaming
            bool i2cBusDoesntExist = !I2cBusExists(busId);

            if (i2cBusDoesntExist)
            {
                continue;
            }

            // ReSharper disable once HeapView.DelegateAllocation
            serviceCollection.AddTransient<I2cConnectionSettings>(provider =>
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                new I2cConnectionSettings(busId, default));
        }
    }
}
