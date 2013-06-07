using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.Model;

namespace HXMail.IBLL
{
    public interface IContactManage
    {
        int CreateContact(ContactInfo Contact);
        int ModifyContact(ContactInfo Contact);
    }
}
