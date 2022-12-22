using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeadManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedtestleadsinDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Leads",
                columns: new[] { "LeadId", "Description", "Name", "SourceId", "SupplierId" },
                values: new object[,]
                {
                    { 4, "Lead4 with Source1 and Supplier1", "Lead4", 1, 1 },
                    { 5, "Lead5 with Source1 and Supplier1", "Lead5", 1, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Leads",
                keyColumn: "LeadId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Leads",
                keyColumn: "LeadId",
                keyValue: 5);
        }
    }
}
