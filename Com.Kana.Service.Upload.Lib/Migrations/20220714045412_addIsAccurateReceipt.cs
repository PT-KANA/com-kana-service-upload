using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class addIsAccurateReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccurateReceipt",
                table: "AccuSalesInvoices",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccurateReceipt",
                table: "AccuSalesInvoices");
        }
    }
}
