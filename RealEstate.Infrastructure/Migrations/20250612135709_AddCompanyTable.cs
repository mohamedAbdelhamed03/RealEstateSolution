using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RealEstate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                table: "estates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bedrooms",
                table: "estates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "estates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "estates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "Id", "City", "CreatedAt", "Email", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("839adb92-8619-4dc3-ba81-2e3bf8d40f6a"), "Dokki", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "eagle@gmail.com", "Eagle Realty", "+0987654321", "54321", "Giza", "456 Elm St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d14b8033-6397-4e77-bfcf-ef099e5204a5"), "Nasr City", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "falcon@gmai.com", "Falcon Real Estate", "+1234567890", "12345", "Cairo", "123 Main St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e24c7f38-5942-484b-9863-53ba26132a25"), "Sidi Gaber", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hawk@gmail.com", "Hawk Properties", "+1122334455", "67890", "Alexandria", "789 Oak St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.UpdateData(
                table: "estates",
                keyColumn: "Id",
                keyValue: new Guid("148e17f1-0e65-4ed2-8320-8b2183f1018a"),
                columns: new[] { "Bathrooms", "Bedrooms", "CompanyId", "Price" },
                values: new object[] { 0, 0, new Guid("839adb92-8619-4dc3-ba81-2e3bf8d40f6a"), 0.0 });

            migrationBuilder.UpdateData(
                table: "estates",
                keyColumn: "Id",
                keyValue: new Guid("64009858-0cfe-4cdd-b0b6-a17bd02ba400"),
                columns: new[] { "Bathrooms", "Bedrooms", "CompanyId", "Price" },
                values: new object[] { 0, 0, new Guid("e24c7f38-5942-484b-9863-53ba26132a25"), 0.0 });

            migrationBuilder.UpdateData(
                table: "estates",
                keyColumn: "Id",
                keyValue: new Guid("65958d38-6dc4-454e-9db4-a010506f9a8f"),
                columns: new[] { "Bathrooms", "Bedrooms", "CompanyId", "Price" },
                values: new object[] { 0, 0, new Guid("d14b8033-6397-4e77-bfcf-ef099e5204a5"), 0.0 });

            migrationBuilder.UpdateData(
                table: "estates",
                keyColumn: "Id",
                keyValue: new Guid("6f3811b2-fc13-4982-860a-0416af514635"),
                columns: new[] { "Bathrooms", "Bedrooms", "CompanyId", "Price" },
                values: new object[] { 0, 0, new Guid("839adb92-8619-4dc3-ba81-2e3bf8d40f6a"), 0.0 });

            migrationBuilder.UpdateData(
                table: "estates",
                keyColumn: "Id",
                keyValue: new Guid("a7fa520a-e357-4d25-ac34-b4cdf5edd48e"),
                columns: new[] { "Bathrooms", "Bedrooms", "CompanyId", "Price" },
                values: new object[] { 0, 0, new Guid("d14b8033-6397-4e77-bfcf-ef099e5204a5"), 0.0 });

            migrationBuilder.UpdateData(
                table: "estates",
                keyColumn: "Id",
                keyValue: new Guid("ce338791-bc56-4efa-923b-df4b0d88e589"),
                columns: new[] { "Bathrooms", "Bedrooms", "CompanyId", "Price" },
                values: new object[] { 0, 0, new Guid("e24c7f38-5942-484b-9863-53ba26132a25"), 0.0 });

            migrationBuilder.UpdateData(
                table: "estates",
                keyColumn: "Id",
                keyValue: new Guid("f929dd87-8c20-4cb9-b4a4-215b418ad2a1"),
                columns: new[] { "Bathrooms", "Bedrooms", "CompanyId", "Price" },
                values: new object[] { 0, 0, new Guid("d14b8033-6397-4e77-bfcf-ef099e5204a5"), 0.0 });

            migrationBuilder.CreateIndex(
                name: "IX_estates_CompanyId",
                table: "estates",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_estates_companies_CompanyId",
                table: "estates",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_estates_companies_CompanyId",
                table: "estates");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropIndex(
                name: "IX_estates_CompanyId",
                table: "estates");

            migrationBuilder.DropColumn(
                name: "Bathrooms",
                table: "estates");

            migrationBuilder.DropColumn(
                name: "Bedrooms",
                table: "estates");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "estates");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "estates");
        }
    }
}
