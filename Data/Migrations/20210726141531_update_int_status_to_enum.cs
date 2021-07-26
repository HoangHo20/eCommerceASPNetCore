using Microsoft.EntityFrameworkCore.Migrations;

namespace eCommerceASPNetCore.Data.Migrations
{
    public partial class update_int_status_to_enum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "ProductImages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "ProductImages");
        }
    }
}
