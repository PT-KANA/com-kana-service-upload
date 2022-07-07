using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class Update_Sales_return : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "typeAutoNumber",
                table: "AccuSalesReturns",
                newName: "TypeAutoNumber");

            migrationBuilder.RenameColumn(
                name: "toAddress",
                table: "AccuSalesReturns",
                newName: "ToAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeAutoNumber",
                table: "AccuSalesReturns",
                newName: "typeAutoNumber");

            migrationBuilder.RenameColumn(
                name: "ToAddress",
                table: "AccuSalesReturns",
                newName: "toAddress");
        }
    }
}
