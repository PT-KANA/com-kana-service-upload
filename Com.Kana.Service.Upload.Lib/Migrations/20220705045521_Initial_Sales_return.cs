using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class Initial_Sales_return : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailExpenses_AccuSalesInvoices_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailItems_AccuSalesInvoices_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoiceDetailItems_AccuSalesInvoceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers",
                newName: "AccuSalesInvoiceDetailItemId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers",
                newName: "IX_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoiceDetailItemId");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailItems",
                newName: "AccuSalesInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailItems_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailItems",
                newName: "IX_AccuSalesInvoiceDetailItems_AccuSalesInvoiceId");

            migrationBuilder.RenameColumn(
                name: "DepartementName",
                table: "AccuSalesInvoiceDetailExpenses",
                newName: "DepartmentName");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailExpenses",
                newName: "AccuSalesInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailExpenses_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailExpenses",
                newName: "IX_AccuSalesInvoiceDetailExpenses_AccuSalesInvoiceId");

            migrationBuilder.CreateTable(
                name: "AccuSalesReturn",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    BranchId = table.Column<long>(nullable: false),
                    BranchName = table.Column<string>(nullable: true),
                    CashDiscPercent = table.Column<string>(nullable: true),
                    CashDiscount = table.Column<double>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CurrencyCode = table.Column<string>(nullable: true),
                    CustomerNo = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeliveryOrderNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FiscalRate = table.Column<double>(nullable: false),
                    FobName = table.Column<string>(nullable: true),
                    InclusiveTax = table.Column<bool>(nullable: false),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    PaymentTermName = table.Column<string>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    ReturnType = table.Column<string>(nullable: true),
                    ShipmentName = table.Column<string>(nullable: true),
                    TaxDate = table.Column<DateTimeOffset>(nullable: false),
                    TaxNumber = table.Column<string>(nullable: true),
                    Taxable = table.Column<bool>(nullable: false),
                    TransDate = table.Column<DateTimeOffset>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true),
                    toAddress = table.Column<string>(nullable: true),
                    typeAutoNumber = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuSalesReturn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccuSalesReturnDetailExpense",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountNo = table.Column<string>(nullable: true),
                    AccuSalesReturnId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true),
                    ExpenseAmount = table.Column<double>(nullable: false),
                    ExpenseName = table.Column<string>(nullable: true),
                    ExpenseNotes = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    SalesOrderNumber = table.Column<string>(nullable: true),
                    SalesQuotationNumber = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuSalesReturnDetailExpense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuSalesReturnDetailExpense_AccuSalesReturn_AccuSalesReturnId",
                        column: x => x.AccuSalesReturnId,
                        principalTable: "AccuSalesReturn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccuSalesReturnDetailItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuSalesReturnId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true),
                    DetailName = table.Column<string>(nullable: true),
                    DetailNotes = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemCashDiscount = table.Column<double>(nullable: false),
                    ItemDiscPercent = table.Column<string>(nullable: true),
                    ItemNo = table.Column<string>(nullable: true),
                    ItemUnitName = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    ProjectNo = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true),
                    UnitPrice = table.Column<double>(nullable: false),
                    UseTax1 = table.Column<bool>(nullable: false),
                    UseTax2 = table.Column<bool>(nullable: false),
                    UseTax3 = table.Column<bool>(nullable: false),
                    WarehouseName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuSalesReturnDetailItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuSalesReturnDetailItem_AccuSalesReturn_AccuSalesReturnId",
                        column: x => x.AccuSalesReturnId,
                        principalTable: "AccuSalesReturn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccuSalesReturnDetailSerialNumber",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuSalesReturnDetailItemId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    ExpiredDate = table.Column<DateTimeOffset>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    SerialNumberNo = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuSalesReturnDetailSerialNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuSalesReturnDetailSerialNumber_AccuSalesReturnDetailItem_AccuSalesReturnDetailItemId",
                        column: x => x.AccuSalesReturnDetailItemId,
                        principalTable: "AccuSalesReturnDetailItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccuSalesReturnDetailExpense_AccuSalesReturnId",
                table: "AccuSalesReturnDetailExpense",
                column: "AccuSalesReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_AccuSalesReturnDetailItem_AccuSalesReturnId",
                table: "AccuSalesReturnDetailItem",
                column: "AccuSalesReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_AccuSalesReturnDetailSerialNumber_AccuSalesReturnDetailItemId",
                table: "AccuSalesReturnDetailSerialNumber",
                column: "AccuSalesReturnDetailItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailExpenses_AccuSalesInvoices_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailExpenses",
                column: "AccuSalesInvoiceId",
                principalTable: "AccuSalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailItems_AccuSalesInvoices_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailItems",
                column: "AccuSalesInvoiceId",
                principalTable: "AccuSalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoiceDetailItems_AccuSalesInvoiceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers",
                column: "AccuSalesInvoiceDetailItemId",
                principalTable: "AccuSalesInvoiceDetailItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailExpenses_AccuSalesInvoices_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailItems_AccuSalesInvoices_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoiceDetailItems_AccuSalesInvoiceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers");

            migrationBuilder.DropTable(
                name: "AccuSalesReturnDetailExpense");

            migrationBuilder.DropTable(
                name: "AccuSalesReturnDetailSerialNumber");

            migrationBuilder.DropTable(
                name: "AccuSalesReturnDetailItem");

            migrationBuilder.DropTable(
                name: "AccuSalesReturn");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoiceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers",
                newName: "AccuSalesInvoceDetailItemId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoiceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers",
                newName: "IX_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoceDetailItemId");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailItems",
                newName: "AccuSalesInvoceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailItems_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailItems",
                newName: "IX_AccuSalesInvoiceDetailItems_AccuSalesInvoceId");

            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "AccuSalesInvoiceDetailExpenses",
                newName: "DepartementName");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailExpenses",
                newName: "AccuSalesInvoceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailExpenses_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailExpenses",
                newName: "IX_AccuSalesInvoiceDetailExpenses_AccuSalesInvoceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailExpenses_AccuSalesInvoices_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailExpenses",
                column: "AccuSalesInvoceId",
                principalTable: "AccuSalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailItems_AccuSalesInvoices_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailItems",
                column: "AccuSalesInvoceId",
                principalTable: "AccuSalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoiceDetailItems_AccuSalesInvoceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers",
                column: "AccuSalesInvoceDetailItemId",
                principalTable: "AccuSalesInvoiceDetailItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
