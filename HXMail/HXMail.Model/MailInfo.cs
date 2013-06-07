using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace HXMail.Model
{
    [Serializable]
    public class MailInfo
    {
        private Int64 _Id;
        private int _UserId;
        private string _FromAddress;
        private string _ToAddress;
        private string _Cc;
        private string _Bcc;
        private string _Subject;
        private DateTime _DownloadDate;
        private DateTime _ReceiveDate;
        private int _Type = (int)MailType.Common;
        private int _Del;
        private int _Status;

        public MailInfo() 
        {
          
        } 

        public Int64 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string FromAddress
        {
            get { return _FromAddress; }
            set { _FromAddress = value; }
        }

        public string ToAddress
        {
            get { return _ToAddress; }
            set { _ToAddress = value; }
        }

        public string Cc
        {
            get { return _Cc; }
            set { _Cc = value; }
        }

        public string Bcc
        {
            get { return _Bcc; }
            set { _Bcc = value; }
        }

        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }

        public DateTime DownloadDate
        {
            get { return _DownloadDate; }
            set { _DownloadDate = value; }
        }

        public DateTime ReceiveDate
        {
            get { return _ReceiveDate; }
            set { _ReceiveDate = value; }
        }

        public int Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public int Del
        {
            get { return _Del; }
            set { _Del = value; }
        }

        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public enum MailType
        {
            Common,
            Draft,
            Spam
        }

        public enum MailStatus
        {
            Unread,
            read
        }
    }
}
