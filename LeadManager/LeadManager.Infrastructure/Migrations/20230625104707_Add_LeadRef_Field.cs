using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLeadRefField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeadRef",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeadRef",
                table: "Leads");
        }
    }
}
