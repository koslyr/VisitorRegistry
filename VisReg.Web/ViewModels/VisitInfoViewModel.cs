using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VisReg.Cmn.Namings;
using VisReg.Cmn.Enumeration;

namespace VisReg.Web.ViewModels
{
    public class VisitInfoViewModel : IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int VSI_Id { get; set; }

        public bool VSI_IsActive { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiDate")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VsiDateRequired")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime VSI_Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int VSI_VST_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstLastName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VstLastNameRequired")]
        [RegularExpression("^[A-ZΑ-Ω\\s]+$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyUpperAlphabets")]
        public string VST_LastName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstFirstName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VstFirstNameRequired")]
        [RegularExpression("^[A-ZΑ-Ω\\s]+$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyUpperAlphabets")]
        public string VST_FirstName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstFullName")]
        public string VisitorFullName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VisitorInfo")]
        public string VisitorInfo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<short> VSI_VSC_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VscName")]
        public string VSC_Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<short> VSI_VSR_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsrName")]
        public string VSR_Name { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "SexName")]
        public string SEX_Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<short> VST_SEX_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstMobileNumber")]
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyDigits")]
        public string VSI_MobileNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstIdentificationNumber")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VstIdentificationNumberRequired")]
        public string VSI_IdentificationNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiCardNumber")]
        [RegularExpression("^[0-9]{3,3}$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyDigits")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "VsiCardNumberRequired")]
        public string VSI_CardNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstMobileNumber")]
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyDigits")]
        public string VSΙ_MobileNumber { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> VSI_ORI_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "OriName")]
        public string ORI_Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> VSI_EMI_Id { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "EmiFullNameInfo")]
        public string EMI_FullNameInfo { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiOfficeNumber")]
        [RegularExpression("^[0-9]{4,4}$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyDigits")]
        public string VSI_OfficeNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiTimeEntrance")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public System.TimeSpan VSI_TimeEntrance { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiTimeExit")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public Nullable<System.TimeSpan> VSI_TimeExit { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiIsScheduledVisit")]
        public bool VSI_IsScheduledVisit { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiIsForGGA")]
        public bool VSI_IsForGGA { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiComment")]
        public string VSI_Comment { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstLastName")]
        [RegularExpression("^[A-ZΑ-Ω\\s]+$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyUpperAlphabets")]
        public string SearchLastName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiDate")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime SearchDateVisit { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VstIdentificationNumber")]
        public string SearchIdentificationNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "VsiCardNumber")]
        [RegularExpression("^[0-9]{3,3}$", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "OnlyDigits")]
        public string SearchCardNumber { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "DateVisitFrom")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime SearchDateVisitFrom { get; set; }
        
        [Display(ResourceType = typeof(NamingViewModels), Name = "DateVisitTo")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public System.DateTime SearchDateVisitTo { get; set; }

        // Για εμφάνιση των Custom Errors.
        public string ErrorForTimeExit { get; set; }
        public string ErrorForIdentificationNumber { get; set; }
        public string ErrorForCardNumber { get; set; }

        [DefaultValue(false)]
        public bool IsErrorForTimeExit { get; set;  }
        [DefaultValue(false)]
        public bool IsErrorForIdentificationNumber { get; set; }

        // Custom Validation.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Έλεγχος για χρόνο εξόδου σε σχέση με τo χρόνο εισόδου.
            if (VSI_TimeExit != null)
            {
                if (VSI_TimeExit < VSI_TimeEntrance)
                {
                    IsErrorForTimeExit = true;
                    yield return new ValidationResult("", new[] { "ErrorForTimeExit" });
                }
            }
        }

    }
}