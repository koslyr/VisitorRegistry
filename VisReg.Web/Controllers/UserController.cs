using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using AutoMapper;
using VisReg.Web.ViewModels;
using VisReg.Srv.Interfaces;
using VisReg.Dal.Models;
using VisReg.Dal.Models.Extension;

namespace VisReg.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ISrvUser _SrvUser;

        public UserController(ISrvUser SrvUser)
        {
            _SrvUser = SrvUser;
        }

        // ΑΝΑΖΗΤΗΣΗ ΧΡΗΣΤΩΝ.
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult GetUsers()
        {
            CheckSessionUserInfo();
            SessionUserInfo mySessionUserInfo = (SessionUserInfo)Session["UserInfo"];
            
            // Έλεγχος εάν ο Χρήστης είναι Admin.
            if (!mySessionUserInfo.IsAdmin)
                return RedirectToAction("ShowUser", new { UserId = mySessionUserInfo.UserId });

            return View();
        }

        // ΑΝΑΖΗΤΗΣΗ ΧΡΗΣΤΩΝ (για το Grid-Data).
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetUsersGridData([DataSourceRequest] DataSourceRequest gridRequest)

        {
            IList<UserViewModel> myLstUserVM = new List<UserViewModel>();
            IList<User> myLstUser;

            CheckSessionUserInfo();
            SessionUserInfo mySessionUserInfo = (SessionUserInfo)Session["UserInfo"];

            // Έλεγχος εάν ο Χρήστης ανήκει στην Ομάδα: Admin.
            if (mySessionUserInfo.IsAdmin)
                myLstUser = _SrvUser.GetUserAll(-1);
            else
                myLstUser = _SrvUser.GetUserAll(mySessionUserInfo.UserId);

            // Mapping...
            myLstUserVM = Mapper.Map<IList<User>,
                                     IList<UserViewModel>>(myLstUser);

            return Json(myLstUserVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
        }

        // ΕΜΦΑΝΙΣΗ ΧΡΗΣΤΗ.
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult ShowUser(short UserId)
        {
            UserViewModel myUserVM = new UserViewModel();
            short myUserId = UserId;

            // Ανάκτηση Χρήστη.
            User myUser = _SrvUser.GetUserById(myUserId);
            // Mapping
            myUserVM = Mapper.Map<User, UserViewModel>(myUser);

            return View(myUserVM);
        }

        // ΑΝΑΖΗΤΗΣΗ ΟΜΑΔΩΝ ΠΟΥ ΕΙΝΑΙ ΚΑΤΑΧΩΡΗΜΕΝΟΣ Ο ΧΡΗΣΤΗΣ (για το Grid-Data).
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetUserUserGroupGridData([DataSourceRequest] DataSourceRequest gridRequest, short UserId)

        {
            //
            IList<UserUserGroup> myLstUserUserGroup = _SrvUser.GetUserUserGroupByUserId(UserId);

            // Mapping...
            IList<UserUserGroupViewModel> myLstUserUserGroupVM = Mapper.Map<IList<UserUserGroup>,
                                                                            IList<UserUserGroupViewModel>>(myLstUserUserGroup);

            return Json(myLstUserUserGroupVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
        }

        // ΑΝΑΖΗΤΗΣΗ ΟΛΩΝ ΤΩΝ ΟΜΑΔΩΝ (για το Grid-Data).
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetUserUserGroupInfoGridData([DataSourceRequest] DataSourceRequest gridRequest, short UserId)

        {
            //
            IList<ExtUserUserGroup> myLstExtUserUserGroup = _SrvUser.GetUserUserGroupInfo(UserId);

            // Mapping...
            IList<ExtUserUserGroupViewModel> myLstExtUserUserGroupVM = Mapper.Map<IList<ExtUserUserGroup>,
                                                                                  IList<ExtUserUserGroupViewModel>>(myLstExtUserUserGroup);

            return Json(myLstExtUserUserGroupVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
        }

        // ΔΗΜΙΟΥΡΓΙΑ ΧΡΗΣΤΗ.
        [HttpGet]
        public ActionResult CreateUser()
        {
            UserViewModel myUserVM = new UserViewModel();

            CheckSessionUserInfo();
            SessionUserInfo mySessionUserInfo = (SessionUserInfo)Session["UserInfo"];

            // Έλεχγος για την Ομάδα Χρήστη, καθώς μόνο όσοι ανήκουν στους Admin έχουν δικαίωμα να δημιουργούν Χρήστες.
            if (!mySessionUserInfo.IsAdmin)
                return RedirectToAction("_NotAuthorized", "Common");

            return View("SaveUser", myUserVM);
        }

        // ΕΠΕΞΕΡΓΑΣΙΑ ΧΡΗΣΤΗ.
        [HttpGet]
        public ActionResult EditUser(short UserId)
        {
            UserViewModel myUserVM = new UserViewModel();
            short myUserId = UserId;

            CheckSessionUserInfo();
            SessionUserInfo mySessionUserInfo = (SessionUserInfo)Session["UserInfo"];

            // Έλεχγος για την Ομάδα Χρήστη, καθώς μόνο όσοι ανήκουν στους Admin έχουν δικαίωμα να δημιουργούν Χρήστες.
            if (!mySessionUserInfo.IsAdmin)
                return RedirectToAction("_NotAuthorized", "Common");

            // Ανάκτηση Χρήστη.
            User myUser = _SrvUser.GetUserById(myUserId);
            // Mapping...
            myUserVM = Mapper.Map<User, UserViewModel>(myUser);

            return View("SaveUser", myUserVM);
        }

        // ΑΠΟΘΗΚΕΥΣΗ ΧΡΗΣΤΗ.
        [HttpPost]
        public ActionResult SaveUser(UserViewModel UserVM)
        {
            bool myIsSave = false;
            List<short> myLstUserGroup = new List<short>();

            // Έλεγχος Στοιχείων Χρήστη.
            if (ModelState.IsValid)
            {
                User myUser = new User();
                // Mapping...
                myUser = Mapper.Map<UserViewModel, User>(UserVM);
                // Ανάκτηση Ομάδων.
                myLstUserGroup = UserVM.LstUserGroup.Split(',').Select(Int16.Parse).ToList();

                // Εισαγωγή Νέου Χρήστη. 
                if (UserVM.USE_Id <= 0)
                    myIsSave = _SrvUser.InsertUser(myUser, myLstUserGroup);
                // Επεξεργασία Υφιστάμενου Χρήστη.
                else
                    myIsSave = _SrvUser.UpdateUser(myUser, myLstUserGroup);

                if (myIsSave)
                    return RedirectToAction("ShowUser", new { UserId = myUser.USE_Id });
            }

            return View(UserVM);
        }

        // ΟΡΙΣΜΟΣ ΣΥΝΘΗΜΑΤΙΚΟΥ (Modal για επιβεβαίωση).
        [HttpGet]
        public ActionResult SetUserPassword(short UserId)
        {
            UserPasswordViewModel myUserPwdVM = new UserPasswordViewModel();

            // Οροσμός Χρήστη για αλλαγή Password.
            myUserPwdVM.UserId = UserId;

            return PartialView("_SetUserPassword", myUserPwdVM);
        }

        // ΟΡΙΣΜΟΣ ΣΥΝΘΗΜΑΤΙΚΟΥ (εκτέλεση).
        [HttpPost]
        public ActionResult SetUserPassword(UserPasswordViewModel myUserPwdVM)
        {
            //Εκτέλεση Ορισμού Pwd.
            bool IsSetPwd = _SrvUser
                            .SetUserPassword(myUserPwdVM.UserId, myUserPwdVM.UserPassword);

            return RedirectToAction("ShowUser", new { UserId = myUserPwdVM.UserId });
        }

        // Έλεγχος για το Session του Χρήστη. 
        private void CheckSessionUserInfo()
        {
            if (Session["UserId"] == null)
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