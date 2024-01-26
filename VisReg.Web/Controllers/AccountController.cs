using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using VisReg.Web.ViewModels;
using VisReg.Srv.Interfaces;
using VisReg.Dal.Models;

namespace VisReg.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ISrvUser _SrvUser;
        private readonly ISrvAuthentication _SrvAuthentication;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(ISrvUser SrvUser,
                                 ISrvAuthentication SrvAuthentication,
                                 IAuthenticationManager authenticationManager)
        {
            _SrvUser = SrvUser;
            _SrvAuthentication = SrvAuthentication;
            _authenticationManager = authenticationManager;
        }

        [AllowAnonymous]
        public ActionResult Login(string Message = "")
        {
            UserLoginViewModel myUserLoginVM = new UserLoginViewModel();
            myUserLoginVM.Message = Message;

            return View(myUserLoginVM);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserLoginViewModel myUserLoginVM)
        {
            string myStrMsgError = "ΔΕΝ ΒΡΕΘΗΚΕ ΧΡΗΣΤΗΣ ΜΕ ΤΑ ΣΤΟΙΧΕΙΑ ΠΟΥ ΔΩΣΑΤΕ!!!";
            myUserLoginVM.Message = "";

            if (ModelState.IsValid)
            {
                //1. Τα Credentials που δίνει ο Χρήστης προς εισαγωγή.
                string myProvidedUserName = myUserLoginVM.UserName.ToString();
                string myProvidedUserPassword = myUserLoginVM.UserPassword.ToString().Trim();

                //2.1 Ελεγχος για το UserName.
                bool myIsExistUserName = _SrvUser.IsExistUserName(myProvidedUserName.Trim());
                if (myIsExistUserName == false)
                {
                    ModelState.AddModelError("", myStrMsgError);
                    return View(myUserLoginVM);
                }

                //2.2 Ελεγχος εάν είναι Ενεργός ο συγκεκριμένος Χρήστης.
                bool myIsActiveUserName = _SrvUser.IsActiveUserName(myProvidedUserName.Trim());
                if (myIsActiveUserName == false)
                {
                    ModelState.AddModelError("", myStrMsgError);
                    return View(myUserLoginVM);
                }

                //3. Ανάκτηση των στοιχείων του Χρήστη από τη Βάση(εφόσον υπάρχει το UserName που δόθηκε).
                User myUser = new User();
                myUser = _SrvUser.GetUserByUserName(myProvidedUserName.Trim());

                //4. Έλεγχος για το Password.
                bool myIsVerifyUserPassword = _SrvAuthentication.VerifyHashedPassword(myUser.USE_Password + ":" + myUser.USE_Salt, myProvidedUserPassword.Trim());
                if (myIsVerifyUserPassword == true)
                {
                    // Ενημέρωση στοιχείων Χρήστη, για την πρόσβαση στο Σύστημα (Login Info).
                    _SrvUser.UpdateUserLoginInfo(myUser.USE_Id);

                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, myUserLoginVM.UserName) },
                                                                  DefaultAuthenticationTypes.ApplicationCookie);
                    // This sets the authentication cookie on the client.
                    _authenticationManager.SignIn(identity);
                    
                    //Redirect to default page based on Role.
                    return RedirectToAction("RedirectToDefault", myUserLoginVM);
                }
                // Τα Credentials του χρήστη δεν είναι Valid. 
                else
                    ModelState.AddModelError("", myStrMsgError);
            }

            // Εμφάνιση ίδιας ιστοσελίδας καθώς βρέθηκε κάποιο λάθος.
            return View(myUserLoginVM);
        }

        [Authorize]
        public ActionResult RedirectToDefault(UserLoginViewModel model)
        {
            // Ανάκτηση  Χρήστη, που έχει κάνει Login, και καταχώριση αυτoύ σε Session.
            User myUser = _SrvUser.GetUserByUserName(model.UserName.Trim());

            // Καταχώριση σε Session, τα στοιχεία του Χρήστη που κάνει Είσοδο.
            CheckSessionUserInfo();

            return RedirectToAction("GetVisits", "Visit");
        }

        [Authorize]
        public ActionResult LogOff()
        {
            //1. Clear Session.
            //1. Clear Session.
            Session["UserInfo"] = null;
            Session.Clear();
            Session.Abandon();

            //2. We obtain the IAuthenticationManager instance from the OWIN context, this time calling SignOut passing the authentication type (so the manager knows exactly what cookie to remove).
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            //3. Redirect to Login.
            return RedirectToAction("Login", "Account", new { area = "" });
        }

        // Έλεγχος για το Session του Χρήστη. 
        private void CheckSessionUserInfo()
        {
            if (Session["UserInfo"] == null)
            {
                SessionUserInfo mySessionUserInfo = new SessionUserInfo();

                var myUser = _SrvUser.GetUserByUserName(User.Identity.Name);
                // Ανάκτηση τιμών.
                mySessionUserInfo.UserId = myUser.USE_Id;
                mySessionUserInfo.IsAdmin = _SrvUser.IsUserAdmin(myUser.USE_Id);

                //Καταχώριση σε Session.
                Session["UserInfo"] = mySessionUserInfo;
            }
        }
    }
}