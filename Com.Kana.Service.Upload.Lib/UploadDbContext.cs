using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Moonlay.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Com.Kana.Service.Upload.Lib
{
    public class UploadDbContext : StandardDbContext
    {
        public UploadDbContext(DbContextOptions<UploadDbContext> options) : base(options)
        {
            if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                Database.SetCommandTimeout(1000 * 60 * 20);
        }

        #region items
        public DbSet<AccuItem> AccuItems { get; set; }
        public DbSet<AccuItemDetailGroup> AccuItemDetailGroups { get; set; }
        public DbSet<AccuItemDetailOpenBalance> AccuItemDetailOpenBalances { get; set; }
        public DbSet<AccuItemDetailSerialNumber> AccuItemDetailSerialNumbers { get; set; }
        #endregion

        #region sales-invoice
        public DbSet<AccuSalesInvoice> AccuSalesInvoices { get; set; }
        public DbSet<AccuSalesInvoiceDetailDownPayment> AccuSalesInvoiceDetailDownPayments { get; set; }
        public DbSet<AccuSalesInvoiceDetailExpense> AccuSalesInvoiceDetailExpenses { get; set; }
        public DbSet<AccuSalesInvoiceDetailItem> AccuSalesInvoiceDetailItems { get; set; }
        public DbSet<AccuSalesInvoiceDetailSerialNumber> AccuSalesInvoiceDetailSerialNumbers { get; set; }
        #endregion

        #region sales-return
        public DbSet<AccuSalesReturn> AccuSalesReturns { get; set; }
        public DbSet<AccuSalesReturnDetailExpense> AccuSalesReturnDetailExpenses { get; set; }
        public DbSet<AccuSalesReturnDetailItem> AccuSalesReturnDetailItems { get; set; }
        public DbSet<AccuSalesReturnDetailSerialNumber> AccuSalesReturnDetailSerialNumbers { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new PurchasingDocumentExpeditionConfig());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //modelBuilder.Entity<GarmentPurchaseRequest>()
            //    .HasIndex(i => i.PRNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentPurchaseRequest>()
            //    .HasIndex(i => i.RONo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentExternalPurchaseOrder>()
            //    .HasIndex(i => i.EPONo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentInternNote>()
            //    .HasIndex(i => i.INNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentUnitReceiptNote>()
            //    .HasIndex(i => i.URNNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-04 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentReceiptCorrection>()
            //    .HasIndex(i => i.CorrectionNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentUnitDeliveryOrder>()
            //    .HasIndex(i => i.UnitDONo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentUnitExpenditureNote>()
            //    .HasIndex(i => i.UENNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentCorrectionNote>()
            //    .HasIndex(i => i.CorrectionNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");

            #region Purchasing

            //modelBuilder.Entity<PurchaseRequest>()
            //    .HasIndex(i => i.No)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2020-02-01 00:00:00.0000000')");

            //modelBuilder.Entity<ExternalPurchaseOrder>()
            //    .HasIndex(i => i.EPONo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2020-02-01 00:00:00.0000000')");

            //modelBuilder.Entity<UnitReceiptNote>()
            //    .HasIndex(i => i.URNNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2020-02-01 00:00:00.0000000')");

            //modelBuilder.Entity<UnitPaymentOrder>()
            //    .HasIndex(i => i.UPONo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2020-02-01 00:00:00.0000000')");

            //modelBuilder.Entity<UnitPaymentCorrectionNote>()
            //    .HasIndex(i => i.UPCNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2020-02-01 00:00:00.0000000')");

            //modelBuilder.Entity<GarmentDispositionPurchase>()
            //    .HasIndex(i => i.DispositionNo)
            //    .IsUnique()
            //    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2020-02-01 00:00:00.0000000')");
            #endregion

            #region indexes
            //modelBuilder.Entity<GarmentInternNote>()
            //    .HasIndex(i => new { i.INDate, i.CurrencyId, i.CurrencyCode, i.SupplierId });

            //modelBuilder.Entity<GarmentInvoice>()
            //    .HasIndex(i => new { i.CurrencyId, i.IncomeTaxId, i.SupplierId, i.InvoiceDate, i.IncomeTaxDate });

            //modelBuilder.Entity<GarmentInvoiceItem>()
            //    .HasIndex(i => new { i.ArrivalDate, i.DeliveryOrderId, i.DeliveryOrderNo, i.InvoiceId });

            //modelBuilder.Entity<GarmentInvoiceDetail>()
            //    .HasIndex(i => new { i.EPOId, i.IPOId, i.PRItemId, i.DODetailId, i.ProductId, i.UomId });

            //modelBuilder.Entity<GarmentDeliveryOrder>()
            //    .HasIndex(i => new { i.ArrivalDate, i.CustomsId, i.DOCurrencyId, i.DODate, i.IncomeTaxId, i.SupplierId });

            //modelBuilder.Entity<GarmentDeliveryOrderItem>()
            //    .HasIndex(i => new { i.CurrencyId, i.EPOId, i.GarmentDOId });

            //modelBuilder.Entity<GarmentDeliveryOrderDetail>()
            //    .HasIndex(i => new { i.POId, i.PRId, i.ProductId });

            //modelBuilder.Entity<GarmentExternalPurchaseOrder>()
            //    .HasIndex(i => new { i.CurrencyId, i.IncomeTaxId, i.SupplierId, i.UENId });

            //modelBuilder.Entity<GarmentExternalPurchaseOrderItem>()
            //    .HasIndex(i => new { i.POId, i.PRId });

            //modelBuilder.Entity<GarmentInternalPurchaseOrderItem>()
            //   .HasIndex(i => new { i.CategoryId, i.GPOId, i.ProductId });
            #endregion

            #region BalanceStock
            ////modelBuilder.Entity<BalanceStock>()
            ////    .HasIndex(i => i.BalanceStockId)
            ////    .IsUnique()
            ////    .HasFilter("[IsDeleted]=(0) AND [CreatedUtc]>CONVERT([datetime2],'2019-10-01 00:00:00.0000000')");
            //modelBuilder.Entity<BalanceStock>()
            //    .Property(i => i.OpenPrice)
            //    .HasColumnType("Money");
            //modelBuilder.Entity<BalanceStock>()
            //    .Property(i => i.DebitPrice)
            //    .HasColumnType("Money");
            //modelBuilder.Entity<BalanceStock>()
            //    .Property(i => i.CreditPrice)
            //    .HasColumnType("Money");
            //modelBuilder.Entity<BalanceStock>()
            //    .Property(i => i.ClosePrice)
            //    .HasColumnType("Money");
            #endregion
        }
    }
}
