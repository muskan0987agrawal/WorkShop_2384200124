using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class ContactBL : IContactBL
    {
        private readonly IContactRL contactRL;
        private readonly int userId;

        public ContactBL(IContactRL contactRL)
        {
            this.contactRL = contactRL;
        }

        public ResponseModel<ContactEntity> AddContact(CreateContactRequestModel contact)
        {
            // Validate Contact Fields Using Regex
            string namePattern = @"^[A-Za-z\s]{3,50}$";
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            string phonePattern = @"^\+?[0-9]{10,15}$";

            if (!Regex.IsMatch(contact.Name, namePattern))
            {
                return new ResponseModel<ContactEntity> { Success = false, Message = "Invalid Name. It should only contain letters and be 3 to 50 characters long." };
            }

            if (!Regex.IsMatch(contact.Email, emailPattern))
            {
                return new ResponseModel<ContactEntity> { Success = false, Message = "Invalid Email format." };
            }

            if (!Regex.IsMatch(contact.PhoneNumber, phonePattern))
            {
                return new ResponseModel<ContactEntity> { Success = false, Message = "Invalid Phone Number. It should contain only digits and be 10 to 15 characters long." };
            }

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
            // Validate Contact Fields Using Regex
            string namePattern = @"^[A-Za-z\s]{3,50}$";
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            string phonePattern = @"^\+?[0-9]{10,15}$";

            if (!Regex.IsMatch(contact.Name, namePattern))
            {
                return new ResponseModel<ContactEntity> { Success = false, Message = "Invalid Name. It should only contain letters and be 3 to 50 characters long." };
            }

            if (!Regex.IsMatch(contact.Email, emailPattern))
            {
                return new ResponseModel<ContactEntity> { Success = false, Message = "Invalid Email format." };
            }

            if (!Regex.IsMatch(contact.PhoneNumber, phonePattern))
            {
                return new ResponseModel<ContactEntity> { Success = false, Message = "Invalid Phone Number. It should contain only digits and be 10 to 15 characters long." };
            }

            return contactRL.UpdateContactById(id, contact);
        }
    }
}
