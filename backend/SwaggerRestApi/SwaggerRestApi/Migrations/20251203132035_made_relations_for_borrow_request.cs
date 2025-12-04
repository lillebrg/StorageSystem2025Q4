using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwaggerRestApi.Migrations
{
    /// <inheritdoc />
    public partial class made_relations_for_borrow_request : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecificItem",
                table: "BorrowRequests",
                newName: "SpecificItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRequests_LoanTo",
                table: "BorrowRequests",
                column: "LoanTo");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRequests_SpecificItemId",
                table: "BorrowRequests",
                column: "SpecificItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_SpecificItems_SpecificItemId",
                table: "BorrowRequests",
                column: "SpecificItemId",
                principalTable: "SpecificItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowRequests_Users_LoanTo",
                table: "BorrowRequests",
                column: "LoanTo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_SpecificItems_SpecificItemId",
                table: "BorrowRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowRequests_Users_LoanTo",
                table: "BorrowRequests");

            migrationBuilder.DropIndex(
                name: "IX_BorrowRequests_LoanTo",
                table: "BorrowRequests");

            migrationBuilder.DropIndex(
                name: "IX_BorrowRequests_SpecificItemId",
                table: "BorrowRequests");

            migrationBuilder.RenameColumn(
                name: "SpecificItemId",
                table: "BorrowRequests",
                newName: "SpecificItem");
        }
    }
}
