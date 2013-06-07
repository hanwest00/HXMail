using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HXMail.DAL;
using HXMail.Model;
using HXMail.IBLL;

namespace HXMail.BLL
{
    public class UserManage : IUserManage
    {
        private static readonly UserService userService = HXMail.DAL.UserService.GetInstance();

        #region events
        public delegate void CreatedUserHandler();
        public delegate void CreateUserErrorHandler(Exception e);
        public event CreatedUserHandler OnCreatedUserEvent;
        public event CreateUserErrorHandler OnCreateUserErrorEvent;

        public delegate void RemovedUserHandler();
        public delegate void RemoveUserErrorHandler(Exception e);
        public event RemovedUserHandler OnRemovedUserEvent;
        public event RemoveUserErrorHandler OnRemoveUserErrorEvent;
        #endregion

        public int CreateUser(UserInfo User)
        {
            int ret = 0;
            try
            {
                User.Password = HXMail.Common.EncryptHelper.HXMailEncrypt(User.Password);
                if (userService.GetByUserAndPass(User.EmailAddress, User.Password) != null)
                    return 2;
                ret = userService.Insert(User);
                Common.FileExcute.FolderCreate(Common.PathInfo.GetUserAttachmentDir(User.EmailAddress));
                Common.FileExcute.FolderCreate(Common.PathInfo.GetUserErrorLogDir(User.EmailAddress));
                Common.FileExcute.FolderCreate(Common.PathInfo.GetUserTempDir(User.EmailAddress));
                if (ret == 1)
                    OnCreatedUserEvent();
            }
            catch (Exception e)
            {
                OnCreateUserErrorEvent(e);
            }
            return ret;
        }

        public int RemoveUser(string emailAddress, string password)
        {
            try
            {
                UserInfo user = userService.GetByUserAndPass(emailAddress, password);
                if (user == null)
                    return 0;
                if (userService.Delete(user.Id) > 0)
                {
                    Common.FileExcute.DeleteFolder(Common.PathInfo.GetUserAttachmentDir(user.EmailAddress));
                    Common.FileExcute.DeleteFolder(Common.PathInfo.GetUserLogDir(user.EmailAddress));
                }
            }
            catch (Exception e)
            {
                OnRemoveUserErrorEvent(e);
            }
            OnRemovedUserEvent();
            return 1;
        }

        public int LogicRemoveUser(string emailAddress, string password)
        {
            try
            {
                UserInfo user = userService.GetByUserAndPass(emailAddress, password);
                if (user == null)
                    return 0;
                if (!(userService.LogicDelete(user.Id) > 0))
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                OnRemoveUserErrorEvent(e);
            }
            OnRemovedUserEvent();
            return 1;
        }

        public IList<UserInfo> GetUserList()
        {
            try
            {
                return userService.GetAll(0, 0);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
