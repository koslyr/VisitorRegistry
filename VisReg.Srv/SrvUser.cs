using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using VisReg.Srv.Interfaces;
using VisReg.Dal.Models;
using VisReg.Dal.Models.Extension;

namespace VisReg.Srv
{
    public class SrvUser: ISrvUser
    {
        private VisRegDB _VisReg = null;

        public SrvUser()
        {
            _VisReg = new VisRegDB();
        }

        public bool IsExistUserName(string UserName)
        {
            var myUser = from use in _VisReg.User
                         where use.USE_UserName == UserName
                         select use;

            if (myUser.Count() > 0)
                return true;

            return false;
        }

        public bool IsExistUserPassword(short UserId)
        {
            var myUser = (from use in _VisReg.User
                          where use.USE_Id == UserId
                          select use).First();

            if (myUser.USE_Password != null)
                return true;

            return false;
        }

        public bool IsActiveUserName(string UserName)
        {
            var myUser = from use in _VisReg.User
                         where use.USE_UserName == UserName && use.USE_IsActive == true
                         select use;

            if (myUser.Count() > 0)
                return true;

            return false;
        }

        public bool IsUserAdmin(short UserId)
        {
            var getUserIsAdmin = (from use in _VisReg.User
                                  join uug in _VisReg.UserUserGroup on use.USE_Id equals uug.UUG_USE_Id
                                  where use.USE_Id == UserId && uug.UserGroup.USG_IsAdmin == true
                                  select use).Any();

            return getUserIsAdmin;
        }

        public List<User> GetUserAll(short UserId=-1)
        {
            var getUsers = from use in _VisReg.User
                           select use;

            if (UserId > -1)
                getUsers = getUsers.Where(x => x.USE_Id == UserId);

            return getUsers.ToList();
        }

        public User GetUserById(short UserId)
        {
            var myUser = from use in _VisReg.User
                         where use.USE_Id == UserId
                         select use;

            return myUser.FirstOrDefault();
        }

        public User GetUserByUserName(string UserName)
        {
            var myUser = from use in _VisReg.User
                         where use.USE_UserName == UserName
                         select use;

            return myUser.FirstOrDefault();
        }

        public IList<UserUserGroup> GetUserUserGroupByUserId(short UserId)
        {
            var getUserUserGroup = from use in _VisReg.User
                                   join uug in _VisReg.UserUserGroup on use.USE_Id equals uug.UUG_USE_Id
                                   where use.USE_Id == UserId
                                   select uug;

            return getUserUserGroup.ToList();
        }

        public List<short> GetUserGroupIdByUserId(short UserId)
        {
            var getUserGroupId = from uug in _VisReg.UserUserGroup
                                 where uug.UUG_USE_Id == UserId
                                 select uug.UUG_USG_Id;

            return getUserGroupId.ToList();
        }

        public IList<UserGroup> GetUserGroupAll()
        {
            var getUserGroup = from usg in _VisReg.UserGroup
                               orderby usg.USG_Order
                               select usg;

            return getUserGroup.ToList();
        }

        // Ανάκτηση Επιτροπής με τα στοιχεία των Υπαλλήλων.
        public IList<ExtUserUserGroup> GetUserUserGroupInfo(short UserId)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                // Εκτέλεση SP.
                IList<ExtUserUserGroup> myLstExtUserUserGroup = myVisRegDB.Database.SqlQuery
                                                                <ExtUserUserGroup>
                                                                ("exec dbo.spGetUserUserGroupInfo @UserId",
                                                                        new SqlParameter("@UserId", UserId))
                                                                .ToList();

