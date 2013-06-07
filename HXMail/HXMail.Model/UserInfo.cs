using System;
using System.Collections.Generic;
using System.Text;

namespace HXMail.Model
{
    [Serializable]
    public class UserInfo
    {
        private Int64 _Id;
        private string _UserName;
        private string _Password;
        private string _EmailAddress;
        private string _PopAddress;
        private string _SmtpAddress;
        private int _PopPort = 110;
        private int _SmtpPort = 25;
        private DateTime _CreateDate;
        private int _Del;

        public Int64 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = value; }
        }

        public string PopAddress
        {
            get { return _PopAddress; }
            set { _PopAddress = value; }
        }

        public string SmtpAddress
        {
            get { return _SmtpAddress; }
            set { _SmtpAddress = value; }
        }

        public int PopPort
        {
            get { return _PopPort; }
            set
            {
                _PopPort = value;
            }
        }

        public int SmtpPort
        {
            get { return _SmtpPort; }
            set
            {
                _SmtpPort = value;
            }
        }

        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set
            {
                _CreateDate = value;
            }
        }

        public int Del
        {
            get { return _Del; }
            set { _Del = value; }
        }

        public bool IsEmpty()
        {
            if (string.IsNullOrWhiteSpace(_UserName) || string.IsNullOrWhiteSpace(_EmailAddress) || string.IsNullOrWhiteSpace(_Password) || string.IsNullOrWhiteSpace(_PopAddress) || string.IsNullOrWhiteSpace(_SmtpAddress))
                return true;
            return false;
        }
    }
}
