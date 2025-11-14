using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwaggerRestApi.Migrations
{
    /// <inheritdoc />
    public partial class changed_some_variables_to_nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnLoanItems",
                table: "Users",
                newName: "BorrowedItems");

            migrationBuilder.RenameColumn(
                name: "OnLoanTo",
                table: "SpecificItems",
                newName: "BorrowedTo");

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "BaseItems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<long>(
                name: "ModelBarcode",
                table: "BaseItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BorrowedItems",
                table: "Users",
                newName: "OnLoanItems");

            migrationBuilder.RenameColumn(
                name: "BorrowedTo",
                table: "SpecificItems",
                newName: "OnLoanTo");

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "BaseItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModelBarcode",
                table: "BaseItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
