// <copyright file="FakeDevicesHostedService.cs" company="Shuai Zhang">
// Copyright Shuai Zhang. All rights reserved.
// Licensed under the GPLv3 license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GeothermalResearchInstitute.FakePlcV2;
using GeothermalResearchInstitute.ServerConsole.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GeothermalResearchInstitute.ServerConsole
{
    [SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Instantiated with reflection.")]
    internal class FakeDevicesHostedService : IHostedService, IDisposable
    {
        private readonly IConfiguration rootConfig;
        private readonly FakePlc[] fakePlcList;
        private bool disposedValue = false;

        public FakeDevicesHostedService(
            IConfiguration rootConfig,
            IOptions<DeviceOptions> deviceOptions)
        {
            this.rootConfig = rootConfig;
            this.fakePlcList = deviceOptions.Value.Devices
                .Select(d => new FakePlc(d.ComputeIdBinary()))
                .Take(rootConfig.GetValue<int>("core:fake_device_num"))
                .ToArray();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            int port = this.rootConfig.GetValue<int>("core:plc_port");
            foreach (FakePlc plc in this.fakePlcList)
            {
                await plc.StartAsync(IPAddress.Loopback, port).ConfigureAwait(false);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (FakePlc plc in this.fakePlcList)
            {
                await plc.StopAsync().ConfigureAwait(true);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    foreach (FakePlc plc in this.fakePlcList)
                    {
                        plc.StopAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                        plc.Dispose();
                    }
                }

                this.disposedValue = true;
            }
        }
    }
}
