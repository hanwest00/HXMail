using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HXMail.Model
{
    [Serializable]
    public class ReceiveRecordInfo
    {
        private Int64 _Id;
        private int _UserId;
        private DateTime _ReceiveDate;
        private int _MailCount;

        private Int64 Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        private DateTime ReceiveDate
        {
            get { return _ReceiveDate; }
            set { _ReceiveDate = value; }
        }

        private int MailCount
        {
            get { return _MailCount; }
            set { _MailCount = value; }
        }
    }
}
