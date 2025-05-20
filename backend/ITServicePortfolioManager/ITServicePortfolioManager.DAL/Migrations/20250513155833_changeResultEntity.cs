using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITServicePortfolioManager.DAL.Migrations
{
    public partial class changeResultEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            ALTER TABLE ""ServicePortfolioResults""
            ALTER COLUMN ""ProvidersIncome"" TYPE double precision[]
            USING (ARRAY[]::double precision[]); 
        ");
            
            migrationBuilder.AlterColumn<double[]>(
                name: "ProvidersIncome",
                table: "ServicePortfolioResults",
                type: "double precision[]",
                nullable: false,
                defaultValue: new double[0],
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProvidersIncome",
                table: "ServicePortfolioResults",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(double[]),
                oldType: "double precision[]");
        }
    }

}
