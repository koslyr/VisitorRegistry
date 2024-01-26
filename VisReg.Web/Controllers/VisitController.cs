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
using VisReg.Cmn.Namings;

namespace VisReg.Web.Controllers
{
    [Authorize]
    public class VisitController : Controller
    {
        private readonly ISrvVisit _SrvVisit;
        private readonly ISrvVisitor _SrvVisitor;
        private readonly ISrvUser _SrvUser;

        public VisitController(ISrvVisit SrvVisit, 
                               ISrvVisitor SrvVisitor,
                               ISrvUser SrvUser)
        {
            _SrvVisit = SrvVisit;
            _SrvVisitor = SrvVisitor;
            _SrvUser = SrvUser;
        }

        // ΕΜΦΑΝΙΣΗ ΕΠΙΣΚΕΨΗΣ.
        [HttpGet]
        public ActionResult ShowVisit(int VisitId)
        {
            VisitInfoViewModel myVisitInfoVM = new VisitInfoViewModel();
            int myVisitId = VisitId;

            VisitInfo myVisitInfo = _SrvVisit.GetVisitById(myVisitId);
            // Mapping...
            myVisitInfoVM = Mapper.Map<VisitInfo, VisitInfoViewModel>(myVisitInfo);

            return View(myVisitInfoVM);
        }

        // ΜΗΤΡΩΟ ΕΠΙΣΚΕΨΕΩΝ ΗΜΕΡΑΣ.
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult GetVisits()
        {
            //
            var url = Request.RawUrl;
            if (url == @"/")
            {
                TempData.Keep();
                Response.Redirect("/Visit/GetVisits");
            }

            // Έλεγχος για εμφάνιση μηνυματος.
            CheckForErrorMsg();

            return View();
        }

        private void CheckForErrorMsg()
        {
            // Ορισμός Μηνύματος για Notification.
            if (TempData["myErrMsg"] != null)
            {
                ViewBag.MsgForNotification = TempData["myErrMsg"];
                ViewBag.MsgIsError = true;
            }
            else
                ViewBag.MsgForNotification = "";
        }

        // ΜΗΤΡΩΟ ΕΠΙΣΚΕΨΕΩΝ ΗΜΕΡΑΣ(για το Grid-Data).
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetVisitsGridData([DataSourceRequest] DataSourceRequest gridRequest,
                                            DateTime DateVisit, 
                                            string LastName, string IdentificationNumber, string CardNumber,
                                            string VisitStatus)

