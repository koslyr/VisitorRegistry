using AutoMapper;
using VisReg.Dal.Models;
using VisReg.Web.ViewModels;

namespace VisReg.Web.Mappings
{
    public class MappingViewModelToEntity : Profile
    {
        public override string ProfileName
        {
            get { return "MappingViewModelToEntity"; }
        }

        public MappingViewModelToEntity()
        {
            CreateMap<VisitorViewModel, Visitor>();

            CreateMap<VisitInfoViewModel, VisitInfo>();

            CreateMap<VisitInfoViewModel, Visitor>()
                    .ForMember(d => d.VST_Id,
                               map => map.MapFrom(s => s.VSI_VST_Id))
                    .ForMember(d => d.VST_LastName,
                                map => map.MapFrom(s => s.VST_LastName))
                    .ForMember(d => d.VST_FirstName,
                                map => map.MapFrom(s => s.VST_FirstName))
                    .ForMember(d => d.VST_IdentificationNumber,
                                map => map.MapFrom(s => s.VSI_IdentificationNumber))
                    .ForMember(d => d.VST_VSC_Id,
                                map => map.MapFrom(s => s.VSI_VSC_Id))
                    .ForMember(d => d.VST_MotherName,
                                opt => opt.Ignore())
                    .ForMember(d => d.VST_FatherName,
                                opt => opt.Ignore());

            CreateMap<UserViewModel, User>();
        }
    }
}