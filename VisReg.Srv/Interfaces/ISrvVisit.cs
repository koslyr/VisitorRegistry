using System;
using System.Collections.Generic;
using VisReg.Dal.Models;

namespace VisReg.Srv.Interfaces
{
    public interface ISrvVisit
    {
        VisitInfo GetVisitById(int VisitId);

        IList<VisitInfo> GetVisitsAll();

        IList<VisitInfo> GetVisitsByVisitor(int VisitorId);

        IList<VisitInfo> GetVisitsByDate(DateTime VisitDate, 
                                         string LastName="", string IdentificationNumber="", string CardNumber="",
                                         string VisitStatus="");

        IList<VisitInfo> GetVisitsExt(DateTime? VisitDateFrom, DateTime? VisitDateTo,
                                      string LastName = "", string IdentificationNumber = "", string CardNumber = "",
                                      string VisitStatus = "");

        bool InsertVisit(VisitInfo myVisitInfo, Visitor myVisitor, short UserId);

        bool UpdateVisit(VisitInfo myVisitInfo, short UserId);

        bool DeleteVisit(int VisitInfoId, short UserId);

        bool UpdateVisitTimeExit(int VisitInfoId, short UserId);
    }
}
