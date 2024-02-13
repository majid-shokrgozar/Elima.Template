using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiPay.Template.CoreModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Change_Sample_FullAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeleterId",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Samples",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Samples",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Samples",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifierId",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Samples");
        }
    }
}
