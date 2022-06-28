using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class addMasteritem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccuItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CalculateGroupPrice = table.Column<bool>(nullable: false),
                    CogsGIAccountNo = table.Column<string>(nullable: true),
                    ControlQuality = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DefaultDiscount = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    GoodTransitGIAccountNo = table.Column<string>(nullable: true),
                    InventoryGIAccountNo = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemCategoryName = table.Column<string>(nullable: true),
                    ItemType = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    ManageExpired = table.Column<bool>(nullable: false),
                    ManageSN = table.Column<bool>(nullable: false),
                    MinimumQuantity = table.Column<double>(nullable: false),
                    MinimumQuantityReorder = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    No = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    PercentTaxable = table.Column<double>(nullable: false),
                    PreferedVendorName = table.Column<string>(nullable: true),
                    PrintDetailGroup = table.Column<bool>(nullable: false),
                    PurchaseRetGIAccountNo = table.Column<string>(nullable: true),
                    Ratio2 = table.Column<double>(nullable: false),
                    Ratio3 = table.Column<double>(nullable: false),
                    Ratio4 = table.Column<double>(nullable: false),
                    Ratio5 = table.Column<double>(nullable: false),
                    SalesDiscountGIAccountNo = table.Column<string>(nullable: true),
                    SalesGIAccountNo = table.Column<string>(nullable: true),
                    SalesRetGIAccountNo = table.Column<string>(nullable: true),
                    SerialNumberType = table.Column<string>(nullable: true),
                    Subtituted = table.Column<bool>(nullable: false),
                    SubtitutedItemNo = table.Column<string>(nullable: true),
                    Tax1Name = table.Column<string>(nullable: true),
                    Tax2Name = table.Column<string>(nullable: true),
                    Tax3Name = table.Column<string>(nullable: true),
                    Tax4Name = table.Column<string>(nullable: true),
                    TypeAutoNumber = table.Column<double>(nullable: false),
                    UId = table.Column<string>(maxLength: 255, nullable: true),
                    UnBilledGIAccountNo = table.Column<string>(nullable: true),
                    Unit1Name = table.Column<string>(nullable: true),
                    Unit2Name = table.Column<string>(nullable: true),
                    Unit2Price = table.Column<double>(nullable: false),
                    Unit3Name = table.Column<string>(nullable: true),
                    Unit3Price = table.Column<double>(nullable: false),
                    Unit4Name = table.Column<string>(nullable: true),
                    Unit4Price = table.Column<double>(nullable: false),
                    Unit5Name = table.Column<string>(nullable: true),
                    Unit5Price = table.Column<double>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    UpcNo = table.Column<string>(nullable: true),
                    UsePPn = table.Column<bool>(nullable: false),
                    UseWholesalePrice = table.Column<bool>(nullable: false),
                    VendorPrice = table.Column<double>(nullable: false),
                    VendorUnitName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccuItemDetailGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuItemId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DetailName = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemNo = table.Column<string>(nullable: true),
                    ItemUnitName = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuItemDetailGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuItemDetailGroups_AccuItems_AccuItemId",
                        column: x => x.AccuItemId,
                        principalTable: "AccuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccuItemDetailOpenBalances",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuItemId = table.Column<long>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    AsOf = table.Column<DateTimeOffset>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemUnitName = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true),
                    UnitCost = table.Column<double>(nullable: false),
                    WarehouseName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuItemDetailOpenBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuItemDetailOpenBalances_AccuItems_AccuItemId",
                        column: x => x.AccuItemId,
                        principalTable: "AccuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccuItemDetailSerialNumbers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccuItemDetailOpenBalanceId = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_AccuItemDetailSerialNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccuItemDetailSerialNumbers_AccuItemDetailOpenBalances_AccuItemDetailOpenBalanceId",
                        column: x => x.AccuItemDetailOpenBalanceId,
                        principalTable: "AccuItemDetailOpenBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccuItemDetailGroups_AccuItemId",
                table: "AccuItemDetailGroups",
                column: "AccuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AccuItemDetailOpenBalances_AccuItemId",
                table: "AccuItemDetailOpenBalances",
                column: "AccuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AccuItemDetailSerialNumbers_AccuItemDetailOpenBalanceId",
                table: "AccuItemDetailSerialNumbers",
                column: "AccuItemDetailOpenBalanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccuItemDetailGroups");

            migrationBuilder.DropTable(
                name: "AccuItemDetailSerialNumbers");

            migrationBuilder.DropTable(
                name: "AccuItemDetailOpenBalances");

            migrationBuilder.DropTable(
                name: "AccuItems");
        }
    }
}
