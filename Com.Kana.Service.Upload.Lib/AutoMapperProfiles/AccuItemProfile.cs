using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.AutoMapperProfiles
{
    public class AccuItemProfile : Profile
    {
        public AccuItemProfile()
        {
            CreateMap<AccuItem, AccuItemViewModel>()
                .ReverseMap();

            CreateMap<AccuItemDetailGroup, AccuItemDetailGroupViewModel>()
                .ReverseMap();

            CreateMap<AccuItemDetailOpenBalance, AccuItemDetailOpenBalanceViewModel>()
                .ReverseMap();

            CreateMap<AccuItemDetailSerialNumber, AccuItemDetailSerialNumberViewModel>()
                .ReverseMap();
        }
    }
}
