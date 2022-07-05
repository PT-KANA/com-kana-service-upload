using AutoMapper;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.AutoMapperProfiles
{
	public class AccuSalesInvoiceProfile : Profile
	{
		public AccuSalesInvoiceProfile()
		{
			CreateMap<AccuSalesInvoice, AccuSalesViewModel>()
				.ReverseMap();

			CreateMap<AccuSalesInvoiceDetailItem, AccuSalesInvoiceDetailItemViewModel>()
				.ReverseMap();

			CreateMap<AccuSalesInvoiceDetailExpense, AccuSalesInvoiceDetailExpenseViewModel>()
				.ReverseMap();

			CreateMap<AccuSalesInvoiceDetailDownPayment, AccuSalesInvoiceDetailDownPaymentViewModel>()
				.ReverseMap();

			CreateMap<AccuSalesInvoiceDetailSerialNumber, AccuSalesInvoiceDetailSerialNumberViewModel>()
				.ReverseMap();
		}
	}
}
