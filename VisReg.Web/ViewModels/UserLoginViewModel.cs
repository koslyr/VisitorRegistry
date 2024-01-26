using System.ComponentModel.DataAnnotations;
using VisReg.Cmn.Namings;

namespace VisReg.Web.ViewModels
{
    public class UserLoginViewModel
    {
        public string Message { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserNameRequired")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(NamingViewModels), Name = "UserPassword")]
        [Required(ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserPasswordRequired")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Ο 'Κωδικός Πρόσβασης' πρέπει να αποτελείται το ελάχιστο από 6 χαρακτήρες.")]
        public string UserPassword { get; set; }
    }
}