using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvoComms.Core.Migrations
{
    /// <inheritdoc />
    public partial class NewBaseModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    ClockingId = table.Column<int>(type: "INTEGER", nullable: false),
                    HanvonImg = table.Column<string>(type: "TEXT", nullable: true),
                    HanvonTemplate = table.Column<string>(type: "TEXT", nullable: true),
                    TimyImg = table.Column<string>(type: "TEXT", nullable: true),
                    TimyTemplate = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "OutputTypes",
                columns: table => new
                {
                    OutputTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputTypes", x => x.OutputTypeId);
                });

            migrationBuilder.CreateTable(
                name: "TimyTerminals",
                columns: table => new
                {
                    TimyTerminalId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ModelName = table.Column<string>(type: "TEXT", nullable: true),
                    MacAddress = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimyTerminals", x => x.TimyTerminalId);
                });

            migrationBuilder.CreateTable(
                name: "Clockings",
                columns: table => new
                {
                    ClockingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelName = table.Column<string>(type: "TEXT", nullable: true),
                    MacAddress = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clockings", x => x.ClockingId);
                    table.ForeignKey(
                        name: "FK_Clockings_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "TimySettings",
                columns: table => new
                {
                    TimySettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ListenPort = table.Column<int>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    OutputPath = table.Column<string>(type: "TEXT", nullable: true),
                    OutputTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimySettings", x => x.TimySettingsId);
                    table.ForeignKey(
                        name: "FK_TimySettings_OutputTypes_OutputTypeId",
                        column: x => x.OutputTypeId,
                        principalTable: "OutputTypes",
                        principalColumn: "OutputTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OutputTypes",
                columns: new[] { "OutputTypeId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "BioTime output type", "BioTime" },
                    { 2, "InTime output type", "InTime" },
                    { 3, "InfoTime output type", "InfoTime" }
                });

            migrationBuilder.InsertData(
                table: "TimySettings",
                columns: new[] { "TimySettingsId", "Enabled", "ListenPort", "OutputPath", "OutputTypeId" },
                values: new object[] { 1, true, 8080, "default/path", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Clockings_EmployeeId",
                table: "Clockings",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimySettings_OutputTypeId",
                table: "TimySettings",
                column: "OutputTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clockings");

            migrationBuilder.DropTable(
                name: "TimySettings");

            migrationBuilder.DropTable(
                name: "TimyTerminals");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "OutputTypes");
        }
    }
}
