using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloPosProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixedSomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Ingredients_IngredientId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_IngredientId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CurrentStock",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "Ingredients");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Inventories",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStock",
                table: "Ingredients",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Ingredients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IngredientId",
                table: "Inventories",
                column: "IngredientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Category",
                table: "Ingredients",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Ingredients_IngredientId",
                table: "Inventories",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Ingredients_IngredientId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_IngredientId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Category",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_Name",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Ingredients");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Inventories",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "MinimumStock",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentStock",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "Ingredients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_IngredientId",
                table: "Inventories",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Ingredients_IngredientId",
                table: "Inventories",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
