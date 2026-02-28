using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloPosProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fixedtables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tables_TableId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TableId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TableId1",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TableId1",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TableId1",
                table: "Orders",
                column: "TableId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tables_TableId1",
                table: "Orders",
                column: "TableId1",
                principalTable: "Tables",
                principalColumn: "Id");
        }
    }
}
