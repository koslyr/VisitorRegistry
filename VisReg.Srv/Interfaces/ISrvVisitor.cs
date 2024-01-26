using System.Collections.Generic;
using VisReg.Dal.Models;

namespace VisReg.Srv.Interfaces
{
    public interface ISrvVisitor
    {
        Visitor GetVisitorById(int VisitorId);

        Visitor GetVisitorOnlyById(int VisitorId);

        Visitor GetVisitorByLastNameIdentNumber(string VstLastName, string VstIdentificationNumber);

        Visitor GetVisitorByIdentificationNumber(string VstIdentificationNumber);

        IList<Visitor> GetVisitorsByLastName(string VstLastName);

        IList<Visitor> GetVisitorsByIdentificationNumber(string VstIdentificationNumber);

        IList<Visitor> GetVisitors(string VstLastName = "", string VstIdentificationNumber = "", string VstMobileNumber = "");

        IList<Visitor> GetVisitorsAll();

        bool InsertVisitor(Visitor myVisitor);

        bool UpdateVisitor(Visitor myVisitor);

        bool GetIsExistIdentificationNumber(string VstIdentificationNumber);
    }
}
