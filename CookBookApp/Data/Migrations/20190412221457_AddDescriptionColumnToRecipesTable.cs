using Microsoft.EntityFrameworkCore.Migrations;

namespace CookBookApp.Data.Migrations
{
    public partial class AddDescriptionColumnToRecipesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Recipes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Recipes");
        }
    }
}
