using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Project_3amal.Migrations
{
    /// <inheritdoc />
    public partial class location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                table: "ServiceRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "lat",
                table: "ServiceRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "lng",
                table: "ServiceRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "lat",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "ServiceRequests");
        }
    }
}
