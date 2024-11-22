using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvoComms.Core.Migrations
{
    /// <inheritdoc />
    public partial class CorrectTimySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TimySettings",
                keyColumn: "TimySettingsId",
                keyValue: 1,
                columns: new[] { "ListenPort", "OutputPath" },
                values: new object[] { 7788, "C:/Info" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TimySettings",
                keyColumn: "TimySettingsId",
                keyValue: 1,
                columns: new[] { "ListenPort", "OutputPath" },
                values: new object[] { 8080, "default/path" });
        }
    }
}
