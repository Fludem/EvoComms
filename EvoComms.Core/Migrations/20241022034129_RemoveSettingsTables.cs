using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvoComms.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSettingsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "TimySettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsFirstRun = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingsId);
                });

            migrationBuilder.CreateTable(
                name: "TimySettings",
                columns: table => new
                {
                    TimySettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OutputTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    ListenPort = table.Column<int>(type: "INTEGER", nullable: false),
                    OutputPath = table.Column<string>(type: "TEXT", nullable: true)
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
                table: "Settings",
                columns: new[] { "SettingsId", "IsFirstRun" },
                values: new object[] { 1, true });

            migrationBuilder.InsertData(
                table: "TimySettings",
                columns: new[] { "TimySettingsId", "Enabled", "ListenPort", "OutputPath", "OutputTypeId" },
                values: new object[] { 1, true, 7788, "C:/Info", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_TimySettings_OutputTypeId",
                table: "TimySettings",
                column: "OutputTypeId");
        }
    }
}
