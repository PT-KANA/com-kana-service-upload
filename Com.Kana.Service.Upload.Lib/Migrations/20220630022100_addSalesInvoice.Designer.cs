﻿// <auto-generated />
using Com.Kana.Service.Upload.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Com.Kana.Service.Upload.Lib.Migrations
{
    [DbContext(typeof(UploadDbContext))]
    [Migration("20220630022100_addSalesInvoice")]
    partial class addSalesInvoice
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("CalculateGroupPrice");

                    b.Property<string>("CogsGIAccountNo");

                    b.Property<bool>("ControlQuality");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DefaultDiscount");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<string>("GoodTransitGIAccountNo");

                    b.Property<string>("InventoryGIAccountNo");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ItemCategoryName");

                    b.Property<string>("ItemType");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<bool>("ManageExpired");

                    b.Property<bool>("ManageSN");

                    b.Property<double>("MinimumQuantity");

                    b.Property<double>("MinimumQuantityReorder");

                    b.Property<string>("Name");

                    b.Property<string>("No");

                    b.Property<string>("Notes");

                    b.Property<double>("PercentTaxable");

                    b.Property<string>("PreferedVendorName");

                    b.Property<bool>("PrintDetailGroup");

                    b.Property<string>("PurchaseRetGIAccountNo");

                    b.Property<double>("Ratio2");

                    b.Property<double>("Ratio3");

                    b.Property<double>("Ratio4");

                    b.Property<double>("Ratio5");

                    b.Property<string>("SalesDiscountGIAccountNo");

                    b.Property<string>("SalesGIAccountNo");

                    b.Property<string>("SalesRetGIAccountNo");

                    b.Property<string>("SerialNumberType");

                    b.Property<bool>("Subtituted");

                    b.Property<string>("SubtitutedItemNo");

                    b.Property<string>("Tax1Name");

                    b.Property<string>("Tax2Name");

                    b.Property<string>("Tax3Name");

                    b.Property<string>("Tax4Name");

                    b.Property<double>("TypeAutoNumber");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.Property<string>("UnBilledGIAccountNo");

                    b.Property<string>("Unit1Name");

                    b.Property<string>("Unit2Name");

                    b.Property<double>("Unit2Price");

                    b.Property<string>("Unit3Name");

                    b.Property<double>("Unit3Price");

                    b.Property<string>("Unit4Name");

                    b.Property<double>("Unit4Price");

                    b.Property<string>("Unit5Name");

                    b.Property<double>("Unit5Price");

                    b.Property<double>("UnitPrice");

                    b.Property<string>("UpcNo");

                    b.Property<bool>("UsePPn");

                    b.Property<bool>("UseWholesalePrice");

                    b.Property<double>("VendorPrice");

                    b.Property<string>("VendorUnitName");

                    b.HasKey("Id");

                    b.ToTable("AccuItems");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItemDetailGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccuItemId");

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<string>("DetailName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ItemNo");

                    b.Property<string>("ItemUnitName");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<double>("Quantity");

                    b.Property<string>("Status");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("AccuItemId");

                    b.ToTable("AccuItemDetailGroups");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItemDetailOpenBalance", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccuItemId");

                    b.Property<bool>("Active");

                    b.Property<DateTimeOffset>("AsOf");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ItemUnitName");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<double>("Quantity");

                    b.Property<string>("Status");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.Property<double>("UnitCost");

                    b.Property<string>("WarehouseName");

                    b.HasKey("Id");

                    b.HasIndex("AccuItemId");

                    b.ToTable("AccuItemDetailOpenBalances");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItemDetailSerialNumber", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccuItemDetailOpenBalanceId");

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<DateTimeOffset>("ExpiredDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<double>("Quantity");

                    b.Property<string>("SerialNumberNo");

                    b.Property<string>("Status");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("AccuItemDetailOpenBalanceId");

                    b.ToTable("AccuItemDetailSerialNumbers");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<long>("BranchId");

                    b.Property<string>("BranchName");

                    b.Property<string>("CashDiscPercent");

                    b.Property<double>("CashDiscount");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("CurrencyCode");

                    b.Property<string>("CustomerNo");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<string>("Description");

                    b.Property<string>("DocumentCode");

                    b.Property<double>("FiscalRate");

                    b.Property<string>("FobName");

                    b.Property<bool>("InclusiveTax");

                    b.Property<double>("InputDownPayment");

                    b.Property<bool>("InvoiceDp");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<string>("Number");

                    b.Property<string>("OrderDownPaymentNumber");

                    b.Property<string>("PaymentTermName");

                    b.Property<string>("PoNumber");

                    b.Property<double>("Rate");

                    b.Property<string>("RetailIdCard");

                    b.Property<string>("RetailWpName");

                    b.Property<bool>("ReverseInvoice");

                    b.Property<DateTimeOffset>("ShipDate");

                    b.Property<string>("ShipmentName");

                    b.Property<string>("Tax1Name");

                    b.Property<DateTimeOffset>("TaxDate");

                    b.Property<string>("TaxNumber");

                    b.Property<string>("TaxType");

                    b.Property<bool>("Taxable");

                    b.Property<string>("ToAddress");

                    b.Property<DateTimeOffset>("TransDate");

                    b.Property<long>("TypeAutoNumber");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("AccuSalesInvoices");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailDownPayment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccuSalesInvoceId");

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<string>("InvoiceNumber");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<double>("PaymentAmount");

                    b.Property<string>("Status");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("AccuSalesInvoceId");

                    b.ToTable("AccuSalesInvoiceDetailDownPayments");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailExpense", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountNo");

                    b.Property<long>("AccuSalesInvoceId");

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<string>("DepartementName");

                    b.Property<double>("ExpenseAmount");

                    b.Property<string>("ExpenseName");

                    b.Property<string>("ExpenseNotes");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<string>("SalesOrderNumber");

                    b.Property<string>("SalesQuotationNumber");

                    b.Property<string>("Status");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("AccuSalesInvoceId");

                    b.ToTable("AccuSalesInvoiceDetailExpenses");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccuSalesInvoceId");

                    b.Property<bool>("Active");

                    b.Property<double>("ControlQuantity");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<string>("DeliveryOrderNumber");

                    b.Property<string>("DepartmentName");

                    b.Property<string>("DetailName");

                    b.Property<string>("DetailNotes");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("ItemCashDiscount");

                    b.Property<string>("ItemDiscPercent");

                    b.Property<string>("ItemNo");

                    b.Property<string>("ItemUnitName");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<string>("ProjectNo");

                    b.Property<double>("Quantity");

                    b.Property<string>("SalesOrderNumber");

                    b.Property<string>("SalesQuotationNumber");

                    b.Property<string>("SalesmanListNumber");

                    b.Property<string>("Status");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.Property<double>("UnitPrice");

                    b.Property<bool>("UseTax1");

                    b.Property<bool>("UseTax2");

                    b.Property<bool>("UseTax3");

                    b.Property<string>("WarehouseName");

                    b.HasKey("Id");

                    b.HasIndex("AccuSalesInvoceId");

                    b.ToTable("AccuSalesInvoiceDetailItems");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailSerialNumber", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AccuSalesInvoceDetailItemId");

                    b.Property<bool>("Active");

                    b.Property<string>("CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreatedUtc");

                    b.Property<string>("DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeletedUtc");

                    b.Property<DateTimeOffset>("ExpiredDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("LastModifiedUtc");

                    b.Property<double>("Quantity");

                    b.Property<string>("SerialNumberNo");

                    b.Property<string>("Status");

                    b.Property<string>("UId")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("AccuSalesInvoceDetailItemId");

                    b.ToTable("AccuSalesInvoiceDetailSerialNumbers");
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItemDetailGroup", b =>
                {
                    b.HasOne("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItem", "AccuItem")
                        .WithMany("DetailGroup")
                        .HasForeignKey("AccuItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItemDetailOpenBalance", b =>
                {
                    b.HasOne("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItem", "AccuItem")
                        .WithMany("DetailOpenBalance")
                        .HasForeignKey("AccuItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItemDetailSerialNumber", b =>
                {
                    b.HasOne("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel.AccuItemDetailOpenBalance", "DetailOpenBalance")
                        .WithMany("DetailSerialNumber")
                        .HasForeignKey("AccuItemDetailOpenBalanceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailDownPayment", b =>
                {
                    b.HasOne("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoice", "AccuSalesInvoice")
                        .WithMany("DetailDownPayment")
                        .HasForeignKey("AccuSalesInvoceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailExpense", b =>
                {
                    b.HasOne("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoice", "AccuSalesInvoice")
                        .WithMany("DetailExpense")
                        .HasForeignKey("AccuSalesInvoceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailItem", b =>
                {
                    b.HasOne("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoice", "AccuSalesInvoice")
                        .WithMany("DetailItem")
                        .HasForeignKey("AccuSalesInvoceId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailSerialNumber", b =>
                {
                    b.HasOne("Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel.AccuSalesInvoiceDetailItem", "AccuSalesInvoiceDetailItem")
                        .WithMany("DetailSerialNumber")
                        .HasForeignKey("AccuSalesInvoceDetailItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}