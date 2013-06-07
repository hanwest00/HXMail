using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.Model;

namespace HXMail.IBLL
{
    public interface IAttachmentManage
    {
        int CreateAttactment(AttachmentInfo attachment);
    }
}
