using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyGoParking.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addpointblack_countis_black : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "black_count",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_black",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "point",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "black_count",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "is_black",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "point",
                table: "AspNetUsers");
        }
    }
}
