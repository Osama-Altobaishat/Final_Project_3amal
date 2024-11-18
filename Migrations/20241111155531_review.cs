using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project_3amal.Migrations
{
    /// <inheritdoc />
    public partial class review : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingAvg",
                table: "Reviews");

            migrationBuilder.AddColumn<decimal>(
                name: "RatingAvg",
                table: "Services",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingAvg",
                table: "Services");

            migrationBuilder.AddColumn<decimal>(
                name: "RatingAvg",
                table: "Reviews",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
