using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class change_Name_ItemTemps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuItemTemp",
                table: "AccuItemTemp");

            migrationBuilder.RenameTable(
                name: "AccuItemTemp",
                newName: "AccuItemTemps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuItemTemps",
                table: "AccuItemTemps",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AccuItemTemps",
                table: "AccuItemTemps");

            migrationBuilder.RenameTable(
                name: "AccuItemTemps",
                newName: "AccuItemTemp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccuItemTemp",
                table: "AccuItemTemp",
                column: "Id");
        }
    }
}
