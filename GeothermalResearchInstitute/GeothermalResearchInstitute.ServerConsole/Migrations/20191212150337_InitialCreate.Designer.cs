// <auto-generated />
using System;
using GeothermalResearchInstitute.ServerConsole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeothermalResearchInstitute.ServerConsole.Migrations
{
    [DbContext(typeof(BjdireContext))]
    [Migration("20191212150337_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("GeothermalResearchInstitute.ServerConsole.Models.Metric", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<float>("EnvironmentCelsiusDegree")
                        .HasColumnType("REAL");

                    b.Property<float>("HeaterOutputWaterCelsiusDegree")
                        .HasColumnType("REAL");

                    b.Property<float>("HeaterPowerKilowatt")
                        .HasColumnType("REAL");

                    b.Property<float>("InputWaterCelsiusDegree")
                        .HasColumnType("REAL");

                    b.Property<float>("InputWaterPressureMeter")
                        .HasColumnType("REAL");

                    b.Property<float>("OutputWaterCelsiusDegree")
                        .HasColumnType("REAL");

                    b.Property<float>("OutputWaterPressureMeter")
                        .HasColumnType("REAL");

                    b.Property<float>("WaterPumpFlowRateCubicMeterPerHour")
                        .HasColumnType("REAL");

                    b.HasKey("Id", "Timestamp");

                    b.ToTable("Metrics");
                });
#pragma warning restore 612, 618
        }
    }
}