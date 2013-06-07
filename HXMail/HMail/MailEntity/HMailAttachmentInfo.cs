using System;
using System.Collections.Generic;
using System.Text;

namespace HMail.MailEntity
{
    [Serializable]
    public class HMailAttachmentInfo
    {
        private string _FileName;
        private string _ContentType;
        private string _Encoding;
        private byte[] _FileBuffer;

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }

        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }

        public byte[] FileBuffer
        {
            get { return _FileBuffer; }
            set { _FileBuffer = value; }
        }

        public override string ToString()
        {
            return this.FileName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return this.GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            return this.FileBuffer.GetHashCode();
        }
    }
}
