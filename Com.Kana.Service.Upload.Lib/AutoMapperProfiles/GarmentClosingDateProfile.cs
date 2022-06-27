using AutoMapper;
using Com.Kana.Service.Upload.Lib.Models.GarmentClosingDateModels;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentClosingDateViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.AutoMapperProfiles
{
    public class GarmentClosingDateProfile : Profile
    {
        public GarmentClosingDateProfile()
        {
            CreateMap<GarmentClosingDate, GarmentClosingDateViewModel>()
              .ReverseMap();
        }
    }
}
