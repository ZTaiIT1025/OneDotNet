﻿// <auto-generated />
using System;
using GeothermalResearchInstitute.ServerConsole.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeothermalResearchInstitute.ServerConsole.Migrations
{
    [DbContext(typeof(BjdireContext))]
    [Migration("20190915131054_SplitActualAndDesiredDevicesStates")]
    partial class SplitActualAndDesiredDevicesStates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("GeothermalResearchInstitute.ServerConsole.Model.DeviceActualStates", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("ColdCapacity");

                    b.Property<bool>("DevicePower");

                    b.Property<bool>("ExhaustPower");

                    b.Property<float>("FlowCapacity");

                    b.Property<bool>("HeatPumpAuto");

                    b.Property<bool>("HeatPumpCompressorOn");

                    b.Property<bool>("HeatPumpFanOn");

                    b.Property<bool>("HeatPumpFourWayReversingValue");

                    b.Property<bool>("HeatPumpPower");

                    b.Property<byte[]>("IPAddress");

                    b.Property<int>("MotorMode");

                    b.Property<float>("RateCapacity");

                    b.Property<float>("SummerTemperature");

                    b.Property<float>("WarmCapacity");

                    b.Property<int>("WaterPumpMode");

                    b.Property<float>("WinterTemperature");

                    b.Property<int>("WorkingMode");

                    b.HasKey("Id");

                    b.ToTable("DevicesActualStates");
                });

            modelBuilder.Entity("GeothermalResearchInstitute.ServerConsole.Model.DeviceDesiredStates", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("ColdCapacity");

                    b.Property<bool>("DevicePower");

                    b.Property<bool>("ExhaustPower");

                    b.Property<float>("FlowCapacity");

                    b.Property<bool>("HeatPumpAuto");

                    b.Property<bool>("HeatPumpCompressorOn");

                    b.Property<bool>("HeatPumpFanOn");

                    b.Property<bool>("HeatPumpFourWayReversingValue");

                    b.Property<bool>("HeatPumpPower");

                    b.Property<int>("MotorMode");

                    b.Property<float>("RateCapacity");

                    b.Property<float>("SummerTemperature");

                    b.Property<float>("WarmCapacity");

                    b.Property<int>("WaterPumpMode");

                    b.Property<float>("WinterTemperature");

                    b.Property<int>("WorkingMode");

                    b.HasKey("Id");

                    b.ToTable("DevicesDesiredStates");
                });
#pragma warning restore 612, 618
        }
    }
}
