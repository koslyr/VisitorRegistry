using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VisReg.Dal.Models;
using VisReg.Srv.Interfaces;

namespace VisReg.Srv
{
    public class SrvVisitor : ISrvVisitor
    {
        private VisRegDB _VisRegDB = null;

        public SrvVisitor()
        {
            _VisRegDB = new VisRegDB();
        }

        // Ανάκτηση Επισκέπτη.
        public Visitor GetVisitorById(int VisitorId)
        {
            var getVst = from vst in _VisRegDB.Visitor
                         where vst.VST_Id == VisitorId
                         select vst;

            return getVst.First();
        }

        // Ανάκτηση Επισκέπτη.
        public Visitor GetVisitorOnlyById(int VisitorId)
        {
            _VisRegDB.Configuration.ProxyCreationEnabled = false;
            var getVst = from vst in _VisRegDB.Visitor
                         where vst.VST_Id == VisitorId
                         select vst;

            return getVst.FirstOrDefault();
        }

        // Ανάκτηση Επισκέπτη.
        public Visitor GetVisitorByLastNameIdentNumber(string VstLastName, string VstIdentificationNumber)
        {
            var getVst = from vst in _VisRegDB.Visitor
                         where vst.VST_LastName == VstLastName.ToString().Trim().ToUpper() &&
                               vst.VST_IdentificationNumber == VstIdentificationNumber.ToString().Trim().ToUpper()
                         select vst;

            return getVst.FirstOrDefault();
        }

        // Έλεγχος εάν υπάρχει καταχωρημένος ήδη ο Αρ. Ταυτοπροσωπίας.
        public bool GetIsExistIdentificationNumber(string VstIdentificationNumber)
        {
            var myIsExist = (from vst in _VisRegDB.Visitor
                             where vst.VST_IdentificationNumber == VstIdentificationNumber.ToString().Trim().ToUpper()
                             select vst).Any();

            return myIsExist;
        }

        // Ανάκτηση Επισκέπτη με βάση τον Αρ. Ταυτοπροσωπίας.
        public Visitor GetVisitorByIdentificationNumber(string VstIdentificationNumber)
        {
            var getVst = from vst in _VisRegDB.Visitor
                         where vst.VST_IdentificationNumber == VstIdentificationNumber.ToString().Trim().ToUpper()
                         select vst;

            return getVst.FirstOrDefault();
        }

        // Ανάκτηση Λίστας Υπαλλήλων με βάση το Επώνυμο.
        public IList<Visitor> GetVisitorsByLastName(string VstLastName)
        {
            var getVst = from vst in _VisRegDB.Visitor
                         where vst.VST_LastName.StartsWith(VstLastName.ToString().Trim().ToUpper())
                         orderby vst.VST_LastName, vst.VST_FirstName
                         select vst;

            return getVst.ToList();
        }

        // Ανάκτηση Λίστας Υπαλλήλων με βάση τον Αρ. Τατυτ.
        public IList<Visitor> GetVisitorsByIdentificationNumber(string VstIdentificationNumber)
        {
            var getVst = from vst in _VisRegDB.Visitor
                         where vst.VST_IdentificationNumber.StartsWith(VstIdentificationNumber.ToString().Trim().ToUpper())
                         orderby vst.VST_LastName, vst.VST_FirstName
                         select vst;

            return getVst.ToList();
        }

        // Ανάκτηση Λίστας Επισκεπτών.
        public IList<Visitor> GetVisitorsAll()
        {
            IEnumerable<Visitor> getVst = from vst in _VisRegDB.Visitor
                                          orderby vst.VST_LastName, vst.VST_FirstName, vst.VST_FatherName
                                          select vst;

            return getVst.ToList();
        }

        // Ανάκτηση Λίστας Επισκεπτών.
        public IList<Visitor> GetVisitors(string VstLastName = "", string VstIdentificationNumber = "", string VstMobileNumber = "")
        {
            IQueryable<Visitor> getVst = from vst in _VisRegDB.Visitor
                                         orderby vst.VST_LastName, vst.VST_FirstName, vst.VST_FatherName
                                         select vst;

            if (VstLastName != "")
                getVst = getVst.Where(x => x.VST_LastName.StartsWith(VstLastName.ToString().Trim().ToUpper()));
            if (VstIdentificationNumber != "")
                getVst = getVst.Where(x => x.VST_IdentificationNumber.Contains(VstIdentificationNumber.ToString().Trim().ToUpper()));
            if (VstMobileNumber != "")
                getVst = getVst.Where(x => x.VST_MobileNumber.Contains(VstMobileNumber.ToString().Trim()));

            return getVst.ToList();
        }

        #region CRUD
        // ΕΙΣΑΓΩΓΗ ΝΕΟΥ ΕΠΙΣΚΕΠΤΗ.
        public bool InsertVisitor(Visitor myVisitor)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    // Εισαγωγή Επισκέπτη (Visitor).
                    myVisRegDB.Visitor.Add(myVisitor);

                    // ΑΠΟΘΗΚΕΥΣΗ ΣΤΗ ΒΑΣΗ.
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

        // ΕΝΗΜΕΡΩΣΗ ΥΦΙΣΤΑΜΕΝΟΥ ΕΠΙΣΚΕΠΤΗ.
        public bool UpdateVisitor(Visitor myVisitor)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    // Ενημέρωση Υπαλλήλου (Visitor).
                    myVisRegDB.Visitor.Attach(myVisitor);
                    myVisRegDB.Entry(myVisitor).State = EntityState.Modified;

                    // ΑΠΟΘΗΚΕΥΣΗ ΣΤΗ ΒΑΣΗ.
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

        protected virtual void Dispose(bool disposing)
        {
            _VisRegDB.Dispose();
        }
    }
}