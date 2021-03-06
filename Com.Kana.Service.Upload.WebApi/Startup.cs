using AutoMapper;
using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Facades.BankExpenditureNoteFacades;
using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade;
using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade.Reports;
using Com.Kana.Service.Upload.Lib.Facades.GarmentBeacukaiFacade;
using Com.Kana.Service.Upload.Lib.Facades.GarmentCorrectionNoteFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentDeliveryOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternNoteFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInvoiceFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentUnitDeliveryOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentUnitReceiptNoteFacades;
using Com.Kana.Service.Upload.Lib.Facades.InternalPO;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringUnitReceiptFacades;
using Com.Kana.Service.Upload.Lib.Facades.PurchasingDispositionFacades;
using Com.Kana.Service.Upload.Lib.Facades.Report;
using Com.Kana.Service.Upload.Lib.Facades.UnitPaymentCorrectionNoteFacade;
using Com.Kana.Service.Upload.Lib.Facades.UnitReceiptNoteFacade;
using Com.Kana.Service.Upload.Lib.Facades.GarmentUnitExpenditureNoteFacade;
using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Serializers;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.IntegrationViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.PurchaseOrder;
using Com.Kana.Service.Upload.Lib.ViewModels.UnitPaymentCorrectionNoteViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.UnitReceiptNote;
using Com.Kana.Service.Upload.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Serialization;
using System.Text;
using Com.Kana.Service.Upload.Lib.Facades.GarmentExternalPurchaseOrderFacade.Reports;
using Com.Kana.Service.Upload.Lib.Facades.PurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentUnitDeliveryOrderReturFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentReceiptCorrectionFacades;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCentralBillReceptionFacades;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCentralBillExpenditureFacades;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCorrectionNoteReceptionFacades;
using FluentScheduler;
using Com.Kana.Service.Upload.WebApi.SchedulerJobs;
using Com.Kana.Service.Upload.Lib.Utilities.CacheManager;
using Com.Kana.Service.Upload.Lib.Facades.MonitoringCorrectionNoteExpenditureFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPOMasterDistributionFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentReports;
using Com.Kana.Service.Upload.Lib.Utilities.Currencies;
using Com.Kana.Service.Upload.Lib.Facades.GarmentDailyPurchasingReportFacade;
using Com.Kana.Service.Upload.Lib.AutoMapperProfiles;
using Com.Kana.Service.Upload.Lib.Utilities;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;
using Com.Kana.Service.Upload.Lib.Facades.PRMasterValidationReportFacade;
using Com.Kana.Service.Upload.Lib.Facades.GarmentExternalPurchaseOrderFacades.Reports;
using Com.Kana.Service.Upload.Lib.Facades.GarmentSupplierBalanceDebtFacades;
using Com.Kana.Service.Upload.Lib.Facades.VBRequestPOExternal;
using Com.Kana.Service.Upload.Lib.Facades.GarmentStockOpnameFacades;
using Com.Kana.Service.Upload.Lib.Facades.DebtAndDispositionSummary;
using Com.Kana.Service.Upload.Lib.Facades.UnpaidDispositionReportFacades;
using Com.Kana.Service.Upload.Lib.Facades.BudgetCashflowService;
using Com.Kana.Service.Upload.Lib.Facades.BudgetCashflowService.ExcelGenerator;
using Com.Kana.Service.Upload.Lib.Facades.BudgetCashflowService.PdfGenerator;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchasingExpedition;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchasingBookReport;
using Com.Kana.Service.Upload.Lib.Services.GarmentDebtBalance;
using Com.Kana.Service.Upload.Lib.Facades.GarmentDispositionPurchaseFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentDispositionPaymentReport;
using Microsoft.ApplicationInsights.AspNetCore;
using Com.Kana.Service.Upload.Lib.Facades.GarmentClosingDateFacades;

namespace Com.Kana.Service.Upload.WebApi
{
    public class Startup
    {
        /* Hard Code */
        private string[] EXPOSED_HEADERS = new string[] { "Content-Disposition", "api-version", "content-length", "content-md5", "content-type", "date", "request-id", "response-time" };
        private string PURCHASING_POLICITY = "PurchasingPolicy";

