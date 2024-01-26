using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VisReg.Cmn.Namings;

namespace VisReg.Web.ViewModels
{
    public class UserUserGroupViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public short UUG_Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserRequired")]
        public short UUG_USE_Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserGroupRequired")]
        public short UUG_USG_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserGroupName")]
        public string UserGroupName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserGroupDescription")]
        public string UserGroupDescription { get; set; }
    }
}