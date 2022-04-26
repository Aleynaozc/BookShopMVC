using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShopMvc.Migrations
{
    public partial class RemoveCategoryInBookModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
