using System;
using System.Collections.Generic;
using System.Text;

namespace HMail.MailEntity
{
    [Serializable]
    public class HMailContentInfo
    {
        private string _ContentType;
        private string _Charset;
        private string _Encoding;
        private string _Content;

        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        public string Charset
        {
            get { return _Charset; }
            set { _Charset = value; }
        }

        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }

        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        public override string ToString()
        {
            return this.Content;
        }

        public override int GetHashCode()
        {
            return this.Content.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return this.Content.Equals(obj.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
