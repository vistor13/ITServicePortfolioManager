using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ITServicePortfolioManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameAlgorithm = table.Column<string>(type: "text", nullable: false),
                    CountProvider = table.Column<int>(type: "integer", nullable: false),
                    TotalHumanResource = table.Column<int>(type: "integer", nullable: false),
                    Groups = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicePortfolioResults",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyIncome = table.Column<double>(type: "double precision", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: false),
                    Portfolio = table.Column<string>(type: "jsonb", nullable: true),
                    ProvidersIncome = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePortfolioResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePortfolioResults_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicePortfolioResults_TaskId",
                table: "ServicePortfolioResults",
                column: "TaskId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicePortfolioResults");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
