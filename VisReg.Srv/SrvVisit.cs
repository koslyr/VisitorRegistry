using System;
using System.Data;
using System.Data.Entity;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;
using VisReg.Dal.Models;
using VisReg.Srv.Interfaces;
using VisReg.Cmn.Enumeration;

namespace VisReg.Srv
{
    public class SrvVisit : ISrvVisit
    {
        private VisRegDB _VisRegDB = null;

        public SrvVisit()
        {
            _VisRegDB = new VisRegDB();
        }

        // Ανάκτηση Επίσκεψης.
        public VisitInfo GetVisitById(int VisitId)
        {
            var getVisit = from vsi in _VisRegDB.VisitInfo
                           where vsi.VSI_Id == VisitId
                           select vsi;

            return getVisit.FirstOrDefault();
        }

        // Ανάκτηση όλων των Επισκέψεων.
        public IList<VisitInfo> GetVisitsAll()
        {
            var getVsi = from vsi in _VisRegDB.VisitInfo
                         where vsi.VSI_IsActive == true
                         orderby vsi.VSI_Date, vsi.VSI_TimeEntrance
                         select vsi;

            return getVsi.ToList();
        }

        // Ανάκτηση Λίστας Επισκέψεων ανά Επισκέπτη.
        public IList<VisitInfo> GetVisitsByVisitor(int VisitorId)
        {
            var getVsi = from vsi in _VisRegDB.VisitInfo
                         where vsi.VSI_VST_Id == VisitorId && vsi.VSI_IsActive == true
                         orderby vsi.VSI_Date, vsi.VSI_TimeEntrance
                         select vsi;

            return getVsi.ToList();
        }

        // Ανάκτηση Λίστας Επισκέψεων ανά Ημ/νία.
        public IList<VisitInfo> GetVisitsByDate(DateTime VisitDate, 
                                                string LastName = "", string IdentificationNumber = "", string CardNumber="",
                                                string VisitStatus="All")
        {
            IQueryable<VisitInfo> getVsi = from vsi in _VisRegDB.VisitInfo
                                           where DbFunctions.TruncateTime(vsi.VSI_Date) == VisitDate && vsi.VSI_IsActive == true
                                           orderby vsi.VSI_TimeEntrance, vsi.VSI_CardNumber
                                           select vsi;

            if (LastName != "")
                getVsi = getVsi.Where(x => x.Visitor.VST_LastName.StartsWith(LastName.ToString().Trim().ToUpper()));
            if (IdentificationNumber != "")
                getVsi = getVsi.Where(x => x.VSI_IdentificationNumber.StartsWith(IdentificationNumber.ToString().Trim().ToUpper()));
            if (CardNumber != "")
                getVsi = getVsi.Where(x => x.VSI_CardNumber == CardNumber.ToString().Trim());
            if (VisitStatus != "All")
            {
                if (VisitStatus == "Open")
                    getVsi = getVsi.Where(x => x.VSI_TimeExit == null);
                else if (VisitStatus == "Closed")
                    getVsi = getVsi.Where(x => x.VSI_TimeExit != null);
            }

            return getVsi.ToList();
        }

        // Ανάκτηση Λίστας Επισκέψεων σε εύρος Ημ/νιών.
        public IList<VisitInfo> GetVisitsExt(DateTime? VisitDateFrom, DateTime? VisitDateTo,
                                             string LastName = "", string IdentificationNumber = "", string CardNumber = "",
                                             string VisitStatus = "All")
        {
            IQueryable<VisitInfo> getVsi = from vsi in _VisRegDB.VisitInfo
                                           where vsi.VSI_IsActive == true
                                           orderby vsi.VSI_Date descending, vsi.VSI_TimeEntrance, vsi.VSI_CardNumber
                                           select vsi;

            if (VisitDateFrom != null)
                getVsi = getVsi.Where(x => DbFunctions.TruncateTime(x.VSI_Date) >= VisitDateFrom);
            if (VisitDateTo != null)
                getVsi = getVsi.Where(x => DbFunctions.TruncateTime(x.VSI_Date) <= VisitDateTo);
            if (LastName != "")
                getVsi = getVsi.Where(x => x.Visitor.VST_LastName.StartsWith(LastName.ToString().Trim().ToUpper()));
            if (IdentificationNumber != "")
                getVsi = getVsi.Where(x => x.VSI_IdentificationNumber.StartsWith(IdentificationNumber.ToString().Trim().ToUpper()));
            if (CardNumber != "")
                getVsi = getVsi.Where(x => x.VSI_CardNumber == CardNumber.ToString().Trim());
            if (VisitStatus != "All")
            {
                if (VisitStatus == "Open")
                    getVsi = getVsi.Where(x => x.VSI_TimeExit == null);
                else if (VisitStatus == "Closed")
                    getVsi = getVsi.Where(x => x.VSI_TimeExit != null);
            }

            return getVsi.ToList();
        }

        
        // Ανάκτηση Επίσκεψης.
        //public int GetMaxVisitAA(DateTime VisitDate)
        //{
        //    int getMaxAA = _VisRegDB.VisitInfo.Where(t => DbFunctions.TruncateTime(t.VSI_Date) == VisitDate).Select(x => x.VSI_AA).DefaultIfEmpty(0).Max();

