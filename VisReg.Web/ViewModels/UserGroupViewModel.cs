using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VisReg.Cmn.Namings;

namespace VisReg.Web.ViewModels
{
    public class UserGroupViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public short USG_Id { get; set; }

        public string USG_PublicCode { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserGroupName")]
        public string USG_Name { get; set; }

        public bool USG_IsAdmin { get; set; }

        public Nullable<short> USG_Order { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserGroupDescription")]
        public string USG_Description { get; set; }
    }
}