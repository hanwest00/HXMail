using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace HMail.MailEntity
{
    [Serializable]
    public class HMailInfo
    {
        private int _MailId;
        private MailAddress _From;
        private IList<MailAddress> _To;
        private IList<MailAddress> _Cc;
        private IList<MailAddress> _Bcc;
        private string _SubJect;
        private IList<HMailContentInfo> _ContentInfo;
        private IList<HMailAttachmentInfo> _Attachments;
        private string _Date;

        public int MailId
        {
            get { return _MailId; }
            set { _MailId = value; }
        }

        public MailAddress From
        {
            get { return _From; }
            set { _From = value; }
        }

        public IList<MailAddress> To
        {
            get { return _To; }
            set { _To = value; }
        }

        public IList<MailAddress> Cc
        {
            get { return _Cc; }
            set { _Cc = value; }
        }

        public IList<MailAddress> Bcc
        {
            get { return _Bcc; }
            set { _Bcc = value; }
        }

        public string SubJect
        {
            get { return _SubJect; }
            set { _SubJect = value; }
        }

        public IList<HMailContentInfo> ContentInfo
        {
            get { return _ContentInfo; }
            set { _ContentInfo = value; }
        }

        public IList<HMailAttachmentInfo> Attachments
        {
            get { return _Attachments; }
            set { _Attachments = value; }
        }

        public string Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        public override string ToString()
        {
            if (this.ContentInfo == null)
                return "";
            StringBuilder strBud = new StringBuilder();
            foreach (HMailContentInfo info in this.ContentInfo)
            {
                strBud.Append(info.Content);
            }
            return strBud.ToString();
        }

    }
}
