using System.Collections.Generic;
using VisReg.Dal.Models;
using VisReg.Dal.Models.Extension;

namespace VisReg.Srv.Interfaces
{
    public interface ISrvUser
    {
        List<User> GetUserAll(short UserId=-1);

        User GetUserById(short UserId);

        User GetUserByUserName(string UserName);

        List<short> GetUserGroupIdByUserId(short UserId);

        IList<UserUserGroup> GetUserUserGroupByUserId(short UserId);

        IList<ExtUserUserGroup> GetUserUserGroupInfo(short UserId);

        bool IsExistUserName(string UserName);

        bool IsExistUserPassword(short UserId);

        bool IsActiveUserName(string UserName);

        bool IsUserAdmin(short UserId);

        bool InsertUser(User myUser, List<short> LstUserGroupId);

        bool UpdateUser(User myUser, List<short> LstUserGroupId);

        bool SetUserPassword(short UserId, string UserPassword);

        void UpdateUserLoginInfo(short UserId);
    }
}
