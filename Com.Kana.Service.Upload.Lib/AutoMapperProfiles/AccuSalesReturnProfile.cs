using AutoMapper;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.AutoMapperProfiles
{
    public class AccuSalesReturnProfile : Profile
    {
		public AccuSalesReturnProfile()
		{
			CreateMap<AccuSalesReturn, AccuSalesReturnViewModel>()
				.ReverseMap();

			CreateMap<AccuSalesReturnDetailItem, AccuSalesReturnDetailItemViewModel>()
				.ReverseMap();

			CreateMap<AccuSalesReturnDetailExpense, AccuSalesReturnDetailExpenseViewModel>()
				.ReverseMap();

			CreateMap<AccuSalesReturnDetailSerialNumber, AccuSalesReturnDetailSerialNumberViewModel>()
				.ReverseMap();
		}
	}
}
