using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiPay.Template.CoreModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_CreationAudit_To_Sample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Samples");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Samples");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Samples");

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreateDate",
                table: "Samples",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CreateTime",
                table: "Samples",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