        public IConfiguration Configuration { get; }

        public bool HasAppInsight => !string.IsNullOrEmpty(Configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY") ?? Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Register

        private void RegisterEndpoints()
        {
            APIEndpoint.Purchasing = Configuration.GetValue<string>(Constant.PURCHASING_ENDPOINT) ?? Configuration[Constant.PURCHASING_ENDPOINT];
            APIEndpoint.Core = Configuration.GetValue<string>(Constant.CORE_ENDPOINT) ?? Configuration[Constant.CORE_ENDPOINT];
            APIEndpoint.Inventory = Configuration.GetValue<string>(Constant.INVENTORY_ENDPOINT) ?? Configuration[Constant.INVENTORY_ENDPOINT];
            APIEndpoint.Finance = Configuration.GetValue<string>(Constant.FINANCE_ENDPOINT) ?? Configuration[Constant.FINANCE_ENDPOINT];
            APIEndpoint.CustomsReport = Configuration.GetValue<string>(Constant.CUSTOMSREPORT_ENDPOINT) ?? Configuration[Constant.CUSTOMSREPORT_ENDPOINT];
            APIEndpoint.Sales = Configuration.GetValue<string>(Constant.SALES_ENDPOINT) ?? Configuration[Constant.SALES_ENDPOINT];
            APIEndpoint.Auth = Configuration.GetValue<string>(Constant.AUTH_ENDPOINT) ?? Configuration[Constant.AUTH_ENDPOINT];
            APIEndpoint.GarmentProduction = Configuration.GetValue<string>(Constant.GARMENT_PRODUCTION_ENDPOINT) ?? Configuration[Constant.GARMENT_PRODUCTION_ENDPOINT];
            APIEndpoint.PackingInventory = Configuration.GetValue<string>(Constant.PACKINGINVENTORY_ENDPOINT) ?? Configuration[Constant.PACKINGINVENTORY_ENDPOINT];

            AuthCredential.Username = Configuration.GetValue<string>(Constant.USERNAME) ?? Configuration[Constant.USERNAME];
            AuthCredential.Password = Configuration.GetValue<string>(Constant.PASSWORD) ?? Configuration[Constant.PASSWORD];
        }

        private void RegisterFacades(IServiceCollection services)
        {
            services
                .AddTransient<ICoreData, CoreData>()
                .AddTransient<ICoreHttpClientService, CoreHttpClientService>()
                .AddTransient<IMemoryCacheManager, MemoryCacheManager>()
                .AddTransient<PurchasingDocumentExpeditionFacade>()
                .AddTransient<IBankExpenditureNoteFacade, BankExpenditureNoteFacade>()
                .AddTransient<IBankDocumentNumberGenerator, BankDocumentNumberGenerator>()
                .AddTransient<PurchasingDocumentExpeditionReportFacade>()
                .AddTransient<IPurchasingDocumentExpeditionFacade, PurchasingDocumentExpeditionFacade>()
                .AddTransient<IPPHBankExpenditureNoteFacade, PPHBankExpenditureNoteFacade>()
                .AddTransient<IPPHBankExpenditureNoteReportFacade, PPHBankExpenditureNoteReportFacade>()
                .AddTransient<IUnitPaymentOrderPaidStatusReportFacade, UnitPaymentOrderPaidStatusReportFacade>()
                .AddTransient<IUnitPaymentOrderUnpaidReportFacade, UnitPaymentOrderUnpaidReportFacade>()
                .AddTransient<UnitPaymentOrderNotVerifiedReportFacade>()
                .AddTransient<PurchaseRequestFacade>()
                .AddTransient<DeliveryOrderFacade>()
                .AddTransient<IDeliveryOrderFacade, DeliveryOrderFacade>()
                .AddTransient<InternalPurchaseOrderFacade>()
                .AddTransient<ExternalPurchaseOrderFacade>()
                .AddTransient<MonitoringPriceFacade>()
                .AddTransient<IUnitReceiptNoteFacade, UnitReceiptNoteFacade>()
                .AddTransient<TotalPurchaseFacade>()
                .AddTransient<PurchaseRequestGenerateDataFacade>()
                .AddTransient<ExternalPurchaseOrderGenerateDataFacade>()
                .AddTransient<UnitReceiptNoteGenerateDataFacade>()
                .AddTransient<IUnitPaymentOrderFacade, UnitPaymentOrderFacade>()
                .AddTransient<IUnitPaymentQuantityCorrectionNoteFacade, UnitPaymentQuantityCorrectionNoteFacade>()
                .AddTransient<IUnitPaymentPriceCorrectionNoteFacade, UnitPaymentPriceCorrectionNoteFacade>()
                .AddTransient<PurchaseOrderMonitoringAllFacade>()
                .AddTransient<IGarmentPurchaseRequestFacade, GarmentPurchaseRequestFacade>()
                .AddTransient<IGarmentPurchaseRequestItemFacade, GarmentPurchaseRequestItemFacade>()
                .AddTransient<IGarmentInternalPurchaseOrderFacade, GarmentInternalPurchaseOrderFacade>()
                .AddTransient<IGarmentTotalPurchaseOrderFacade, TotalGarmentPurchaseFacade>()
                .AddTransient<IGarmentInvoice, GarmentInvoiceFacade>()
                .AddTransient<IGarmentInternNoteFacade, GarmentInternNoteFacades>()
                .AddTransient<IGarmentExternalPurchaseOrderFacade, GarmentExternalPurchaseOrderFacade>()
                .AddTransient<IGarmentDeliveryOrderFacade, GarmentDeliveryOrderFacade>()
                .AddTransient<IGarmentDebtBalanceReportFacade, GarmentDebtBalanceReportFacade>()
                .AddTransient<IPurchasingDispositionFacade, PurchasingDispositionFacade>()
                .AddTransient<IGarmentCorrectionNotePriceFacade, GarmentCorrectionNotePriceFacade>()
                .AddTransient<IGarmentCorrectionNoteQuantityFacade, GarmentCorrectionNoteQuantityFacade>()
                .AddTransient<IGarmentBeacukaiFacade, GarmentBeacukaiFacade>()
                .AddTransient<IPurchasingDispositionFacade, PurchasingDispositionFacade>()
                .AddTransient<IGarmentCorrectionNotePriceFacade, GarmentCorrectionNotePriceFacade>()
                .AddTransient<IGarmentUnitReceiptNoteFacade, GarmentUnitReceiptNoteFacade>()
                .AddTransient<IGarmentDOItemFacade, GarmentDOItemFacade>()
                .AddTransient<IMonitoringUnitReceiptAllFacade, MonitoringUnitReceiptAllFacade>()
                .AddTransient<IGarmentUnitDeliveryOrderFacade, GarmentUnitDeliveryOrderFacade>()
                .AddTransient<IGarmentUnitExpenditureNoteFacade, GarmentUnitExpenditureNoteFacade>()
                .AddTransient<IGarmentReturnCorrectionNoteFacade, GarmentReturnCorrectionNoteFacade>()
                .AddTransient<IGarmentUnitDeliveryOrderReturFacade, GarmentUnitDeliveryOrderReturFacade>()
                .AddTransient<IMonitoringCentralBillReceptionFacade, MonitoringCentralBillReceptionFacade>()
                .AddTransient<IMonitoringCentralBillExpenditureFacade, MonitoringCentralBillExpenditureFacade>()
                .AddTransient<IMonitoringCorrectionNoteReceptionFacade, MonitoringCorrectionNoteReceptionFacade>()
                .AddTransient<IMonitoringCorrectionNoteExpenditureFacade, MonitoringCorrectionNoteExpenditureFacade>()
                .AddTransient<IGarmentDailyPurchasingReportFacade, GarmentDailyPurchasingReportFacade>()
                .AddTransient<IGarmentReceiptCorrectionFacade, GarmentReceiptCorrectionFacade>()
                .AddTransient<IGarmentPOMasterDistributionFacade, GarmentPOMasterDistributionFacade>()
                .AddTransient<IMonitoringROJobOrderFacade, MonitoringROJobOrderFacade>()
                .AddTransient<IMonitoringROMasterFacade, MonitoringROMasterFacade>()
                .AddTransient<IBudgetMasterSampleDisplayFacade, BudgetMasterSampleDisplayFacade>()
                .AddTransient<IUnitPaymentOrderExpeditionReportService, UnitPaymentOrderExpeditionReportService>()
                .AddTransient<ILocalPurchasingBookReportFacade, LocalPurchasingBookReportFacade>()
                .AddTransient<IImportPurchasingBookReportFacade, ImportPurchasingBookReportFacade>()
                .AddTransient<IDetailCreditBalanceReportFacade, DetailCreditBalanceReportFacade>()
                .AddTransient<ICurrencyProvider, CurrencyProvider>()
                .AddTransient<IPurchaseMonitoringService, PurchaseMonitoringService>()
                .AddTransient<IGarmentPurchasingBookReportFacade, GarmentPurchasingBookReportFacade>()
                .AddTransient<IPRMasterValidationReportFacade, PRMasterValidationReportFacade>()
                .AddTransient<IAccountingStockReportFacade, AccountingStockReportFacade>()
                .AddTransient<IGarmentReceiptCorrectionReportFacade, GarmentReceiptCorrectionReportFacade>()
                .AddTransient<IGarmentTopTenPurchaseSupplier, TopTenGarmentPurchaseFacade>()
                .AddTransient<IGarmentFlowDetailMaterialReport, GarmentFlowDetailMaterialReportFacade>()
                .AddTransient<IGarmentPurchaseDayBookReport, GarmentPurchaseDayBookReportFacade>()
                .AddTransient<IGarmentCorrectionNoteFacade, GarmentCorrectionNoteFacade>()
                .AddTransient<IGarmentPurchaseDayBookReport, GarmentPurchaseDayBookReportFacade>()
                .AddTransient<IGarmentStockReportFacade, GarmentStockReportFacade>()
                .AddTransient<IGarmenInternNotePaymentStatusFacade, GarmentInternNotePaymentStatusFacade>()
                .AddTransient<IGarmentReportCMTFacade, GarmentReportCMTFacade>()
                .AddTransient<IGarmentRealizationCMTReportFacade, GarmentRealizationCMTReportFacade>()
                .AddTransient<IDebtBookReportFacade, DebtBookReportFacade>()
                .AddTransient<IBalanceDebtFacade, GarmentSupplierBalanceDebtFacade>()
                .AddTransient<IDebtCardReportFacade, DebtCardReportFacade>()
                .AddTransient<IVBRequestPOExternalService, VBRequestPOExternalService>()
                .AddTransient<IDebtAndDispositionSummaryService, DebtAndDispositionSummaryService>()
                .AddTransient<IGarmentStockOpnameFacade, GarmentStockOpnameFacade>()
                .AddTransient<IBudgetCashflowService, BudgetCashflowService>()
                .AddTransient<IBudgetCashflowUnitPdf, BudgetCashflowUnitPdf>()
                .AddTransient<IBudgetCashflowDivisionPdf, BudgetCashflowDivisionPdf>()
                .AddTransient<IBudgetCashflowUnitExcelGenerator, BudgetCashflowUnitExcelGenerator>()
                .AddTransient<IBudgetCashflowDivisionExcelGenerator, BudgetCashflowDivisionExcelGenerator>()
                .AddTransient<IGarmentPurchasingExpeditionService, GarmentPurchasingExpeditionService>()
                .AddTransient<IUnpaidDispositionReportDetailFacade, UnpaidDispositionReportDetailFacade>()
                .AddTransient<IGarmentPurchasingBookReportService, GarmentPurchasingBookReportService>()
                .AddTransient<IGarmentDebtBalanceService, GarmentDebtBalanceService>()
                .AddTransient<IGarmentDispositionPurchaseFacade, GarmentDispositionPurchaseFacade>()
                .AddTransient<IGarmentDispositionPaymentReportService, GarmentDispositionPaymentReportService>()
                .AddTransient<IROFeatureFacade, ROFeatureFacade>()
                .AddTransient<ITraceableBeacukaiFacade, TraceableBeacukaiFacade>()
                .AddTransient<IGarmentBC23ReportFacade, GarmentBC23ReportFacade>()
                .AddTransient<IMutationBeacukaiFacade, MutationBeacukaiFacade>()
                .AddTransient<IGarmentClosingDateFacade, GarmentClosingDateFacade>()
                .AddTransient<IMonitoringFlowProductFacade, MonitoringFlowProductFacade>()
                .AddTransient<IBeacukaiNoFeature, BeacukaiNoFeature>()
                .AddTransient<IRealizationBOMFacade, RealizationBOMFacade>();

        }

        private void RegisterServices(IServiceCollection services, bool isTest)
        {
            services
                .AddScoped<IdentityService>()
                .AddScoped<ValidateService>()
                .AddScoped<IHttpClientService, HttpClientService>()
                .AddScoped<IValidateService, ValidateService>();

            //if (isTest == false)
            //{
            //    services.AddScoped<IHttpClientService, HttpClientService>();
            //}
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
            ClassMap<UnitPaymentCorrectionNoteViewModel>.Register();
        }

        #endregion Register

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString(Constant.DEFAULT_CONNECTION) ?? Configuration[Constant.DEFAULT_CONNECTION];
            string env = Configuration.GetValue<string>(Constant.ASPNETCORE_ENVIRONMENT);
            string connectionStringLocalCashFlow = Configuration.GetConnectionString("LocalDbCashFlowConnection") ?? Configuration["LocalDbCashFlowConnection"];
            APIEndpoint.ConnectionString = Configuration.GetConnectionString("DefaultConnection") ?? Configuration["DefaultConnection"];

            /* Register */
            //services.AddDbContext<PurchasingDbContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<PurchasingDbContext>(options => options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(1000 * 60 * 20)));
            services.AddTransient<ILocalDbCashFlowDbContext>(s => new LocalDbCashFlowDbContext(connectionStringLocalCashFlow));
            RegisterEndpoints();
            RegisterFacades(services);
            RegisterServices(services, env.Equals("Test"));
            services.AddAutoMapper(typeof(BaseAutoMapperProfile));
            services.AddMemoryCache();

            RegisterSerializationProvider();
            RegisterClassMap();
            MongoDbContext.connectionString = Configuration.GetConnectionString(Constant.MONGODB_CONNECTION) ?? Configuration[Constant.MONGODB_CONNECTION];

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("RedisConnection") ?? Configuration["RedisConnection"];
                options.InstanceName = Configuration.GetValue<string>("RedisConnectionName") ?? Configuration["RedisConnectionName"];
            });

