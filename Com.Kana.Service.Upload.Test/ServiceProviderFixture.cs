using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Facades.InternalPO;
using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.Facades.Report;
using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Serializers;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.IntegrationViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.PurchaseOrder;
using Com.Kana.Service.Upload.Lib.ViewModels.UnitReceiptNote;
using Com.Kana.Service.Upload.Test.DataUtils.ExpeditionDataUtil;
using Com.Kana.Service.Upload.Test.DataUtils.PurchaseRequestDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.InternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.UnitReceiptNote;
using Com.Kana.Service.Upload.Test.Helpers;
using Com.Kana.Service.Upload.WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using Xunit;
using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade;
using Com.Kana.Service.Upload.Test.DataUtils.ExternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.DeliveryOrderDataUtils;
using Com.Kana.Service.Upload.Lib.Facades.BankExpenditureNoteFacades;
using Com.Kana.Service.Upload.Test.DataUtils.BankExpenditureNoteDataUtils;
using Com.Kana.Service.Upload.Lib.Facades.UnitReceiptNoteFacade;
using Com.Kana.Service.Upload.Test.DataUtils.UnitReceiptNoteDataUtils;
using Com.Kana.Service.Upload.Lib.ViewModels.Expedition;
using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade.Reports;
using Com.Kana.Service.Upload.Lib.Facades.UnitPaymentCorrectionNoteFacade;
using Com.Kana.Service.Upload.Test.DataUtils.UnitPaymentCorrectionNoteDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.UnitPaymentOrderDataUtils;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInvoiceFacades;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentInvoiceDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentInternNoteDataUtils;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternNoteFacades;
using Com.Kana.Service.Upload.Lib.Facades.PurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCentralBillReceptionFacades;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCentralBillExpenditureFacades;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCorrectionNoteReceptionFacades;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCorrectionNoteExpenditureFacades;
using Com.Kana.Service.Upload.Lib.Utilities.Currencies;

namespace Com.Kana.Service.Upload.Test
{
    public class ServiceProviderFixture
    {
        public IServiceProvider ServiceProvider { get; private set; }

        private void RegisterEndpoints(IConfigurationRoot Configuration)
        {
            APIEndpoint.Purchasing = Configuration.GetValue<string>(Constant.PURCHASING_ENDPOINT) ?? Configuration[Constant.PURCHASING_ENDPOINT];
        }

        private void RegisterSerializationProvider()
        {
            BsonSerializer.RegisterSerializationProvider(new SerializationProvider());
        }

        private void RegisterClassMap()
        {
            ClassMap<UnitReceiptNoteViewModel>.Register();
            ClassMap<UnitReceiptNoteItemViewModel>.Register();
            ClassMap<UnitViewModel>.Register();
            ClassMap<DivisionViewModel>.Register();
            ClassMap<CategoryViewModel>.Register();
            ClassMap<ProductViewModel>.Register();
            ClassMap<UomViewModel>.Register();
            ClassMap<PurchaseOrderViewModel>.Register();
            ClassMap<SupplierViewModel>.Register();
            ClassMap<UnitPaymentOrderUnpaidViewModel>.Register();
        }