                return myLstExtUserUserGroup;
            }
        }

        #region CRUD
        // ΕΙΣΑΓΩΓΗ ΝΕΟΥ ΧΡΗΣΤΗ.
        public bool InsertUser(User myUser, List<short> LstUserGroupId)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    //1. Εισαγωγή Χρήστη (User).
                    myVisRegDB.User.Add(myUser);
                    myUser.USE_IsActive = true;
                    myUser.USE_LoginCounter = 0;

                    //2. Εισαγωγή Χρήση σε Ομάδες (UserUserGroup).
                    foreach (short item in LstUserGroupId)
                    {
                        UserUserGroup myUserUserGroup = new UserUserGroup();
                        myUserUserGroup.UUG_USG_Id = item;

                        myVisRegDB.UserUserGroup.Add(myUserUserGroup);
                        myUser.UserUserGroup.Add(myUserUserGroup);
                    }

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

        // ΕΝΗΜΕΡΩΣΗ ΥΦΙΣΤΑΜΕΝΟΥ ΧΡΗΣΤΗ.
        public bool UpdateUser(User myUser, List<short> LstUserGroupId)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    //1. Ενημέρωση Χρήστη (User).
                    myVisRegDB.User.Attach(myUser);
                    myVisRegDB.Entry(myUser).State = EntityState.Modified;
                    
                    //
                    myVisRegDB.Entry(myUser).Property(x => x.USE_Salt).IsModified = false;
                    myVisRegDB.Entry(myUser).Property(x => x.USE_Password).IsModified = false;

                    //2. Ενημέρωση Ομάδων που ανήκει ο Χρήστης (UserUserGroup).
                    var getUserUserGroup = from uug in myVisRegDB.UserUserGroup
                                           where uug.UUG_USE_Id == myUser.USE_Id
                                           select uug;

                    //2.1 Διαγραφή των υφιστάμενων Ομάδων (UserUserGroup), που άνηκε ο Χρήστης.
                    foreach (UserUserGroup itemUUG in getUserUserGroup)
                    {
                        myVisRegDB.UserUserGroup.Remove(itemUUG);
                    }

                    //2.2 Εισαγωγή εκ νέου των νέων Ομάδων (UserUserGroup).
                    foreach (short item in LstUserGroupId)
                    {
                        UserUserGroup myUserUserGroup = new UserUserGroup();
                        myUserUserGroup.UUG_USG_Id = item;

                        myVisRegDB.UserUserGroup.Add(myUserUserGroup);
                        myUser.UserUserGroup.Add(myUserUserGroup);
                    }

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

        // ΕΝΗΜΕΡΩΣΗ ΥΦΙΣΤΑΜΕΝΟΥ ΧΡΗΣΤΗ ΓΙΑ ΤΗΝ ΠΡΟΣΒΑΣΗ.
        public void UpdateUserLoginInfo(short UserId)
        {
            try
            {
                //1. Ενημέρωση Χρήστη (User).
                var myUser = _VisReg.User.SingleOrDefault(x => x.USE_Id == UserId);
                if (myUser != null)
                {
                    myUser.USE_LoginLastDate = DateTime.Now;
                    myUser.USE_LoginCounter = (short)(myUser.USE_LoginCounter + 1);

                    _VisReg.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        // ΕΝΗΜΕΡΩΣΗ ΣΥΝΘΗΜΑΤΙΚΟΥ.
        public bool SetUserPassword(short UserId, string UserPassword)
        {
            using (VisRegDB myVisRegDB = new VisRegDB())
            {
                try
                {
                    //1. Στοιχεία Χρήστη (για Υποψήφιο) προς καταχώριση.
                    User myUser = myVisRegDB.User.Where(x => x.USE_Id == UserId).Single();
                    myVisRegDB.User.Attach(myUser);

                    //2. Δημιουργία Salt & Hash με βάση το Password που δήλωσε ο χρήστης.
                    SrvPassword myPasswordService = new SrvPassword();
                    string[] myHashPwdSlt = myPasswordService.GetHashPasswordAndSalt(UserPassword.Trim());

                    // 3. Ενημέρωση Password.
                    myUser.USE_Password = myHashPwdSlt[0];
                    myUser.USE_Salt = myHashPwdSlt[1];
                    myVisRegDB.Entry(myUser).Property(x => x.USE_Password).IsModified = true;
                    myVisRegDB.Entry(myUser).Property(x => x.USE_Salt).IsModified = true;


                    // Αποθήκευση στην Βάση.
                    myVisRegDB.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    string Error = ex.ToString();
                    return false;
                }
            }
        }
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            _VisReg.Dispose();
        }
    }
}
