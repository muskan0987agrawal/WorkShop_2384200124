﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
   public interface IAddressBookBL
    {
        IEnumerable<ResponseAddressBook> GetAllContacts();
        ResponseAddressBook GetContactById(int id);
        ResponseAddressBook AddContact(RequestAddressBook contact);
        ResponseAddressBook UpdateContact(int id, RequestAddressBook contact);
        bool DeleteContact(int id);
    }
}
