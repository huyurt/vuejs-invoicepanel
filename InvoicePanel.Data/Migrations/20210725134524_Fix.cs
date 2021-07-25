using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoicePanel.Data.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Products_ProductId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ProductInventorySnapshots");

            migrationBuilder.DropColumn(
                name: "IdealQuantity",
                table: "ProductInventorySnapshots");

            migrationBuilder.DropColumn(
                name: "QuantityOnHand",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "ProductInventorySnapshots",
                newName: "SnapshotTime");

            migrationBuilder.RenameColumn(
                name: "SnapshotTime",
                table: "Customers",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Customers",
                newName: "PrimaryAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_ProductId",
                table: "Customers",
                newName: "IX_Customers_PrimaryAddressId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Customers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customers",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerAddresses_PrimaryAddressId",
                table: "Customers",
                column: "PrimaryAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerAddresses_PrimaryAddressId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "SnapshotTime",
                table: "ProductInventorySnapshots",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "Customers",
                newName: "SnapshotTime");

            migrationBuilder.RenameColumn(
                name: "PrimaryAddressId",
                table: "Customers",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_PrimaryAddressId",
                table: "Customers",
                newName: "IX_Customers_ProductId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ProductInventorySnapshots",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "IdealQuantity",
                table: "ProductInventorySnapshots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityOnHand",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Products_ProductId",
                table: "Customers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
