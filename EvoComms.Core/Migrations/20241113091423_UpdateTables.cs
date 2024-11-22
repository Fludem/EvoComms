using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvoComms.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clockings_Employees_EmployeeId",
                table: "Clockings");

            migrationBuilder.DropTable(
                name: "TimyTerminals");

            migrationBuilder.DropColumn(
                name: "HanvonImg",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "HanvonTemplate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TimyImg",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TimyTemplate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "MacAddress",
                table: "Clockings");

            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "Clockings");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Employees",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Clockings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClockedAt",
                table: "Clockings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ClockingMachineId",
                table: "Clockings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedAt",
                table: "Clockings",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ClockingMachines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClockingMachines", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clockings_ClockingMachineId",
                table: "Clockings",
                column: "ClockingMachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clockings_ClockingMachines_ClockingMachineId",
                table: "Clockings",
                column: "ClockingMachineId",
                principalTable: "ClockingMachines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Clockings_Employees_EmployeeId",
                table: "Clockings",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clockings_ClockingMachines_ClockingMachineId",
                table: "Clockings");

            migrationBuilder.DropForeignKey(
                name: "FK_Clockings_Employees_EmployeeId",
                table: "Clockings");

            migrationBuilder.DropTable(
                name: "ClockingMachines");

            migrationBuilder.DropIndex(
                name: "IX_Clockings_ClockingMachineId",
                table: "Clockings");

            migrationBuilder.DropColumn(
                name: "ClockedAt",
                table: "Clockings");

            migrationBuilder.DropColumn(
                name: "ClockingMachineId",
                table: "Clockings");

            migrationBuilder.DropColumn(
                name: "ReceivedAt",
                table: "Clockings");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "name");

            migrationBuilder.AddColumn<string>(
                name: "HanvonImg",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HanvonTemplate",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimyImg",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimyTemplate",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Clockings",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "MacAddress",
                table: "Clockings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "Clockings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TimyTerminals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MacAddress = table.Column<string>(type: "TEXT", nullable: true),
                    ModelName = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimyTerminals", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Clockings_Employees_EmployeeId",
                table: "Clockings",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
