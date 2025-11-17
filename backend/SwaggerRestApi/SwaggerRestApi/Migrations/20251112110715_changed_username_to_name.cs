using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwaggerRestApi.Migrations
{
    /// <inheritdoc />
    public partial class changed_username_to_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "Users",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Name",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Roles");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }
    }
}
