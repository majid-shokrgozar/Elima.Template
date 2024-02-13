using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigiPay.Template.FirstModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Change_Sample_FullAuditLogAggregation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Samples",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Samples");
        }
    }
}
