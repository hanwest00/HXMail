using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HMail.MailEntity;
using HXMail.Model;
using HXMail.Common;

namespace HMConvert
{
    /// <summary>
    /// 把mail实体转换为数据库实体
    /// </summary>
    public class ConvertMailEntity
    {
        public ConvertMailEntity() 
        { }

        /// <summary>
        /// 把邮件UserInfo转换为数据库实体
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static HMail.MailEntity.UserInfo ConvertToMailEntity(HXMail.Model.UserInfo user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (user.IsEmpty())
                return null;

            try
            {
                HMail.MailEntity.UserInfo userInfo = new HMail.MailEntity.UserInfo();
                userInfo.EmailAddress = user.EmailAddress;
                userInfo.Password = EncryptHelper.HXMailDecrypt(user.Password);
                userInfo.PopAddress = user.PopAddress;
                userInfo.SmtpAddress = user.SmtpAddress;
                userInfo.PopPort = user.PopPort;
                userInfo.SmtpPort = user.SmtpPort;
                userInfo.UserName = user.EmailAddress.Split('@')[0];
                return userInfo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 把邮件MailInfo转换为数据库实体
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hmailInfo"></param>
        /// <returns></returns>
        public static MailInfo ConvertToDbMailEntity(int userId, HMailInfo hmailInfo)
        {
            if(hmailInfo == null)
                throw new ArgumentNullException("hmailInfo");

            try
            {
                MailInfo mailTmp = new MailInfo();
                StringBuilder sb = new StringBuilder();
                mailTmp.Subject = hmailInfo.SubJect;
                mailTmp.FromAddress = hmailInfo.From.Address;

                if (hmailInfo.To != null)
                    hmailInfo.To.AsParallel().ForAll((s) => { sb.Append(string.Format("{0}{1}", s.Address, ",")); });
                mailTmp.ToAddress = sb.ToString().TrimEnd(',');

                sb.Clear();
                if (hmailInfo.Cc != null)
                    hmailInfo.Cc.AsParallel().ForAll((v) => { sb.Append(string.Format("{0}{1}", v.Address, ",")); });
                mailTmp.Cc = sb.ToString().TrimEnd(',');

                sb.Clear();
                if (hmailInfo.Bcc != null)
                    hmailInfo.Bcc.AsParallel().ForAll((n) => { sb.Append(string.Format("{0}{1}", n.Address, ",")); });
                mailTmp.Bcc = sb.ToString().TrimEnd(',');

                mailTmp.ReceiveDate = ConvertCommon.ConvertFromMailDate(hmailInfo.Date);
                mailTmp.DownloadDate = DateTime.Now;
                mailTmp.Type = (int)MailInfo.MailType.Common;
                mailTmp.UserId = userId;
                mailTmp.Del = 0;
                mailTmp.Status = (int)MailInfo.MailStatus.Unread;

                return mailTmp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 把邮件ContentInfo转换为数据库实体
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hmailInfo"></param>
        /// <returns></returns>
        public static IList<ContentInfo> ConvertToDbContentEntity(int mailId, HMailInfo hmailInfo)
        {
            if (hmailInfo == null)
                throw new ArgumentNullException("hmailInfo");

            try
            {
                IList<ContentInfo> contentList = new List<ContentInfo>();
                if (hmailInfo.ContentInfo.Count() > 0)
                    hmailInfo.ContentInfo.AsParallel().ForAll(v => contentList.Add(new ContentInfo(null,mailId,v.ContentType,v.Charset,v.Encoding,v.Content)));
                return contentList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 把邮件AttachmentInfo转换为数据库实体
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hmailInfo"></param>
        /// <returns></returns>
        public static IList<AttachmentInfo> ConvertToDbAttachEntity(int mailId, HMailInfo hmailInfo)
        {
            if (hmailInfo == null)
                throw new ArgumentNullException("hmailInfo");

            try
            {
                IList<AttachmentInfo> AttList = new List<AttachmentInfo>();
                if (hmailInfo.Attachments.Count() > 0)
                    hmailInfo.Attachments.AsParallel().ForAll(v => AttList.Add(new AttachmentInfo(null, mailId, v.FileName, v.ContentType, v.Encoding, v.FileBuffer)));
                return AttList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
