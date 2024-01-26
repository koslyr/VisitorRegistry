using System.ComponentModel.DataAnnotations;
using VisReg.Cmn.Namings;

namespace VisReg.Web.ViewModels
{
    public class UserPasswordViewModel
    {
        public short UserId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Κωδικός Πρόσβασης:")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Ο 'Κωδικός Πρόσβασης' πρέπει να αποτελείται το ελάχιστο από 6 χαρακτήρες.")]
        [Required(ErrorMessage = "Ο 'Κωδικός Πρόσβασης' είναι υποχρεωτικός.")]
        public string UserPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Επιβεβαίωση Κωδικού:")]
        [System.ComponentModel.DataAnnotations.Compare("UserPassword", ErrorMessageResourceType = typeof(NamingErrors), ErrorMessageResourceName = "UserPasswordConfirmMismatch")]
        [Required(ErrorMessage = "Η 'Επιβεβαίωση Κωδικού' είναι υποχρεωτική.")]
        public string UserPasswordConfirm { get; set; }
    }
}