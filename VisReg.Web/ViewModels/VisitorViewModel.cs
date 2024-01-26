using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VisReg.Cmn.Namings;

namespace VisReg.Web.ViewModels
{
    public class VisitorViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int VST_Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        //[Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "SexNameRequired")]
        public Nullable<short> VST_SEX_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "SexName")]
        public string SEX_Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<short> VST_VSC_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VscName")]
        public string VSC_Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<short> VST_IDC_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "IdcName")]
        public string IDC_Name { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstFullName")]
        public string VST_FullName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstLastName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VstLastNameRequired")]
        [RegularExpression("^[A-ZΑ-Ω\\s]+$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyUpperAlphabets")]
        public string VST_LastName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstFirstName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VstFirstNameRequired")]
        [RegularExpression("^[A-ZΑ-Ω\\s]+$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyUpperAlphabets")]
        public string VST_FirstName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstFatherName")]
        [RegularExpression("^[A-ZΑ-Ω\\s]+$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyUpperAlphabets")]
        public string VST_FatherName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstMotherName")]
        [RegularExpression("^[A-ZΑ-Ω\\s]+$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyUpperAlphabets")]
        public string VST_MotherName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstIdentificationNumber")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VstIdentificationNumberRequired")]
        public string VST_IdentificationNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstMobileNumber")]
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyDigits")]
        public string VST_MobileNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "ResidenceAddress")]
        public string VST_Address { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "ResidencePostalCode")]
        public string VST_PostalCode { get; set; }


        [Display(ResourceType = typeof(NamingViewModels), Name = "Email")]
        [EmailAddress(ErrorMessage = "Η 'Ηλεκτρονική Διεύθυνση' δεν είναι έγκυρη.")]
        public string VST_Email { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstDescription")]
        public string VST_Description { get; set; }
    }
}