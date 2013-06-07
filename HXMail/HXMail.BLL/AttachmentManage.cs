using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.IBLL;
using HXMail.DAL;
using HXMail.Model;

namespace HXMail.BLL
{
    public class AttachmentManage : IAttachmentManage
    {
        private AttachmentService attachmentService = AttachmentService.GetInstance();

        public int CreateAttactment(AttachmentInfo attachment)
        {
            return attachmentService.Insert(attachment);
        }
    }
}
