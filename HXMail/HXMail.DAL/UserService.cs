using System;
using System.Collections.Generic;
using System.Text;
using HXMail.Model;
using System.Data.SQLite;

namespace HXMail.DAL
{
    public class UserService
    {
        private UserService()
        { }

        #region Singleton
        private static volatile UserService instance = new UserService();
        private static object lockObj = new object();
        public static UserService GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new UserService();
                }
            }
            return instance;
        }
        #endregion

        private SQLiteParameter[] Parameter(UserInfo User)
        {
            return new SQLiteParameter[] {
               new SQLiteParameter("@Id",User.Id),
               new SQLiteParameter("@UserName",User.UserName),
               new SQLiteParameter("@Password",User.Password),
               new SQLiteParameter("@EmailAddress",User.EmailAddress),
               new SQLiteParameter("@PopAddress",User.PopAddress),
               new SQLiteParameter("@SmtpAddress",User.SmtpAddress),
               new SQLiteParameter("@PopPort",User.PopPort),
               new SQLiteParameter("@SmtpPort",User.SmtpPort),
               new SQLiteParameter("@CreateDate",User.CreateDate),
               new SQLiteParameter("@Del",User.Del)
           };
        }

        public int Insert(UserInfo User)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "insert into hm_user (UserName,Password,EmailAddress,PopAddress,SmtpAddress,PopPort,SmtpPort,CreateDate,Del)values(@UserName,@Password,@EmailAddress,@PopAddress,@SmtpAddress,@PopPort,@SmtpPort,@CreateDate,@Del)", Parameter(User));
            }
            catch
            {
                return -1;
            }
        }

        public int Delete(Int64 Id)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("delete from hm_user where Id = {0}", Id));
            }
            catch
            {
                return -1;
            }
        }

        public int Delete(string EmailAddress, string Password)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("delete from hm_user where EmailAddress = '{0}' and Password = '{1}'", EmailAddress, Password));
            }
            catch
            {
                return -1;
            }
        }

        public int LogicDelete(Int64 Id)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("update from hm_user set Del = 1 where Id = {0}", Id));
            }
            catch
            {
                return -1;
            }
        }

        public int LogicDelete(string EmailAddress, string Password)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("update from hm_user set Del = 1 where EmailAddress = '{0}' and Password = '{1}'", EmailAddress, Password));
            }
            catch
            {
                return -1;
            }
        }

        public int UpDate(UserInfo User)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "update hm_user set UserName = @UserName,Password = @Password,EmailAddress = @EmailAddress,PopAddress = @PopAddress,SmtpAddress = @SmtpAddress,PopPort = @PopPort,SmtpPort = @SmtpPort,CreateDate = @CreateDate,Del = @Del where Id = @Id", Parameter(User));
            }
            catch
            {
                return -1;
            }
        }

        public UserInfo GetById(int Id)
        {
            try
            {
                return SqliteHelper.InfoBinder<UserInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,UserName,Password,EmailAddress,PopAddress,SmtpAddress,PopPort,SmtpPort,CreateDate,Del from hm_user where Id = {0} and Del = 0", Id)));
            }
            catch
            {
                return null;
            }
        }

        public IList<UserInfo> GetAll(int start, int end)
        {
            try
            {
                return SqliteHelper.InfosBinder<UserInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,UserName,Password,EmailAddress,PopAddress,SmtpAddress,PopPort,SmtpPort,CreateDate,Del from hm_user where Del = 0 {0}", end == 0 ? "" : string.Format("limit {0},{1}", start, end))));
            }
            catch
            {
                return null;
            }
        }

        public UserInfo GetByUserAndPass(string EmailAddress, string password)
        {
            try
            {
                return SqliteHelper.InfoBinder<UserInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,UserName,Password,EmailAddress,PopAddress,SmtpAddress,PopPort,SmtpPort,CreateDate,Del from hm_user where EmailAddress = '{0}' and Password = '{1}'", EmailAddress, password)));
            }
            catch
            {
                return null;
            }
        }
    }
}
