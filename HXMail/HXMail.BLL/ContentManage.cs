using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.DAL;
using HXMail.IBLL;
using HXMail.Model;

namespace HXMail.BLL
{
    public class ContentManage : IContentManage
    {
        private static readonly ContentService contentService = HXMail.DAL.ContentService.GetInstance();

        public int CreateContent(ContentInfo Content)
        {
           return contentService.Insert(Content);
        }

        public int UpDateDraftContent(ContentInfo Content)
        {
            return contentService.UpDate(Content);
        }

        public IList<ContentInfo> GetByMailId(int mailId)
        {
            return contentService.GetByMailId(mailId);
        }
    }

}
