using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using HXMail.Model;

namespace HXMail.DAL
{
    public class ContactService
    {
        private ContactService()
        { }

        #region Singleton
        private static volatile ContactService instance = new ContactService();
        private static object lockObj = new object();
        public static ContactService GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new ContactService();
                }
            }
            return instance;
        }
        #endregion

        private SQLiteParameter[] Parameter(ContactInfo Contact)
        {
            return new SQLiteParameter[] {
               new SQLiteParameter("@Id",Contact.Id),
               new SQLiteParameter("@UserId",Contact.UserId),
               new SQLiteParameter("@Name",Contact.Name),
               new SQLiteParameter("@Address",Contact.Address)
           };
        }

        public int Insert(ContactInfo Contact)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "insert into hm_contact (UserId,Name,Address)values(@UserId,@Name,@Address)", Parameter(Contact));
            }
            catch
            {
                return -1;
            }
        }

        public int Delete(int Id)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("delete from hm_contact where Id = {0}", Id));
            }
            catch
            {
                return -1;
            }
        }

        public int DeleteByUserId(int UserId)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("delete from hm_contact where UserId = {0}", UserId));
            }
            catch
            {
                return -1;
            }
        }

        public int UpDate(ContactInfo Contact)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "update hm_contact set UserId = @UserId,Name = @Name,Address = @Address where Id = @Id", Parameter(Contact));
            }
            catch
            {
                return -1;
            }
        }

        public ContactInfo GetById(int Id)
        {
            try
            {
                return SqliteHelper.InfoBinder<ContactInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,UserId,Name,Address from hm_contact where Id = {0}", Id)));
            }
            catch
            {
                return null;
            }
        }

        public IList<ContactInfo> GetAll(int start, int end, int UserId)
        {
            try
            {
                return SqliteHelper.InfosBinder<ContactInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("selectId,UserId,Name,Address from hm_contact {0} {1}", UserId > 0 ? string.Format("where UserId = {0}", UserId) : "", end == 0 ? "" : string.Format("limit {0},{1}", start, end))));
            }
            catch
            {
                return null;
            }
        }
    }
}
