using Microsoft.EntityFrameworkCore.Migrations;

namespace CookBookApp.Data.Migrations
{
    public partial class FixIngredientCategoryNamePropertyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "IngredientCategories",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "IngredientCategories",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
