using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VisReg.Cmn.Namings;

namespace VisReg.Web.ViewModels
{
    public class UserViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public short USE_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserNameRequired")]
        public string USE_UserName { get; set; }

        //public string USE_Salt { get; set; }

        //[Display(ResourceType = typeof(NamingViewModels), Name = "UserPassword")]
        //public string USE_Password { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserFirstName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserFirstNameRequired")]
        public string USE_FirstName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserLastName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserLastNameRequired")]
        public string USE_LastName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserTelephone")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserTelephoneRequired")]
        public string USE_Telephone { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserEmail")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserEmailRequired")]
        public string USE_Email { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserDescription")]
        public string USE_Description { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserIPAddress")]
        public string USE_IPAddress { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserLoginLastDate")]
        public Nullable<System.DateTime> USE_LoginLastDate { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserLoginCounter")]
        public Nullable<short> USE_LoginCounter { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserIsActive")]
        public bool USE_IsActive { get; set; }

        public string USE_FullName { get; set; }

        public string LstUserGroup { get; set; }
    }
}