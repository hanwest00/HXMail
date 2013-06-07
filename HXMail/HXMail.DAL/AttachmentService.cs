using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using HXMail.Model;

namespace HXMail.DAL
{
    public class AttachmentService
    {
        private AttachmentService()
        { }

        #region Singleton
        private static volatile AttachmentService instance = null;
        private static object lockObj = new object();
        public static AttachmentService GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new AttachmentService();
                }
            }
            return instance;
        }
        #endregion

        private SQLiteParameter[] Parameter(AttachmentInfo Attachment)
        {
            return new SQLiteParameter[] {
               new SQLiteParameter("@Id",Attachment.Id),
               new SQLiteParameter("@MailId",Attachment.MailId),
               new SQLiteParameter("@FileName",Attachment.FileName),
               new SQLiteParameter("@ContentType",Attachment.ContentType),
               new SQLiteParameter("@Encoding",Attachment.Encoding),
               new SQLiteParameter("@FileBuufer",Attachment.FileBuffer)
           };
        }

        public int Insert(AttachmentInfo Attachment)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "insert into hm_mailinfo_Attachment (MailId,FileName,ContentType,Encoding,FileBuufer)values(@MailId,@FileName,@ContentType,@Encoding,@FileBuufer)", Parameter(Attachment));
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
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, string.Format("delete from hm_mailinfo_Attachment where Id = {0}", Id));
            }
            catch
            {
                return -1;
            }
        }

        public int UpDate(AttachmentInfo Attachment)
        {
            try
            {
                return SqliteHelper.ExecuteNoQuery(System.Data.CommandType.Text, "update hm_mailinfo_Attachment set MailId = @MailId,FileName = @FileName, = @ContentType,Encoding = @Encoding,FileBuufer = @FileBuufer where Id = @Id", Parameter(Attachment));
            }
            catch
            {
                return -1;
            }
        }

        public AttachmentInfo GetById(int Id)
        {
            try
            {
                return SqliteHelper.InfoBinder<AttachmentInfo>(SqliteHelper.ExecuteReader(System.Data.CommandType.Text, string.Format("select Id,MailId,FileName,ContentType,Encoding,FileBuufer from hm_mailinfo_Attachment where Id = {0}", Id)));
            }
            catch
            {
                return null;
            }
        }
    }
}
