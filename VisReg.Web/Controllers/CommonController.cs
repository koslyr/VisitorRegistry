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
    public class CommonController : Controller
    {
        private readonly ISrvVisitor _SrvVisitor;
        private readonly ISrvVisit _SrvVisit;
        private readonly ISrvCommon _SrvCommon;
        
        public CommonController(ISrvVisitor SrvVisitor,
                                ISrvVisit SrvVisit,   
                                ISrvCommon SrvCommon)
        {
            _SrvVisitor = SrvVisitor;
            _SrvVisit = SrvVisit;
            _SrvCommon = SrvCommon;
        }

        // Ανάκτηση ΕΠΙΣΚΕΠΤΗ με βάση το ID.
        public JsonResult GetVisitorById(int VisitorId)
        {
            var getVisitor = _SrvVisitor.GetVisitorById(VisitorId);

            return Json(getVisitor, JsonRequestBehavior.AllowGet);
        }

        // Λίστα ΕΠΙΣΚΕΠΤΩΝ με βάση το ΕΠΩΝΥΜΟ.
        public JsonResult GetVisitorsByLastName(string VstLastName)
        {
           var getVisitors = _SrvVisitor.GetVisitorsByLastName(VstLastName)
                                        .Select(vst => new
                                         {
                                             VstId = vst.VST_Id,
                                             VstLastName = vst.VST_LastName,
                                             VstFirstName = vst.VST_FirstName,
                                             VstIdentificationNumber = vst.VST_IdentificationNumber,
                                             VstSexId = vst.VST_SEX_Id
                                         });

            return Json(getVisitors.ToList(), JsonRequestBehavior.AllowGet);
        }

        // 
        public JsonResult GetVisitorsByIdentificationNumber(string VstIdentificationNumber)
        {
            var getVisitors = _SrvVisitor.GetVisitorsByIdentificationNumber(VstIdentificationNumber)
                                         .Select(vst => new
                                          {
                                              VstId = vst.VST_Id,
                                              VstLastName = vst.VST_LastName,
                                              VstFirstName = vst.VST_FirstName,
                                              VstIdentificationNumber = vst.VST_IdentificationNumber
                                          });

            return Json(getVisitors.ToList(), JsonRequestBehavior.AllowGet);
        }

        // 
        public JsonResult GetIdentificationCertificateAll()
        {
            var getAll = _SrvCommon.GetIdentificationCertificateAll()
                                   .Select(idc => new
                                    {
                                        IdentificationCertificateId = idc.IDC_Id,
                                        IdentificationCertificateName = idc.IDC_Name
                                    });

            return Json(getAll, JsonRequestBehavior.AllowGet);
        }

        // Ανάκτηση Λόγων Επίσκεψης.
        public JsonResult GetVisitReasonAll()
        {
            var getAll = _SrvCommon.GetVisitReasonAll()
                                    .Select(vsr => new
                                    {
                                        VisitReasonId = vsr.VSR_Id,
                                        VisitReasonName = vsr.VSR_Name
                                    });

            return Json(getAll, JsonRequestBehavior.AllowGet);
        }

        // Ανάκτηση Κατηγοριών Επισκεπτών.
        public JsonResult GetVisitorCategoryAll()
        {
            var getAll = _SrvCommon.GetVisitorCategoryAll()
                                    .OrderBy(x=>x.VSC_Name)
                                    .Select(vsc => new
                                    {
                                        VisitorCategoryId = vsc.VSC_Id,
                                        VisitorCategoryName = vsc.VSC_Name
                                    });
                                    
            return Json(getAll, JsonRequestBehavior.AllowGet);
        }

        // 
        public JsonResult GetSexAll()
        {
            var getAll = _SrvCommon.GetSexAll()
                                      .Select(sex => new
                                      {
                                         SexId = sex.SEX_Id,
                                         SexName = sex.SEX_Name
                                      });

            return Json(getAll, JsonRequestBehavior.AllowGet);
        }

        // Ανάκτηση ΥΠΑΛΛΗΛΩΝ.
        public JsonResult GetEmployeeInfoAll()
        {
            var getAll = _SrvCommon.GetEmployeeInfoAll()
                                   .Select(emi => new
                                   {
                                       EmployeeInfoId = emi.EMI_Id,
                                       EmployeeInfoFullNameOfficeTelephone = emi.EMI_LastName + " " + emi.EMI_FirstName + " (" + emi.EMI_Office + " - " + emi.EMI_Telephone + ")"
                                   });

            return Json(getAll, JsonRequestBehavior.AllowGet);
        }

        // Ανάκτηση ΥΠΑΛΛΗΛΩΝ με βάση το ΕΠΩΝΥΜΟ.
        public JsonResult GetEmployeeInfoByLastName(string EmpLastName)
        {
            var getEmployeeInfo = _SrvCommon.GetEmployeeInfoByLastName(EmpLastName)
                                            .Select(emi => new
                                            {
                                                 EmpId = emi.EMI_Id,
                                                 EmpLastName = emi.EMI_LastName,
                                                 EmpFirstName = emi.EMI_FirstName,
                                                 EmpOffice = emi.EMI_Office,
                                                 EmpTelephone = emi.EMI_Telephone
                                            });

            return Json(getEmployeeInfo.ToList(), JsonRequestBehavior.AllowGet);
        }

        // Ανάκτηση ΟΡΓΑΝΙΚΩΝ ΜΟΝΑΔΩΝ.
        public JsonResult GetOrganizationInfoAll()
        {
            var getAll = _SrvCommon.GetOrganizationInfoAll()
                                   .Select(ori => new
                                   {
                                       OrganizationInfoId = ori.ORI_Id,
                                       OrganizationInfoName = ori.ORI_Name
                                   });

            return Json(getAll, JsonRequestBehavior.AllowGet);
        }

        // Ανάκτηση ΟΡΓΑΝΙΚΩΝ ΜΟΝΑΔΩΝ με βάση την Ονομασία.
        public JsonResult GetOrganizationInfoByName(string OriName)
        {
            var getOrganizationInfo = _SrvCommon.GetOrganizationInfoByName(OriName)
                                                .Select(ori => new
                                                {
                                                    OriId = ori.ORI_Id,
                                                    OriName = ori.ORI_Name
                                                });

            return Json(getOrganizationInfo.ToList(), JsonRequestBehavior.AllowGet);
        }

        // Εμφάνισης Σελίδας για μη εξουσιοδοτημένη πρόσβαση.
        public ActionResult _NotAuthorized()
        {

            return View();
        }
    }
}