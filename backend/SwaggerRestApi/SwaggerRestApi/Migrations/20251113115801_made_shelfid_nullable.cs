using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwaggerRestApi.Migrations
{
    /// <inheritdoc />
    public partial class made_shelfid_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_Shelves_ShelfId",
                table: "BaseItems");

            migrationBuilder.AlterColumn<int>(
                name: "ShelfId",
                table: "BaseItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_Shelves_ShelfId",
                table: "BaseItems",
                column: "ShelfId",
                principalTable: "Shelves",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_Shelves_ShelfId",
                table: "BaseItems");

            migrationBuilder.AlterColumn<int>(
                name: "ShelfId",
                table: "BaseItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_Shelves_ShelfId",
                table: "BaseItems",
                column: "ShelfId",
                principalTable: "Shelves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
