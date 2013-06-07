using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.Model;

namespace HXMail.IBLL
{
    public interface IMailManage
    {
        int CreateMail(MailInfo mail);


        int RemoveMail(int Id);


        int UpDateDraftMail(MailInfo mail);


        MailInfo LoadMailById(int Id);

        /// <summary>
        /// 获取DB中的邮件记录
        /// </summary>
        /// <param name="start">起始号</param>
        /// <param name="end">结束号，为0是表示获取所以记录</param>
        /// <param name="userId">用户ID</param>
        /// <returns>获取到的MailInfo</returns>
        IList<MailInfo> LoadMails(int start, int end, int UserId);

        int GetUserMailCount(int userId);

    }
}
