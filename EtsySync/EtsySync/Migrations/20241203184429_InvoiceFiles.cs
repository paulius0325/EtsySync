using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtsySync.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Buyer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "EncryptionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncryptionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemNumber = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.InvoiceId);
                });

            migrationBuilder.CreateTable(
                name: "ZipFiles",
                columns: table => new
                {
                    ZipFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipFiles", x => x.ZipFileId);
                });

            migrationBuilder.CreateTable(
                name: "SalesItems",
                columns: table => new
                {
                    SalesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InvoiceItemInvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesItems", x => x.SalesId);
                    table.ForeignKey(
                        name: "FK_SalesItems_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId");
                    table.ForeignKey(
                        name: "FK_SalesItems_InvoiceItems_InvoiceItemInvoiceId",
                        column: x => x.InvoiceItemInvoiceId,
                        principalTable: "InvoiceItems",
                        principalColumn: "InvoiceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_ClientId",
                table: "SalesItems",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_InvoiceItemInvoiceId",
                table: "SalesItems",
                column: "InvoiceItemInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncryptionKeys");

            migrationBuilder.DropTable(
                name: "SalesItems");

            migrationBuilder.DropTable(
                name: "ZipFiles");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "InvoiceItems");
        }
    }
}
