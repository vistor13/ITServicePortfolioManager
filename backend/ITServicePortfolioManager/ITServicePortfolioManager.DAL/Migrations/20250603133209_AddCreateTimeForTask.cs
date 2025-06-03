using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITServicePortfolioManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCreateTimeForTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DELETE FROM \"ServicePortfolioResults\";");
			migrationBuilder.Sql("DELETE FROM \"Tasks\";");
			migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tasks");
        }
    }
}