        public ServiceProviderFixture()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(Constant.SECRET, "DANLIRISDEVELOPMENT"),
                    new KeyValuePair<string, string>("ASPNETCORE_ENVIRONMENT", "Test"),
                    //new KeyValuePair<string, string>(Constant.DEFAULT_CONNECTION, "Server=(localdb)\\mssqllocaldb;Database=com-danliris-db-test;Trusted_Connection=True;MultipleActiveResultSets=true"),
                    new KeyValuePair<string, string>(Constant.DEFAULT_CONNECTION, "Server=localhost,1401;Database=com.danliris.db.purchasing.service.test;User Id=sa;Password=Standar123.;MultipleActiveResultSets=True;"),
                    new KeyValuePair<string, string>(Constant.MONGODB_CONNECTION, "mongodb://localhost:27017/admin")
                })
                .Build();

            RegisterEndpoints(configuration);
            string connectionString = configuration.GetConnectionString(Constant.DEFAULT_CONNECTION) ?? configuration[Constant.DEFAULT_CONNECTION];
			APIEndpoint.ConnectionString = configuration.GetConnectionString(Constant.DEFAULT_CONNECTION) ?? configuration[Constant.DEFAULT_CONNECTION];

			this.ServiceProvider = new ServiceCollection()
                .AddDbContext<UploadDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(connectionString);
                }, ServiceLifetime.Transient)
                .AddTransient<PurchasingDocumentExpeditionFacade>()
                .AddTransient<PurchasingDocumentExpeditionReportFacade>()
                .AddTransient<ImportPurchasingBookReportFacade>()
                .AddTransient<LocalPurchasingBookReportFacade>()
                .AddTransient<SendToVerificationDataUtil>()

                .AddTransient<UnitPaymentOrderUnpaidReportFacade>()
                .AddTransient<UnitPaymentOrderNotVerifiedReportFacade>()
                .AddTransient<PurchasingDocumentAcceptanceDataUtil>()
                .AddTransient<UnitReceiptNoteBsonDataUtil>()
                .AddTransient<UnitPaymentOrderUnpaidReportDataUtil>()
                .AddTransient<UnitReceiptNoteImportFalseBsonDataUtil>()

                .AddTransient<PurchaseRequestFacade>()
                .AddTransient<PurchaseRequestDataUtil>()
                .AddTransient<PurchaseRequestItemDataUtil>()
                .AddTransient<PurchaseOrderMonitoringAllFacade>()
                .AddTransient<MonitoringPriceFacade>()
                .AddTransient<PurchaseRequestGenerateDataFacade>()
                .AddTransient<ExternalPurchaseOrderGenerateDataFacade>()
                .AddTransient<UnitReceiptNoteGenerateDataFacade>()

                .AddTransient<InternalPurchaseOrderFacade>()
                .AddTransient<InternalPurchaseOrderDataUtil>()
                .AddTransient<InternalPurchaseOrderItemDataUtil>()

                .AddTransient<ExternalPurchaseOrderFacade>()
                .AddTransient<ExternalPurchaseOrderDataUtil>()
                .AddTransient<ExternalPurchaseOrderItemDataUtil>()
                .AddTransient<ExternalPurchaseOrderDetailDataUtil>()

                .AddTransient<DeliveryOrderFacade>()
                .AddTransient<IDeliveryOrderFacade, DeliveryOrderFacade>()
                .AddTransient<DeliveryOrderDataUtil>()
                .AddTransient<DeliveryOrderItemDataUtil>()
                .AddTransient<DeliveryOrderDetailDataUtil>()

                .AddTransient<BankExpenditureNoteFacade>()
                .AddTransient<BankExpenditureNoteDataUtil>()

                //.AddTransient<UnitReceiptNoteFacade>()
                .AddTransient<UnitReceiptNoteFacade>()
                .AddTransient<UnitReceiptNoteDataUtil>()
                .AddTransient<UnitReceiptNoteItemDataUtil>()
				.AddTransient<TotalPurchaseFacade>()
				.AddTransient<ImportPurchasingBookReportFacade>()
				.AddTransient<IGarmentInvoice,GarmentInvoiceFacade>()
				.AddTransient<GarmentInvoiceDataUtil>()
				.AddTransient<GarmentInvoiceItemDataUtil>()
				.AddTransient<GarmentInvoiceDetailDataUtil>()

                .AddTransient<GarmentInternNoteFacades>()
                .AddTransient<GarmentInternNoteDataUtil>()

                .AddTransient<IUnitPaymentOrderFacade, UnitPaymentOrderFacade>()
                .AddTransient<UnitPaymentOrderDataUtil>()
                .AddTransient<IUnitPaymentPriceCorrectionNoteFacade, UnitPaymentPriceCorrectionNoteFacade>()
                .AddTransient<IMonitoringCentralBillReceptionFacade, MonitoringCentralBillReceptionFacade>()
                .AddTransient<IMonitoringCentralBillExpenditureFacade, MonitoringCentralBillExpenditureFacade>()
                .AddTransient<IMonitoringCorrectionNoteReceptionFacade, MonitoringCorrectionNoteReceptionFacade>()
                .AddTransient<IMonitoringCorrectionNoteExpenditureFacade, MonitoringCorrectionNoteExpenditureFacade>()
                .AddTransient<UnitPaymentPriceCorrectionNoteDataUtils>()
                .AddTransient<UnitPaymentCorrectionNoteDataUtil>()
				.AddSingleton<IHttpClientService, HttpClientTestService>()
				.AddSingleton<ICurrencyProvider, CurrencyProvider>()
                .AddSingleton<IdentityService>()
                .BuildServiceProvider();

            RegisterSerializationProvider();
            RegisterClassMap();
            MongoDbContext.connectionString = configuration.GetConnectionString(Constant.MONGODB_CONNECTION) ?? configuration[Constant.MONGODB_CONNECTION];

            UploadDbContext dbContext = ServiceProvider.GetService<UploadDbContext>();
            if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                dbContext.Database.Migrate();
            }
        }     
    }

    [CollectionDefinition("ServiceProviderFixture Collection")]
    public class ServiceProviderFixtureCollection : ICollectionFixture<ServiceProviderFixture>
    {
    }
}
