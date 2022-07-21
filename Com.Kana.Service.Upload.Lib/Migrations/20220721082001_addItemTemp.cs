using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    public partial class addItemTemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccuItemTemp",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    No = table.Column<string>(nullable: true),
                    UId = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccuItemTemp", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccuItemTemp");
        }
    }
}
