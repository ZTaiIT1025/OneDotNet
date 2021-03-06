// <copyright file="20191214052037_InitialCreate.cs" company="Shuai Zhang">
// Copyright Shuai Zhang. All rights reserved.
// Licensed under the GPLv3 license. See LICENSE file in the project root for full license information.
// </copyright>

using Microsoft.EntityFrameworkCore.Migrations;

namespace GeothermalResearchInstitute.ServerConsole.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "AlarmChanges",
                columns: table => new
                {
                    DeviceId = table.Column<string>(nullable: false),
                    Timestamp = table.Column<long>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    Direction = table.Column<byte>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmChanges", x => new { x.DeviceId, x.Timestamp, x.Type });
                });

            migrationBuilder.CreateTable(
                name: "Alarms",
                columns: table => new
                {
                    DeviceId = table.Column<string>(nullable: false),
                    Timestamp = table.Column<long>(nullable: false),
                    LowFlowRate = table.Column<bool>(nullable: false),
                    HighHeaterPressure = table.Column<bool>(nullable: false),
                    LowHeaterPressure = table.Column<bool>(nullable: false),
                    NoPower = table.Column<bool>(nullable: false),
                    HeaterOverloadedBroken = table.Column<bool>(nullable: false),
                    ElectricalHeaterBroken = table.Column<bool>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alarms", x => new { x.DeviceId, x.Timestamp });
                });

            migrationBuilder.CreateTable(
                name: "Metrics",
                columns: table => new
                {
                    DeviceId = table.Column<string>(nullable: false),
                    Timestamp = table.Column<long>(nullable: false),
                    OutputWaterCelsiusDegree = table.Column<float>(nullable: false),
                    InputWaterCelsiusDegree = table.Column<float>(nullable: false),
                    HeaterOutputWaterCelsiusDegree = table.Column<float>(nullable: false),
                    EnvironmentCelsiusDegree = table.Column<float>(nullable: false),
                    OutputWaterPressureMeter = table.Column<float>(nullable: false),
                    InputWaterPressureMeter = table.Column<float>(nullable: false),
                    HeaterPowerKilowatt = table.Column<float>(nullable: false),
                    WaterPumpFlowRateCubicMeterPerHour = table.Column<float>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metrics", x => new { x.DeviceId, x.Timestamp });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "AlarmChanges");

            migrationBuilder.DropTable(
                name: "Alarms");

            migrationBuilder.DropTable(
                name: "Metrics");
        }
    }
}
