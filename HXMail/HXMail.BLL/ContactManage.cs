using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HXMail.IBLL;
using HXMail.DAL;
using HXMail.Model;

namespace HXMail.BLL
{
    public class ContactManage : IContactManage
    {
        private ContactService contactManage = ContactService.GetInstance();

        public int CreateContact(ContactInfo Contact)
        {
            return contactManage.Insert(Contact);
        }

        public int ModifyContact(ContactInfo Contact)
        {
            return contactManage.UpDate(Contact);
        }
    }
}
