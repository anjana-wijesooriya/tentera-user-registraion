using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserRegistraion.Migrations
{
    /// <inheritdoc />
    public partial class Added_Verification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "MobileVerified",
                table: "Users",
                newName: "HasAcceptedPolicies");

            migrationBuilder.RenameColumn(
                name: "EmailVerified",
                table: "Users",
                newName: "EnableBiometric");

            migrationBuilder.AddColumn<int>(
                name: "VerificationId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Verifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MobilePin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailPin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    MobileVerified = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Verifications");

            migrationBuilder.DropColumn(
                name: "VerificationId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "HasAcceptedPolicies",
                table: "Users",
                newName: "MobileVerified");

            migrationBuilder.RenameColumn(
                name: "EnableBiometric",
                table: "Users",
                newName: "EmailVerified");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
