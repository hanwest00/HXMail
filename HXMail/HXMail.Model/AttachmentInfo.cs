using System;
using System.Collections.Generic;
using System.Text;

namespace HXMail.Model
{
    [Serializable]
    public class AttachmentInfo
    {
        private Int64? _Id;
        private int _MailId;
        private string _FileName;
        private string _ContentType;
        private string _Encoding;
        private byte[] _FileBuffer;

        public AttachmentInfo(Int64? id, int mailId, string fileName, string contentType, string encoding, byte[] fileBuffer)
        {
            this._Id = id;
            this._MailId = mailId;
            this._FileName = fileName;
            this._ContentType = contentType;
            this._Encoding = encoding;
            this._FileBuffer = fileBuffer;
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
