using Microsoft.EntityFrameworkCore.Migrations;

namespace CookBookApp.Data.Migrations
{
    public partial class IngredientInRecipesTableChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsInRecipes_Recipes_RecipeId",
                table: "IngredientsInRecipes");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "IngredientsInRecipes",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsInRecipes_Recipes_RecipeId",
                table: "IngredientsInRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsInRecipes_Recipes_RecipeId",
                table: "IngredientsInRecipes");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "IngredientsInRecipes",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsInRecipes_Recipes_RecipeId",
                table: "IngredientsInRecipes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
