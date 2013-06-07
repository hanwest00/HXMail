using System;
using System.Collections.Generic;
using System.Text;

namespace HXMail.Model
{
    public class ContactInfo
    {
        private Int64 _Id;
        private int _UserId;
        private string _Name;
        private string _Address;

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

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} <{1}>",this.Name,this.Address);
        }
    }
}