        {
            IList<VisitInfoViewModel> myLstVisitInfoVM = new List<VisitInfoViewModel>();
            
            IList<VisitInfo> myLstVisitInfo = _SrvVisit.GetVisitsByDate(DateVisit, 
                                                                        LastName, IdentificationNumber, CardNumber,
                                                                        VisitStatus);
            // Mapping...
            myLstVisitInfoVM = Mapper.Map<IList<VisitInfo>,
                                          IList<VisitInfoViewModel>>(myLstVisitInfo);

            // Increase MaxJsonLength
            var myJsonResult = Json(myLstVisitInfoVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
            myJsonResult.MaxJsonLength = int.MaxValue;
            return myJsonResult;

            //return Json(myLstVisitInfoVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
        }

        // ΛΙΣΤΑ ΕΠΙΣΚΕΨΕΩΝ.
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetVisitsList()
        {

            return View();
        }

        // ΛΙΣΤΑ ΕΠΙΣΚΕΨΕΩΝ (για το Grid-Data).
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetVisitsListGridData([DataSourceRequest] DataSourceRequest gridRequest,
                                                DateTime? DateVisitFrom, DateTime? DateVisitTo, 
                                                string LastName, string IdentificationNumber, string CardNumber,
                                                string VisitStatus)

        {
            IList<VisitInfoViewModel> myLstVisitInfoVM = new List<VisitInfoViewModel>();

            IList<VisitInfo> myLstVisitInfo = _SrvVisit.GetVisitsExt(DateVisitFrom, DateVisitTo, 
                                                                     LastName, IdentificationNumber, CardNumber,
                                                                     VisitStatus);
            // Mapping...
            myLstVisitInfoVM = Mapper.Map<IList<VisitInfo>,
                                          IList<VisitInfoViewModel>>(myLstVisitInfo);

            return Json(myLstVisitInfoVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
        }

        // ΦΟΡΜΑ ΕΠΙΣΚΕΨΗΣ για δημιουργία (Modal).
        [HttpGet]
        public ActionResult CreateVisit()
        {
            VisitInfoViewModel myVisitInfoVM = new VisitInfoViewModel();

            return PartialView("_SaveVisit", myVisitInfoVM);
        }

        // ΦΟΡΜΑ ΕΠΙΣΚΕΨΗΣ για επεξερεγασία (Modal).
        [HttpGet]
        public ActionResult EditVisit(int VisitId)
        {
            VisitInfoViewModel myVisitInfoVM = new VisitInfoViewModel();

            // Ανάκτηση Επίσκεψης. 
            VisitInfo myVisitInfo = _SrvVisit.GetVisitById(VisitId);

            // Mapping...
            myVisitInfoVM = Mapper.Map<VisitInfo, VisitInfoViewModel>(myVisitInfo);

            return PartialView("_SaveVisit", myVisitInfoVM);
        }

        // ΦΟΡΜΑ ΕΠΙΣΚΕΨΗΣ (Modal για εκτέλεση Αποθήκευσης).
        [HttpPost]
        public ActionResult SaveVisit(VisitInfoViewModel myVisitInfoVM)
        {
            bool myIsSave = false;
            string myVisitorFullName = "";

            // Ανάκτηση Χρήστη.
            var myUser = _SrvUser.GetUserByUserName(User.Identity.Name);

            // Έλεγχος για τον Αρ. Ταυτοπροσωπίας.
            if (_SrvVisitor.GetIsExistIdentificationNumber(myVisitInfoVM.VSI_IdentificationNumber.Trim()))
            {
                var myVisitorByIdentNum = _SrvVisitor.GetVisitorByIdentificationNumber(myVisitInfoVM.VSI_IdentificationNumber.Trim());
                if (myVisitorByIdentNum.VST_LastName != myVisitInfoVM.VST_LastName)
                {
                    ModelState.AddModelError("ErrorForIdentificationNumber", "");
                    myVisitorFullName = myVisitorByIdentNum.VST_LastName + ' ' + myVisitorByIdentNum.VST_FirstName;
                    myVisitInfoVM.IsErrorForIdentificationNumber = true;
                }
            }

            // Αποθήκευση Επίσκεψης.
            if (ModelState.IsValid)
            {
                // Mapping...
                VisitInfo myVisitInfo = Mapper.Map<VisitInfoViewModel,
                                                   VisitInfo>(myVisitInfoVM);
                Visitor myVisitor = Mapper.Map<VisitInfoViewModel,
                                               Visitor>(myVisitInfoVM);
                
                // Εισαγωγή Νέας Επίσκεψης. 
                if (myVisitInfoVM.VSI_Id <= 0)
                    myIsSave = _SrvVisit.InsertVisit(myVisitInfo, myVisitor, myUser.USE_Id);
                // Επεξεργασία Υφιστάμενης Επίσκεψης.
                else
                    myIsSave = _SrvVisit.UpdateVisit(myVisitInfo, myUser.USE_Id);

                // Έλεγχος για το προηγούμενο action. 
                var action = (Request.UrlReferrer.Segments.Skip(2).Take(1).SingleOrDefault()).Trim('/');
                if (action == "ShowVisit")
                    //
                    return RedirectToAction("ShowVisit", new { VisitId= myVisitInfoVM.VSI_Id });
                else if (action == "GetVisits")
                    // Μετά την Αποθήκευση, εμφάνιση Λίστας Επισκέψεων. 
                    return RedirectToAction("GetVisits","Visit");
            }

            //var errors = ModelState.Select(x => x.Value.Errors)
            //              .Where(y => y.Count > 0)
            //              .ToList();

            string myErrMsg = "";
            TempData["myErrMsg"] = "";

            // Έλεγχος για αναφορά λάθους (από custom validation). 
            if (myVisitInfoVM.IsErrorForTimeExit)
               TempData["myErrMsg"] = NamingErrors.ErrorForTimeExit;
            else if  (myVisitInfoVM.IsErrorForIdentificationNumber)
                TempData["myErrMsg"] = "Ο συγκεκριμένος Αρ. Ταυτοπροσωπίας ανήκει σε άλλον Επισκέπτη με το Ονοματεπώνυμο:" + myVisitorFullName;

            return RedirectToAction("GetVisits", new {  MsgForNotification = myErrMsg });
        }

        // ΔΙΑΓΡΑΦΗ ΕΠΙΣΚΕΨΗΣ (Modal για επιβεβαίωση).
        [HttpGet]
        public ActionResult DeleteVisit(int VisitId)
        {
            VisitInfo myVisitInfo = _SrvVisit.GetVisitById(VisitId);
            // Mapping...
            VisitInfoViewModel myVisitInfoVM = Mapper.Map<VisitInfo, VisitInfoViewModel>(myVisitInfo);

            return PartialView("_DeleteVisit", myVisitInfoVM);
        }

        // ΔΙΑΓΡΑΦΗ ΕΠΙΣΚΕΨΗΣ (εκτέλεση Διαγραφής).
        [HttpPost]
        public ActionResult DeleteVisit(VisitInfoViewModel myVisitInfoVM)
        {
            // Ανάκτηση Χρήστη.
            var myUser = _SrvUser.GetUserByUserName(User.Identity.Name);
            bool IsDelete = _SrvVisit.DeleteVisit(myVisitInfoVM.VSI_Id, myUser.USE_Id);

            // Μετά τη Διαγραφή, εμφάνιση λίστας Επισκέψεων. 
            return RedirectToAction("GetVisits");
        }

        // ΕΝΗΜΕΡΩΣΗ ΕΠΙΣΚΕΨΗΣ (μονο για το χρόνο εξόδου).
        [HttpPost]
        public ActionResult EditVisitTimeExit(int VisitInfoId)
        {
            // Ανάκτηση Χρήστη.
            var myUser = _SrvUser.GetUserByUserName(User.Identity.Name);
            _SrvVisit.UpdateVisitTimeExit(VisitInfoId, myUser.USE_Id);

            // Μετά την ενημέρωση, εμφάνιση λίστας Επισκέψεων. 
            return RedirectToAction("GetVisitsGridData" , new { DateVisit=DateTime.Now });
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