using System;
using System.Collections.Generic;
using System.Text;
using HXMail.Model;

namespace HXMail.IBLL
{
    public interface IUserManage
    {
        int CreateUser(UserInfo User);
        int RemoveUser(string emailAddress, string password);
        int LogicRemoveUser(string emailAddress, string password);
        IList<UserInfo> GetUserList();
    }
}
