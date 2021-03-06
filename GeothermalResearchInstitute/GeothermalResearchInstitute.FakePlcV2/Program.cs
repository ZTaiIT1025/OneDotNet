// <copyright file="Program.cs" company="Shuai Zhang">
// Copyright Shuai Zhang. All rights reserved.
// Licensed under the GPLv3 license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace GeothermalResearchInstitute.FakePlcV2
{
    internal class Program
    {
        internal static async Task Main()
        {
            using var plc = new FakePlc(PhysicalAddress.Parse("10BF4879B2A4").GetAddressBytes());
            await plc.StartAsync(IPAddress.Loopback, 8889).ConfigureAwait(false);
            Console.ReadKey();
            await plc.StopAsync().ConfigureAwait(false);
        }
    }
}
