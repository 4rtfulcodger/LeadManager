using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeadManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeeddatabaseusingOnModelCreating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "SourceId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Source1 description", "Source1" },
                    { 2, "Source2 description", "Source2" },
                    { 3, "Source3 description", "Source3" }
                });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Supplier1 description", "Supplier1" },
                    { 2, "Supplier2 description", "Supplier2" },
                    { 3, "Supplier3 description", "Supplier3" }
                });

            migrationBuilder.InsertData(
                table: "Leads",
                columns: new[] { "LeadId", "Description", "Name", "SourceId", "SupplierId" },
                values: new object[,]
                {
                    { 1, "Lead1 with Source1 and Supplier1", "Lead1", 1, 1 },
                    { 2, "Lead2 with Source2 and Supplier2", "Lead2", 2, 2 },
                    { 3, "Lead3 with Source3 and Supplier3", "Lead3", 3, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Leads",
                keyColumn: "LeadId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Leads",
                keyColumn: "LeadId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Leads",
                keyColumn: "LeadId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "SourceId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "SourceId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "SourceId",
                keyValue: 3);
        }
    }
}
