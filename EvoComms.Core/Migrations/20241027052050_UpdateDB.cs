using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvoComms.Core.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutputTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutputTypes",
                columns: table => new
                {
                    OutputTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputTypes", x => x.OutputTypeId);
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
        }
    }
}
