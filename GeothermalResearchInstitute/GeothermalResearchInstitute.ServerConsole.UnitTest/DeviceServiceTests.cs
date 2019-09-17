// <copyright file="DeviceServiceTests.cs" company="Shuai Zhang">
// Copyright Shuai Zhang. All rights reserved.
// Licensed under the GPLv3 license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeothermalResearchInstitute.ServerConsole.GrpcServices;
using GeothermalResearchInstitute.ServerConsole.Models;
using GeothermalResearchInstitute.v1;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Core.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeothermalResearchInstitute.ServerConsole.UnitTest
{
    [TestClass]
    public class DeviceServiceTests
    {
        private IHost Host { get; set; }

        [TestInitialize]
        public void Init()
        {
            this.Host = new HostBuilder()
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "Environment", "UnitTest" },
                    });
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "devices:0:id", "10:BF:48:79:B2:A4" },
                        { "devices:0:name", "测试设备0" },
                        { "devices:1:id", "BC:96:80:E6:70:16" },
                        { "devices:1:name", "测试设备1" },
                    });
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddProvider(new MsTestLoggerProvider());
                })
                .ConfigureServices((context, builder) =>
                {
                    IConfiguration config = context.Configuration;

                    builder.AddDbContext<BjdireContext>(options => options.UseInMemoryDatabase("bjdire"));

                    // Configuration options.
                    builder.Configure<DeviceOptions>(context.Configuration);

                    // Grpc services.
                    builder.AddSingleton(serviceProvider =>
                    {
                        return new GrpcLoggerAdapater.GrpcLoggerAdapter(
                            serviceProvider.GetRequiredService<ILoggerFactory>(),
                            serviceProvider.GetRequiredService<ILogger<Server>>());
                    });
                    builder.AddSingleton<DeviceServiceImpl>();
                })
                .Build();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.Host.Dispose();
            this.Host = null;
        }

        [TestMethod]
        public async Task ListDevices()
        {
            var service = this.Host.Services.GetRequiredService<DeviceServiceImpl>();
            var fakeServerCallContext = TestServerCallContext.Create(
                nameof(service.ListDevices),
                null,
                DateTime.UtcNow.AddHours(1),
                new Metadata(),
                CancellationToken.None,
                null,
                null,
                null,
                (metadata) => Task.CompletedTask,
                () => new WriteOptions(),
                (writeOptions) => { });
            var response = await service.ListDevices(new ListDevicesRequest(), fakeServerCallContext).ConfigureAwait(false);

            CollectionAssert.AreEqual(
                new List<Device>
                {
                    new Device
                    {
                        Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                        Name = "测试设备0",
                    },
                    new Device
                    {
                        Id = ByteString.CopyFrom(new byte[] { 0xBC, 0x96, 0x80, 0xE6, 0x70, 0x16 }),
                        Name = "测试设备1",
                    },
                },
                response.Devices);
        }

        [TestMethod]
        public async Task GetDeviceInvalidId()
        {
            var service = this.Host.Services.GetRequiredService<DeviceServiceImpl>();
            var fakeServerCallContext = TestServerCallContext.Create(
                nameof(service.GetDevice),
                null,
                DateTime.UtcNow.AddHours(1),
                new Metadata(),
                CancellationToken.None,
                null,
                null,
                null,
                (metadata) => Task.CompletedTask,
                () => new WriteOptions(),
                (writeOptions) => { });

            var rpcException = await Assert
                .ThrowsExceptionAsync<RpcException>(() => service.GetDevice(
                    new GetDeviceRequest
                    {
                        Id = ByteString.CopyFromUtf8("NOT_EXIST"),
                        View = DeviceView.NameOnly,
                    },
                    fakeServerCallContext))
                .ConfigureAwait(false);
            Assert.AreEqual(StatusCode.NotFound, rpcException.StatusCode);
        }

        [TestMethod]
        public async Task GetDeviceNameOnly()
        {
            var service = this.Host.Services.GetRequiredService<DeviceServiceImpl>();
            var fakeServerCallContext = TestServerCallContext.Create(
                nameof(service.GetDevice),
                null,
                DateTime.UtcNow.AddHours(1),
                new Metadata(),
                CancellationToken.None,
                null,
                null,
                null,
                (metadata) => Task.CompletedTask,
                () => new WriteOptions(),
                (writeOptions) => { });

            var response = await service
                .GetDevice(
                    new GetDeviceRequest
                    {
                        Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                        View = DeviceView.NameOnly,
                    },
                    fakeServerCallContext)
                .ConfigureAwait(false);
            Assert.AreEqual(
                new Device
                {
                    Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                    Name = "测试设备0",
                },
                response);
        }

        [TestMethod]
        public async Task GetDeviceWorkingModeOnlyNonExisting()
        {
            var service = this.Host.Services.GetRequiredService<DeviceServiceImpl>();
            var fakeServerCallContext = TestServerCallContext.Create(
                nameof(service.GetDevice),
                null,
                DateTime.UtcNow.AddHours(1),
                new Metadata(),
                CancellationToken.None,
                null,
                null,
                null,
                (metadata) => Task.CompletedTask,
                () => new WriteOptions(),
                (writeOptions) => { });

            var response = await service
                .GetDevice(
                    new GetDeviceRequest
                    {
                        Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                        View = DeviceView.WorkingModeOnly,
                    },
                    fakeServerCallContext)
                .ConfigureAwait(false);
            Assert.AreEqual(
                new Device
                {
                    Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                    WorkingMode = DeviceWorkingMode.Unspecified,
                },
                response);
        }

        [TestMethod]
        public async Task GetDeviceWorkingModeOnlyExisting()
        {
            var bjdireContext = this.Host.Services.GetRequiredService<BjdireContext>();
            var actualStates = new DeviceActualStates
            {
                Id = new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 },
                WorkingMode = DeviceWorkingMode.KeepWarmCapacity,
            };
            bjdireContext.DevicesActualStates.Add(actualStates);
            bjdireContext.SaveChanges();

            var service = this.Host.Services.GetRequiredService<DeviceServiceImpl>();
            var fakeServerCallContext = TestServerCallContext.Create(
                nameof(service.GetDevice),
                null,
                DateTime.UtcNow.AddHours(1),
                new Metadata(),
                CancellationToken.None,
                null,
                null,
                null,
                (metadata) => Task.CompletedTask,
                () => new WriteOptions(),
                (writeOptions) => { });

            var response = await service
                .GetDevice(
                    new GetDeviceRequest
                    {
                        Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                        View = DeviceView.WorkingModeOnly,
                    },
                    fakeServerCallContext)
                .ConfigureAwait(false);
            Assert.AreEqual(
                new Device
                {
                    Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                    WorkingMode = DeviceWorkingMode.KeepWarmCapacity,
                },
                response);
        }

        [TestMethod]
        public async Task UpdateDeviceWorkingModeNonExisting()
        {
            var service = this.Host.Services.GetRequiredService<DeviceServiceImpl>();
            var fakeServerCallContext = TestServerCallContext.Create(
                nameof(service.UpdateDevice),
                null,
                DateTime.UtcNow.AddHours(1),
                new Metadata(),
                CancellationToken.None,
                null,
                null,
                null,
                (metadata) => Task.CompletedTask,
                () => new WriteOptions(),
                (writeOptions) => { });

            var response = await service
                .UpdateDevice(
                    new UpdateDeviceRequest
                    {
                        Device = new Device
                        {
                            Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                            WorkingMode = DeviceWorkingMode.MeasureTemperature,
                        },
                        UpdateMask = Google.Protobuf.WellKnownTypes.FieldMask.FromString("working_mode"),
                    },
                    fakeServerCallContext)
                .ConfigureAwait(false);

            Assert.AreEqual(
                new Device
                {
                    Id = ByteString.CopyFrom(new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 }),
                    WorkingMode = DeviceWorkingMode.MeasureTemperature,
                },
                response);

            var bjdireContext = this.Host.Services.GetRequiredService<BjdireContext>();
            Assert.AreEqual(
                new DeviceDesiredStates
                {
                    Id = new byte[] { 0x10, 0xBF, 0x48, 0x79, 0xB2, 0xA4 },
                    WorkingMode = DeviceWorkingMode.MeasureTemperature,
                },
                bjdireContext.DevicesDesiredStates.SingleOrDefault());
        }
    }
}