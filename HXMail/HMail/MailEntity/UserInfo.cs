using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace HMail.MailEntity
{   
    [Serializable]
    public class UserInfo
    {
        private string _UserName;
        private string _Password;
        private string _EmailAddress;
        private string _PopAddress;
        private string _SmtpAddress;
        private int _PopPort = 110;
        private int _SmtpPort = 25;

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
    }                
}
