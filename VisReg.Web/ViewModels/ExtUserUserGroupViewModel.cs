using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VisReg.Cmn.Namings;

namespace VisReg.Web.ViewModels
{
    public class ExtUserUserGroupViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public short USE_Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public short USG_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserGroupName")]
        public string USG_Name { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserGroupDescription")]
        public string USG_Description { get; set; }

        [Display(Name = "Ανήκει")]
        public bool UserIsBelongUserGroup { get; set; }
    }
}