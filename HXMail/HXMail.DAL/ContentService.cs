using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using HXMail.Model;

namespace HXMail.DAL
{
    public class ContentService
    {
        private ContentService()
        { }

        #region Singleton
        private static ContentService instance = new ContentService();
        private static object lockObj = new object();
        public static ContentService GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new ContentService();
                }
            }
            return instance;
        }
        #endregion

        private SQLiteParameter[] Parameter(ContentInfo Content)
        {
            return new SQLiteParameter[] {
               new SQLiteParameter("@Id",Content.Id),
               new SQLiteParameter("@MailId",Content.MailId),
               new SQLiteParameter("@ContentType",Content.ContentType),
               new SQLiteParameter("@Charset",Content.Charset),
               new SQLiteParameter("@Encoding",Content.Encoding),
               new SQLiteParameter("@Content",Content.Content)
           };
        }

        public int Insert(ContentInfo Content)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "insert into hm_mailinfo_Content (MailId,ContentType,Charset,Encoding,Content)values(@MailId,@ContentType,@Charset,@Encoding,@Content)", Parameter(Content));
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
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("delete from hm_mailinfo_Content where Id = {0}", Id));
            }
            catch
            {
                return -1;
            }
        }

        public int UpDate(ContentInfo Content)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "update hm_mailinfo_Content set MailId = @MailId,ContentType = @ContentType,Charset = @Charset, = @Encoding, = @Content where Id = @Id", Parameter(Content));
            }
            catch
            {
                return -1;
            }
        }

        public ContentInfo GetById(int Id)
        {
            try
            {
                return SqliteHelper.InfoBinder<ContentInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,MailId,ContentType,Charset,Encoding,Content from hm_mailinfo_Content where Id = {0}", Id)));
            }
            catch
            {
                return null;
            }
        }

        public IList<ContentInfo> GetByMailId(int mailId)
        {
            try
            {
                IList<ContentInfo> ret = new List<ContentInfo>();
                using (SQLiteDataReader read = SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,MailId,ContentType,Charset,Encoding,Content from hm_mailinfo_Content where MailId = {0}", mailId)))
                {
                    while (read.Read())
                    {
                        ret.Add(new ContentInfo((long)read[0], (int)read[1], read[2].ToString(), read[3].ToString(), read[4].ToString(), read[5].ToString()));
                    }
                }
                return ret;
            }
            catch
            {
                return null;
            }
        }
    }
}
