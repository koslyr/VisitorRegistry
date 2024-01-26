using AutoMapper;
using VisReg.Dal.Models;
using VisReg.Web.ViewModels;

namespace VisReg.Web.Mappings
{
    public class MappingEntityToViewModel : Profile
    {
        public override string ProfileName
        {
            get { return "MappingEntityToViewModel"; }
        }

        public MappingEntityToViewModel()
        {
            CreateMap<Visitor, VisitorViewModel>()
                      .ForMember(d => d.IDC_Name,
                                      map => map.MapFrom(s => s.IdentificationCertificate.IDC_Name))
                      .ForMember(d => d.VSC_Name,
                                      map => map.MapFrom(s => s.VisitorCategory.VSC_Name))
                      .ForMember(d => d.SEX_Name,
                                      map => map.MapFrom(s => s.Sex.SEX_Name))
                      .ForMember(d => d.VST_FullName,
                                      map => map.MapFrom(s => GetFullName(s.VST_LastName, s.VST_FirstName)));

            CreateMap<VisitInfo, VisitInfoViewModel>()
                     .ForMember(d => d.VSI_VSC_Id,
                                     map => map.MapFrom(s => s.VisitorCategory.VSC_Id))
                     .ForMember(d => d.VSC_Name,
                                     map => map.MapFrom(s => s.VisitorCategory.VSC_Name))
                     .ForMember(d => d.VSR_Name,
                                     map => map.MapFrom(s => s.VisitReason.VSR_Name))
                     .ForMember(d => d.VST_LastName,
                                     map => map.MapFrom(s => s.Visitor.VST_LastName))
                     .ForMember(d => d.VST_FirstName,
                                     map => map.MapFrom(s => s.Visitor.VST_FirstName))
                     .ForMember(d => d.VST_SEX_Id,
                                     map => map.MapFrom(s => s.Visitor.VST_SEX_Id))
                     .ForMember(d => d.SEX_Name,
                                     map => map.MapFrom(s => s.Visitor.Sex.SEX_Name))
                     .ForMember(d => d.ORI_Name,
                                     map => map.MapFrom(s => s.OrganizationInfo.ORI_Name))
                     .ForMember(d => d.EMI_FullNameInfo,
                                     map => map.MapFrom(s => GetFullNameOfficeTelephone(s.EmployeeInfo.EMI_LastName, s.EmployeeInfo.EMI_FirstName, s.EmployeeInfo.EMI_Office, s.EmployeeInfo.EMI_Telephone)))
                     .ForMember(d => d.VisitorFullName,
                                     map => map.MapFrom(s => GetFullName(s.Visitor.VST_LastName, s.Visitor.VST_FirstName)));

            CreateMap<User, UserViewModel>()
                      .ForMember(d => d.USE_FullName,
                                 map => map.MapFrom(s => GetFullName(s.USE_LastName, s.USE_FirstName)));

            CreateMap<UserGroup, UserGroupViewModel>();

            CreateMap<UserUserGroup, UserUserGroupViewModel>()
                     .ForMember(d => d.UserName,
                                map => map.MapFrom(s => s.User.USE_UserName))
                     .ForMember(d => d.UserGroupName,
                                map => map.MapFrom(s => s.UserGroup.USG_Name))
                     .ForMember(d => d.UserGroupDescription,
                                map => map.MapFrom(s => s.UserGroup.USG_Description));
        }

        // Ονοματεπώνυμο Υπαλλήλου.
        private string GetFullName(string LastName, string FirstName)
        {
            string myFullName = "";

            myFullName = LastName + " " + FirstName;

            return myFullName;
        }

        // Ονοματεπώνυμο Υπαλλήλου με Γραφείο & Τηλέφωνο.
        private string GetFullNameOfficeTelephone(string LastName, string FirstName, string Office, string TelePhone)
        {
            string myFullNameInfo = "";

            myFullNameInfo = LastName + " " + FirstName + "(" + Office + "-" + TelePhone + ")";

            return myFullNameInfo;
        }
    }
}