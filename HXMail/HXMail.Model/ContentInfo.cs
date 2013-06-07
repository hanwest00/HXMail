using System;
using System.Collections.Generic;
using System.Text;

namespace HXMail.Model
{
    [Serializable]
    public class ContentInfo
    {
        private Int64? _Id;
        private int _MailId;
        private string _ContentType;
        private string _Charset;
        private string _Encoding;
        private string _Content;

        public ContentInfo(Int64? id, int mailId, string contentType, string charset, string encoding, string content) 
        {
            this._Id = id;
            this._MailId = mailId;
            this._ContentType = contentType;
            this._Charset = charset;
            this._Encoding = encoding;
            this._Content = content;
        }

        public Int64? Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        public int MailId
        {
            get { return _MailId; }
            set { _MailId = value; }
        }

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
