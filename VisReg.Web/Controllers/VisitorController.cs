using System.Data;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using AutoMapper;
using VisReg.Web.ViewModels;
using VisReg.Srv.Interfaces;
using VisReg.Dal.Models;

namespace VisReg.Web.Controllers
{
    [Authorize]
    public class VisitorController : Controller
    {
        private readonly ISrvVisitor _SrvVisitor;
        private readonly ISrvVisit _SrvVisit;

        public VisitorController(ISrvVisitor SrvVisitor,
                                 ISrvVisit SrvVisit)
        {
            _SrvVisitor = SrvVisitor;
            _SrvVisit = SrvVisit;
        }

        // ΛΙΣΤΑ ΕΠΙΣΚΕΠΤΩΝ.
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult GetVisitors()
        {

            return View();
        }

        // ΛΙΣΤΑ ΕΠΙΣΚΕΠΤΩΝ (για το Grid-Data).
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetVisitorsGridData([DataSourceRequest] DataSourceRequest gridRequest,
                                              string VstLastName, string VstIdentificationNumber, string VstMobileNumber)

        {
            IList<VisitorViewModel> myLstVisitorVM = new List<VisitorViewModel>();

            IList<Visitor> myLstVisitor = _SrvVisitor.GetVisitors(VstLastName, VstIdentificationNumber, VstMobileNumber);
            // Mapping...
            myLstVisitorVM = Mapper.Map<IList<Visitor>,
                                        IList<VisitorViewModel>>(myLstVisitor);

            return Json(myLstVisitorVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
        }

        // ΕΜΦΑΝΙΣΗ ΕΠΙΣΚΕΠΤΗ.
        [HttpGet]
        public ActionResult ShowVisitor(int VisitorId)
        {
            VisitorViewModel myVstVM = new VisitorViewModel();
            int myVstId = VisitorId;

            Visitor myVst = _SrvVisitor.GetVisitorById(myVstId);
            // Mapping...
            myVstVM = Mapper.Map<Visitor, VisitorViewModel>(myVst);

            return View(myVstVM);
        }

        // ΛΙΣΤΑ ΕΠΙΣΚΕΠΤΩΝ (για το Grid-Data).
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetVisitsOfVisitorGridData([DataSourceRequest] DataSourceRequest gridRequest,
                                                     int VisitorId)

        {
            IList<VisitInfoViewModel> myLstVisitInfoVM = new List<VisitInfoViewModel>();

            IList<VisitInfo> myLstVisitInfo = _SrvVisit.GetVisitsByVisitor(VisitorId);
            // Mapping...
            myLstVisitInfoVM = Mapper.Map<IList<VisitInfo>,
                                          IList<VisitInfoViewModel>>(myLstVisitInfo);

            return Json(myLstVisitInfoVM.ToDataSourceResult(gridRequest), JsonRequestBehavior.AllowGet);
        }

        // ΔΗΜΙΟΥΡΓΙΑ ΕΠΙΣΚΕΠΤΗ.
        [HttpGet]
        public ActionResult CreateVisitor()
        {
            VisitorViewModel myVstVM = new VisitorViewModel();

            return View("SaveVisitor", myVstVM);
        }

        // ΕΠΕΞΕΡΓΑΣΙΑ ΕΠΙΣΚΕΠΤΗ.
        [HttpGet]
        public ActionResult EditVisitor(int VisitorId)
        {
            VisitorViewModel myVstVM = new VisitorViewModel();
            int myVstId = VisitorId;

            // Ανάκτηση Επισκέπτη.
            Visitor myVst = _SrvVisitor.GetVisitorById(myVstId);
            // Mapping...
            myVstVM = Mapper.Map<Visitor, VisitorViewModel>(myVst);

            return View("SaveVisitor", myVstVM);
        }

        // ΑΠΟΘΗΚΕΥΣΗ ΕΠΙΣΚΕΠΤΗ.
        [HttpPost]
        public ActionResult SaveVisitor(VisitorViewModel VisitorVM)
        {
            bool myIsSave = false;

            // Έλεγχος Στοιχείων Υπαλλήλου.
            if (ModelState.IsValid)
            {
                Visitor myVisitor = new Visitor();
                // Mapping...
                myVisitor = Mapper.Map<VisitorViewModel, Visitor>(VisitorVM);

                // Εισαγωγή Νέου Επισκέπτη. 
                if (VisitorVM.VST_Id <= 0)
                    myIsSave = _SrvVisitor.InsertVisitor(myVisitor);
                // Επεξεργασία Υφιστάμενου Επισκέπτη.
                else
                    myIsSave = _SrvVisitor.UpdateVisitor(myVisitor);

                if (myIsSave)
                    return RedirectToAction("ShowVisitor", new { VisitorId = myVisitor.VST_Id });
            }

            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

            return View(VisitorVM);
        }
    }
}