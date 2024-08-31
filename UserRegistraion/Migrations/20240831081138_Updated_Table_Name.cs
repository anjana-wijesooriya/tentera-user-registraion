using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRegistraion.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Table_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Verifications",
                table: "Verifications");

            migrationBuilder.DropColumn(
                name: "VerificationId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Verifications",
                newName: "Otps");

            migrationBuilder.RenameIndex(
                name: "IX_Verifications_UserId",
                table: "Otps",
                newName: "IX_Otps_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Otps",
                table: "Otps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Otps_Users_UserId",
                table: "Otps",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Otps_Users_UserId",
                table: "Otps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Otps",
                table: "Otps");

            migrationBuilder.RenameTable(
                name: "Otps",
                newName: "Verifications");

            migrationBuilder.RenameIndex(
                name: "IX_Otps_UserId",
                table: "Verifications",
                newName: "IX_Verifications_UserId");

            migrationBuilder.AddColumn<int>(
                name: "VerificationId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Verifications",
                table: "Verifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Verifications_Users_UserId",
                table: "Verifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
