using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerceASPNetCore.Data.Migrations
{
    public partial class category_subcate_status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "SubCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Categories");
        }
    }
}
