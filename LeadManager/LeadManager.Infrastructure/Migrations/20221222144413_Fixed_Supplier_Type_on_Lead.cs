using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedSupplierTypeonLead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Sources_SupplierId",
                table: "Leads");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Suppliers_SupplierId",
                table: "Leads",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Suppliers_SupplierId",
                table: "Leads");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Sources_SupplierId",
                table: "Leads",
                column: "SupplierId",
                principalTable: "Sources",
                principalColumn: "SourceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
