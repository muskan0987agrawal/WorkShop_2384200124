using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

namespace BusinessLayer.Service
{
    public class ContactBL:IContactBL
    {
        private readonly IContactRL contactRL;
        private readonly int userId;
        public ContactBL(IContactRL contactRL)
        {
            this.contactRL = contactRL;
        }

        public ResponseModel<ContactEntity> AddContact(CreateContactRequestModel contact)
        {
            return contactRL.AddContact(contact);
        }

        public List<ContactEntity> FetchAllContact()
        {
            return contactRL.FetchAllContact(userId);
        }

        public ResponseModel<ContactEntity> FetchContactById(int id)
        {
            return contactRL.FetchContactById(id);
        }

        public ResponseModel<ContactEntity> DeleteContactById(int id)
        {
            return contactRL.DeleteContactById(id);
        }

        public ResponseModel<ContactEntity> UpdateContactById(int id, UpdateContactRequestModel contact)
        {
            return contactRL.UpdateContactById(id, contact);
        }
    }
}