        //    return getMaxAA;
        //}

        #region CRUD
        // ΕΙΣΑΓΩΓΗ ΕΠΊΣΚΕΨΗΣ.
        public bool InsertVisit(VisitInfo myVisitInfo, Visitor myVisitor, short UserId)
        {
            int myVisitorId = 0;
            int myVisitInfoId = 0;

            using (TransactionScope myTrans = new TransactionScope())
            {
                try
                {
                    //1. Εισαγωγή στοιχείων Επισκέπτη.
                    using (VisRegDB myVisRegVisitorDB = new VisRegDB())
                    {
                        // Έλεγχος για Εισαγωγή/Ενημέρωση Επισκέπτη.
                        if (myVisitor.VST_Id <= 0)
                            myVisRegVisitorDB.Visitor.Add(myVisitor);
                        else
                        {
                            myVisRegVisitorDB.Visitor.Attach(myVisitor);
                            myVisRegVisitorDB.Entry(myVisitor).State = EntityState.Modified;
                        }

                        // ΑΠΟΘΗΚΕΥΣΗ ΕΠΙΣΚΕΠΤΗ.
                        myVisRegVisitorDB.SaveChanges();
                        // Ανάκτηση ID Επισκέπτη.
                        myVisitorId = myVisitor.VST_Id;
                    }

                    //2. Εισαγωγή στοιχείων Επίσκεψης.
                    using (VisRegDB myVisRegVisitDB = new VisRegDB())
                    {
                        myVisitInfo.VSI_VST_Id = myVisitorId;
                        myVisitInfo.VSI_IsActive = true;
                        myVisRegVisitDB.VisitInfo.Add(myVisitInfo);
                        
                        // ΑΠΟΘΗΚΕΥΣΗ ΕΠΙΣΚΕΨΗΣ.
                        myVisRegVisitDB.SaveChanges();
                        // Ανάκτηση ID Επίσκεψης.
                        myVisitInfoId = myVisitInfo.VSI_Id;
                    }

                    //3. Εισαγωγή Log.
                    using (VisRegDB myVisRegLogDB = new VisRegDB())
                    {
                        VisitInfoLog myVisitInfoLog = new VisitInfoLog();

                        myVisitInfoLog.VIL_VSI_Id = myVisitInfoId;
                        myVisitInfoLog.VIL_USE_Id = UserId;
                        myVisitInfoLog.VIL_ACT_Id = (short)EnmActionType.CreateVisit;
                        myVisitInfoLog.VIL_Date = DateTime.Now;
                        myVisRegLogDB.VisitInfoLog.Add(myVisitInfoLog);

                        // ΑΠΟΘΗΚΕΥΣΗ LOG.
                        myVisRegLogDB.SaveChanges();
                    }

                    // ΕΚΤΕΛΕΣΗ TRAN.
                    myTrans.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return false;
                }
            }
        }

        // ΕΝΗΜΕΡΩΣΗ ΕΠΙΣΚΕΨΗΣ.
        public bool UpdateVisit(VisitInfo myVisitInfo, short UserId)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    //1. Ενημέρωση Επίσκεψης (VisitInfo).
                    myVisRegDB.VisitInfo.Attach(myVisitInfo);
                    myVisRegDB.Entry(myVisitInfo).State = EntityState.Modified;

