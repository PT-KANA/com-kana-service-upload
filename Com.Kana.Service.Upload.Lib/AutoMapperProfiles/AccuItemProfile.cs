using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;

namespace Com.Kana.Service.Upload.Lib.AutoMapperProfiles
{
    public class AccuItemProfile : Profile
    {
        public AccuItemProfile()
        {
            CreateMap<AccuItem, AccuItemViewModel>()
                .ForMember(d => d._id, opt => opt.MapFrom(s => s.Id))
                .ReverseMap();
        }
    }
}
