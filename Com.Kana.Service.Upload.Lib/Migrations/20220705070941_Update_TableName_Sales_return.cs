using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class Update_TableName_Sales_return : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesReturnDetailExpense_AccuSalesReturn_AccuSalesReturnId",
                table: "AccuSalesReturnDetailExpense");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesReturnDetailItem_AccuSalesReturn_AccuSalesReturnId",
                table: "AccuSalesReturnDetailItem");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesReturnDetailSerialNumber_AccuSalesReturnDetailItem_AccuSalesReturnDetailItemId",
                table: "AccuSalesReturnDetailSerialNumber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturnDetailSerialNumber",
                table: "AccuSalesReturnDetailSerialNumber");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturnDetailItem",
                table: "AccuSalesReturnDetailItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturnDetailExpense",
                table: "AccuSalesReturnDetailExpense");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturn",
                table: "AccuSalesReturn");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturnDetailSerialNumber",
                newName: "AccuSalesReturnDetailSerialNumbers");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturnDetailItem",
                newName: "AccuSalesReturnDetailItems");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturnDetailExpense",
                newName: "AccuSalesReturnDetailExpenses");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturn",
                newName: "AccuSalesReturns");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesReturnDetailSerialNumber_AccuSalesReturnDetailItemId",
                table: "AccuSalesReturnDetailSerialNumbers",
                newName: "IX_AccuSalesReturnDetailSerialNumbers_AccuSalesReturnDetailItemId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesReturnDetailItem_AccuSalesReturnId",
                table: "AccuSalesReturnDetailItems",
                newName: "IX_AccuSalesReturnDetailItems_AccuSalesReturnId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesReturnDetailExpense_AccuSalesReturnId",
                table: "AccuSalesReturnDetailExpenses",
                newName: "IX_AccuSalesReturnDetailExpenses_AccuSalesReturnId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturnDetailSerialNumbers",
                table: "AccuSalesReturnDetailSerialNumbers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturnDetailItems",
                table: "AccuSalesReturnDetailItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturnDetailExpenses",
                table: "AccuSalesReturnDetailExpenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturns",
                table: "AccuSalesReturns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesReturnDetailExpenses_AccuSalesReturns_AccuSalesReturnId",
                table: "AccuSalesReturnDetailExpenses",
                column: "AccuSalesReturnId",
                principalTable: "AccuSalesReturns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesReturnDetailItems_AccuSalesReturns_AccuSalesReturnId",
                table: "AccuSalesReturnDetailItems",
                column: "AccuSalesReturnId",
                principalTable: "AccuSalesReturns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesReturnDetailSerialNumbers_AccuSalesReturnDetailItems_AccuSalesReturnDetailItemId",
                table: "AccuSalesReturnDetailSerialNumbers",
                column: "AccuSalesReturnDetailItemId",
                principalTable: "AccuSalesReturnDetailItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesReturnDetailExpenses_AccuSalesReturns_AccuSalesReturnId",
                table: "AccuSalesReturnDetailExpenses");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesReturnDetailItems_AccuSalesReturns_AccuSalesReturnId",
                table: "AccuSalesReturnDetailItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesReturnDetailSerialNumbers_AccuSalesReturnDetailItems_AccuSalesReturnDetailItemId",
                table: "AccuSalesReturnDetailSerialNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturns",
                table: "AccuSalesReturns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturnDetailSerialNumbers",
                table: "AccuSalesReturnDetailSerialNumbers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturnDetailItems",
                table: "AccuSalesReturnDetailItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuSalesReturnDetailExpenses",
                table: "AccuSalesReturnDetailExpenses");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturns",
                newName: "AccuSalesReturn");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturnDetailSerialNumbers",
                newName: "AccuSalesReturnDetailSerialNumber");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturnDetailItems",
                newName: "AccuSalesReturnDetailItem");

            migrationBuilder.RenameTable(
                name: "AccuSalesReturnDetailExpenses",
                newName: "AccuSalesReturnDetailExpense");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesReturnDetailSerialNumbers_AccuSalesReturnDetailItemId",
                table: "AccuSalesReturnDetailSerialNumber",
                newName: "IX_AccuSalesReturnDetailSerialNumber_AccuSalesReturnDetailItemId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesReturnDetailItems_AccuSalesReturnId",
                table: "AccuSalesReturnDetailItem",
                newName: "IX_AccuSalesReturnDetailItem_AccuSalesReturnId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesReturnDetailExpenses_AccuSalesReturnId",
                table: "AccuSalesReturnDetailExpense",
                newName: "IX_AccuSalesReturnDetailExpense_AccuSalesReturnId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturn",
                table: "AccuSalesReturn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturnDetailSerialNumber",
                table: "AccuSalesReturnDetailSerialNumber",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturnDetailItem",
                table: "AccuSalesReturnDetailItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuSalesReturnDetailExpense",
                table: "AccuSalesReturnDetailExpense",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesReturnDetailExpense_AccuSalesReturn_AccuSalesReturnId",
                table: "AccuSalesReturnDetailExpense",
                column: "AccuSalesReturnId",
                principalTable: "AccuSalesReturn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesReturnDetailItem_AccuSalesReturn_AccuSalesReturnId",
                table: "AccuSalesReturnDetailItem",
                column: "AccuSalesReturnId",
                principalTable: "AccuSalesReturn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesReturnDetailSerialNumber_AccuSalesReturnDetailItem_AccuSalesReturnDetailItemId",
                table: "AccuSalesReturnDetailSerialNumber",
                column: "AccuSalesReturnDetailItemId",
                principalTable: "AccuSalesReturnDetailItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
