using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.DAL;
using HXMail.Model;
using HXMail.IBLL;

namespace HXMail.BLL
{
    public class MailManage : IMailManage
    {
        private static readonly MailService mailService = HXMail.DAL.MailService.GetInstance();

        public int CreateMail(MailInfo mail)
        {
            return mailService.Insert(mail);
        }

        public int RemoveMail(int id)
        {
            return mailService.Delete(id);
        }

        public int UpDateDraftMail(MailInfo mail)
        {
            return mailService.UpDate(mail);
        }

        public MailInfo LoadMailById(int id)
        {
            return mailService.GetById(id);
        }

        public IList<MailInfo> LoadMails(int start, int end, int userId)
        {
            return mailService.GetAll(start, end, userId);
        }

        public int GetUserMailCount(int userId)
        {
            return mailService.GetUserMailCount(userId);
        }
    }
}
