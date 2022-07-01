using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class addISRefundColumninSalesInvoiceDetailItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRefund",
                table: "AccuSalesInvoiceDetailItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRefund",
                table: "AccuSalesInvoiceDetailItems");
        }
    }
}