            /* Versioning */
            services.AddApiVersioning(options => { options.DefaultApiVersion = new ApiVersion(1, 0); });

            /* Authentication */
            string Secret = Configuration.GetValue<string>(Constant.SECRET) ?? Configuration[Constant.SECRET];
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = Key
                    };
                });

            /* CORS */
            services.AddCors(options => options.AddPolicy(PURCHASING_POLICITY, builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders(EXPOSED_HEADERS);
            }));

            /* API */
            services
               .AddMvcCore()
               .AddApiExplorer()
               .AddAuthorization()
               .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
               .AddJsonFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Purchasing API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Enumerable.Empty<string>() },
                });
                c.OperationFilter<ResponseHeaderFilter>();

                c.CustomSchemaIds(i => i.FullName);
            });

            // App Insight
            if (HasAppInsight)
            {
                services.AddApplicationInsightsTelemetry();
                services.AddAppInsightRequestBodyLogging();
            }

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /* Update Database */
            //using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    PurchasingDbContext context = serviceScope.ServiceProvider.GetService<PurchasingDbContext>();

            //    if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            //    {
            //        context.Database.SetCommandTimeout(10 * 60 * 1000);
            //        context.Database.Migrate();
            //    }
            //}

            if(HasAppInsight){
                app.UseAppInsightRequestBodyLogging();
                app.UseAppInsightResponseBodyLogging();
            }

            app.UseAuthentication();
            app.UseCors(PURCHASING_POLICITY);
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            JobManager.Initialize(new MasterRegistry(app.ApplicationServices));
        }
    }
}
