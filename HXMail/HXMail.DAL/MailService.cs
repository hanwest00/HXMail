using System;
using System.Collections.Generic;
using System.Text;
using HXMail.Model;
using System.Data.SQLite;

namespace HXMail.DAL
{
    public class MailService
    {
        private MailService()
        { }

        #region Singleton
        private static volatile MailService instance = null;
        private static object lockObj = new object();
        public static MailService GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new MailService();
                }
            }
            return instance;
        }
        #endregion

        private SQLiteParameter[] Parameter(MailInfo mail)
        {
            return new SQLiteParameter[] {
               new SQLiteParameter("@Id",mail.Id),
               new SQLiteParameter("@UserId",mail.UserId),
               new SQLiteParameter("@FromAddress",mail.FromAddress),
               new SQLiteParameter("@ToAddress",mail.ToAddress),
               new SQLiteParameter("@Cc",mail.Cc),
               new SQLiteParameter("@Bcc",mail.Bcc),
               new SQLiteParameter("@Subject",mail.Subject),
               new SQLiteParameter("@DownloadDate",mail.DownloadDate),
               new SQLiteParameter("@ReceiveDate",mail.ReceiveDate),
               new SQLiteParameter("@Type",mail.Type),
               new SQLiteParameter("@Status",mail.Status)
           };
        }

        public int Insert(MailInfo mail)
        {
            try
            {
                lock (this)
                {
                    if (SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "insert into hm_mailinfo (UserId,FromAddress,ToAddress,Cc,Bcc,Subject,DownloadDate,ReceiveDate,Type,Status)values(@UserId,@FromAddress,@ToAddress,@Cc,@Bcc,@Subject,@DownloadDate,@ReceiveDate,@Type,@Status)", Parameter(mail)) > 0)
                        return Convert.ToInt32(SqliteHelper.ExecuteScaler(System.Data.CommandType.Text, "select id from hm_mailinfo order by id desc limit 0,1"));
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        public int Delete(int id)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("delete from hm_mailinfo where Id = {0}", id));
            }
            catch
            {
                return -1;
            }
        }

        public int UpDate(MailInfo mail)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "update hm_mailinfo set UserId = @UserId FromAddress = @FromAddress,ToAddress = @ToAddress,Cc = @Cc,Bcc = @Bcc,Subject = @Subject,DownloadDate = @DownloadDate,ReceiveDate = @ReceiveDate,Type=@Type where Id = @Id", Parameter(mail));
            }
            catch
            {
                return -1;
            }
        }

        public MailInfo GetById(int id)
        {
            try
            {
                return SqliteHelper.InfoBinder<MailInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,UserId,FromAddress,ToAddress,Cc,Bcc,Subject,DownloadDate,ReceiveDate,Type,Status from hm_mailinfo where Id = {0}", id)));
            }
            catch
            {
                return null;
            }
        }

        public IList<MailInfo> GetAll(int start, int end, int userId)
        {
            try
            {
                return SqliteHelper.InfosBinder<MailInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,UserId,FromAddress,ToAddress,Cc,Bcc,Subject,DownloadDate,ReceiveDate,Type,Del,status from hm_mailinfo {0} {1}", userId > 0 ? string.Format("where UserId = {0}", userId) : "", end == 0 ? "" : string.Format("limit {0},{1}", start, end))));
            }
            catch
            {
                return null;
            }
        }

        public int GetUserMailCount(int userId)
        {
            try
            {
                return Convert.ToInt32(SqliteHelper.ExecuteScaler(System.Data.CommandType.Text, string.Format("select count(1) from hm_mailinfo where UserId = {0}", userId)));
            }
            catch
            {
                return -1;
            }
        }
    }
}
