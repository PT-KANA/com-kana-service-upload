using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class addSalesInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccuSalesInvoices",
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
                    Description = table.Column<string>(nullable: true),
                    DocumentCode = table.Column<string>(nullable: true),
                    FiscalRate = table.Column<double>(nullable: false),
                    FobName = table.Column<string>(nullable: true),
                    InclusiveTax = table.Column<bool>(nullable: false),
                    InputDownPayment = table.Column<double>(nullable: false),
                    InvoiceDp = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    OrderDownPaymentNumber = table.Column<string>(nullable: true),
                    PaymentTermName = table.Column<string>(nullable: true),
                    PoNumber = table.Column<string>(nullable: true),
                    Rate = table.Column<double>(nullable: false),
                    RetailIdCard = table.Column<string>(nullable: true),
                    RetailWpName = table.Column<string>(nullable: true),
                    ReverseInvoice = table.Column<bool>(nullable: false),
                    ShipDate = table.Column<DateTimeOffset>(nullable: false),
                    ShipmentName = table.Column<string>(nullable: true),
                    Tax1Name = table.Column<string>(nullable: true),
                    TaxDate = table.Column<DateTimeOffset>(nullable: false),
                    TaxNumber = table.Column<string>(nullable: true),
                    TaxType = table.Column<string>(nullable: true),
                    Taxable = table.Column<bool>(nullable: false),
                    ToAddress = table.Column<string>(nullable: true),
                    TransDate = table.Column<DateTimeOffset>(nullable: false),
                    TypeAutoNumber = table.Column<long>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuSalesInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccuSalesInvoiceDetailDownPayments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuSalesInvoceId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    PaymentAmount = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuSalesInvoiceDetailDownPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoices_AccuSalesInvoceId",
                        column: x => x.AccuSalesInvoceId,
                        principalTable: "AccuSalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccuSalesInvoiceDetailExpenses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountNo = table.Column<string>(nullable: true),
                    AccuSalesInvoceId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DepartementName = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_AccuSalesInvoiceDetailExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuSalesInvoiceDetailExpenses_AccuSalesInvoices_AccuSalesInvoceId",
                        column: x => x.AccuSalesInvoceId,
                        principalTable: "AccuSalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccuSalesInvoiceDetailItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuSalesInvoceId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ControlQuantity = table.Column<double>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeliveryOrderNumber = table.Column<string>(nullable: true),
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
                    SalesOrderNumber = table.Column<string>(nullable: true),
                    SalesQuotationNumber = table.Column<string>(nullable: true),
                    SalesmanListNumber = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_AccuSalesInvoiceDetailItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuSalesInvoiceDetailItems_AccuSalesInvoices_AccuSalesInvoceId",
                        column: x => x.AccuSalesInvoceId,
                        principalTable: "AccuSalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccuSalesInvoiceDetailSerialNumbers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuSalesInvoceDetailItemId = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_AccuSalesInvoiceDetailSerialNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoiceDetailItems_AccuSalesInvoceDetailItemId",
                        column: x => x.AccuSalesInvoceDetailItemId,
                        principalTable: "AccuSalesInvoiceDetailItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailDownPayments",
                column: "AccuSalesInvoceId");

            migrationBuilder.CreateIndex(
                name: "IX_AccuSalesInvoiceDetailExpenses_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailExpenses",
                column: "AccuSalesInvoceId");

            migrationBuilder.CreateIndex(
                name: "IX_AccuSalesInvoiceDetailItems_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailItems",
                column: "AccuSalesInvoceId");

            migrationBuilder.CreateIndex(
                name: "IX_AccuSalesInvoiceDetailSerialNumbers_AccuSalesInvoceDetailItemId",
                table: "AccuSalesInvoiceDetailSerialNumbers",
                column: "AccuSalesInvoceDetailItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccuSalesInvoiceDetailDownPayments");

            migrationBuilder.DropTable(
                name: "AccuSalesInvoiceDetailExpenses");

            migrationBuilder.DropTable(
                name: "AccuSalesInvoiceDetailSerialNumbers");

            migrationBuilder.DropTable(
                name: "AccuSalesInvoiceDetailItems");

            migrationBuilder.DropTable(
                name: "AccuSalesInvoices");
        }
    }
}
