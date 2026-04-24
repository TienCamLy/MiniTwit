using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SimulatorLatestState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SimulatorLatest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    latest_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulatorLatest", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SimulatorLatest",
                columns: new[] { "Id", "latest_id" },
                values: new object[] { 1, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimulatorLatest");
        }
    }
}