                    //2. Ενημέρωση Επισκέπτη (Visitor).
                    Visitor myVisitor = myVisRegDB.Visitor.Where(x => x.VST_Id == myVisitInfo.VSI_VST_Id).Single();
                    myVisRegDB.Visitor.Attach(myVisitor);
                    // Ενημέρωση στοιχείων Υπαλλήλου στην κεντρική καρτέλα.
                    myVisitor.VST_IdentificationNumber = myVisitInfo.VSI_IdentificationNumber;
                    // Specify the fields that should be updated.
                    myVisRegDB.Entry(myVisitor).Property(x => x.VST_IdentificationNumber).IsModified = true;

                    // 3. Εισαγωγή Log.
                    VisitInfoLog myVisitInfoLog = new VisitInfoLog();
                    myVisitInfoLog.VIL_VSI_Id = myVisitInfo.VSI_Id;
                    myVisitInfoLog.VIL_USE_Id = UserId;
                    myVisitInfoLog.VIL_ACT_Id = (short)EnmActionType.EditVisit;
                    myVisitInfoLog.VIL_Date = DateTime.Now;
                    myVisRegDB.VisitInfoLog.Add(myVisitInfoLog);

                    myVisRegDB.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return false;
                }
            }
        }

        // ΕΝΗΜΕΡΩΣΗ ΕΠΙΣΚΕΨΗΣ (μόνο για των ώρα εξόδου).
        public bool UpdateVisitTimeExit(int VisitInfoId, short UserId)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    //1. Ενημέρωση Επίσκεψης (VisitInfo).
                    VisitInfo myVisitInfo = myVisRegDB.VisitInfo.Where(x => x.VSI_Id == VisitInfoId).Single();
                    myVisRegDB.VisitInfo.Attach(myVisitInfo);

                    myVisitInfo.VSI_TimeExit = (TimeSpan)(DateTime.Now.TimeOfDay);
                    // Specify the fields that should be updated.
                    myVisRegDB.Entry(myVisitInfo).Property(x => x.VSI_TimeExit).IsModified = true;

                    // 3. Εισαγωγή Log.
                    VisitInfoLog myVisitInfoLog = new VisitInfoLog();
                    myVisitInfoLog.VIL_VSI_Id = myVisitInfo.VSI_Id;
                    myVisitInfoLog.VIL_USE_Id = UserId;
                    myVisitInfoLog.VIL_ACT_Id = (short)EnmActionType.EditVisitTimeExit;
                    myVisitInfoLog.VIL_Date = DateTime.Now;
                    myVisRegDB.VisitInfoLog.Add(myVisitInfoLog);

                    myVisRegDB.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return false;
                }
            }
        }

        // ΔΙΑΓΡΑΦΗ ΕΠΙΣΚΕΨΗΣ.
        public bool DeleteVisit(int VisitId, short UserId)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    // Ανάκτηση Επίσκεψης για Διαγραφή.
                    var getVisitInfo = (from vis in myVisRegDB.VisitInfo
                                        where vis.VSI_Id == VisitId
                                        select vis).Single();

                    //1. Ενημέρωση Επίσκεψης για Διαγραφή (Visit).
                    myVisRegDB.VisitInfo.Attach(getVisitInfo);
                    getVisitInfo.VSI_IsActive = false;
                    myVisRegDB.Entry(getVisitInfo).Property(x => x.VSI_IsActive).IsModified = true;

                    // 3. Εισαγωγή Log.
                    VisitInfoLog myVisitInfoLog = new VisitInfoLog();
                    myVisitInfoLog.VIL_VSI_Id = getVisitInfo.VSI_Id;
                    myVisitInfoLog.VIL_USE_Id = UserId;
                    myVisitInfoLog.VIL_ACT_Id = (short)EnmActionType.DeleteVisit;
                    myVisitInfoLog.VIL_Date = DateTime.Now;
                    myVisRegDB.VisitInfoLog.Add(myVisitInfoLog);

                    myVisRegDB.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return false;
                }
            }
        }
        #endregion

    }
}
