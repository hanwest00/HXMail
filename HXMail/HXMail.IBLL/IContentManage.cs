using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.Model;

namespace HXMail.IBLL
{
    public interface IContentManage
    {
         int CreateContent(ContentInfo Content);
         int UpDateDraftContent(ContentInfo Content);
         IList<ContentInfo> GetByMailId(int mailId);
    }
}
