using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class add_flag_accurate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoices_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailDownPayments");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailDownPayments",
                newName: "AccuSalesInvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailDownPayments",
                newName: "IX_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoiceId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccurate",
                table: "AccuItems",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoices_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailDownPayments",
                column: "AccuSalesInvoiceId",
                principalTable: "AccuSalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoices_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailDownPayments");

            migrationBuilder.DropColumn(
                name: "IsAccurate",
                table: "AccuItems");

            migrationBuilder.RenameColumn(
                name: "AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailDownPayments",
                newName: "AccuSalesInvoceId");

            migrationBuilder.RenameIndex(
                name: "IX_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoiceId",
                table: "AccuSalesInvoiceDetailDownPayments",
                newName: "IX_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccuSalesInvoiceDetailDownPayments_AccuSalesInvoices_AccuSalesInvoceId",
                table: "AccuSalesInvoiceDetailDownPayments",
                column: "AccuSalesInvoceId",
                principalTable: "AccuSalesInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
