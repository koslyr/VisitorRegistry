using System.Collections.Generic;
using VisReg.Dal.Models;

namespace VisReg.Srv.Interfaces
{
    public interface ISrvCommon
    {
        IList<IdentificationCertificate> GetIdentificationCertificateAll();

        IList<VisitReason> GetVisitReasonAll();

        IList<VisitorCategory> GetVisitorCategoryAll();

        IList<Sex> GetSexAll();

        IList<EmployeeInfo> GetEmployeeInfoAll();

        IList<EmployeeInfo> GetEmployeeInfoByLastName(string EmpLastName);

        IList<OrganizationInfo> GetOrganizationInfoAll();

        IList<OrganizationInfo> GetOrganizationInfoByName(string OriName);
    }
}
