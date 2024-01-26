using System.Collections.Generic;
using System.Linq;
using VisReg.Dal.Models;
using VisReg.Srv.Interfaces;

namespace VisReg.Srv
{
    public class SrvCommon : ISrvCommon
    {
        private VisRegDB _VisRegDB = null;

        public SrvCommon()
        {
            _VisRegDB = new VisRegDB();
        }

        //
        public IList<IdentificationCertificate> GetIdentificationCertificateAll()
        {
            var getAll = from idc in _VisRegDB.IdentificationCertificate
                         orderby idc.IDC_Order
                         select idc;

            return getAll.ToList();
        }

        //
        public IList<VisitReason> GetVisitReasonAll()
        {
            var getAll = from vsc in _VisRegDB.VisitReason
                         orderby vsc.VSR_Name
                         select vsc;

            return getAll.ToList();
        }

        //
        public IList<VisitorCategory> GetVisitorCategoryAll()
        {
            var getAll = from vsc in _VisRegDB.VisitorCategory
                         orderby vsc.VSC_Name
                         select vsc;

            return getAll.ToList();
        }

        //
        public IList<Sex> GetSexAll()
        {
            var getAll = from sex in _VisRegDB.Sex
                         select sex;

            return getAll.ToList();
        }

        //
        public IList<EmployeeInfo> GetEmployeeInfoAll()
        {
            var getAll = from emi in _VisRegDB.EmployeeInfo
                         where emi.EMI_IsActive == true
                         orderby emi.EMI_LastName, emi.EMI_FirstName
                         select emi;

            return getAll.ToList();
        }

        // Ανάκτηση Λίστας Υπαλλήλων με βάση το Επώνυμο.
        public IList<EmployeeInfo> GetEmployeeInfoByLastName(string EmpLastName)
        {
            var getEmp = from emi in _VisRegDB.EmployeeInfo
                         where emi.EMI_LastName.StartsWith(EmpLastName.ToString().Trim().ToUpper()) && emi.EMI_IsActive == true
                         orderby emi.EMI_LastName, emi.EMI_FirstName
                         select emi;

            return getEmp.ToList();
        }

        //
        public IList<OrganizationInfo> GetOrganizationInfoAll()
        {
            var getAll = from ori in _VisRegDB.OrganizationInfo
                         where ori.ORI_IsActive == true
                         orderby ori.ORI_Name
                         select ori;

            return getAll.ToList();
        }

        // Ανάκτηση Λίστας Οργανικών Μονάδων με βάση την Ονομασία.
        public IList<OrganizationInfo> GetOrganizationInfoByName(string OriName)
        {
            var getOri = from ori in _VisRegDB.OrganizationInfo
                         where ori.ORI_Name.Contains(OriName.ToString().Trim().ToUpper()) && ori.ORI_IsActive == true
                         orderby ori.ORI_Name
                         select ori;

            return getOri.ToList();
        }

        protected virtual void Dispose(bool disposing)
        {
            _VisRegDB.Dispose();
        }
    }
}
