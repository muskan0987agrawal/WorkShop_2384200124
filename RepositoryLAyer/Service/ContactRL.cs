using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
   public class ContactRL:IContactRL
    {
        private readonly MyAddressBookDbContext addressBookContext;
        public ContactRL(MyAddressBookDbContext _addressBookContext)
        {
            addressBookContext = _addressBookContext;
        }

        // Add a new Contact
        public ResponseModel<ContactEntity> AddContact(CreateContactRequestModel contact)
        {

            var existingContact = addressBookContext.Contact.FirstOrDefault(g => g.PhoneNumber == contact.PhoneNumber);
            if (existingContact != null) return null;
           // var owner = addressBookContext.Users.FirstOrDefault(g => g.UserId == userId);

            var newContact = new ContactEntity
            {
                Name = contact.Name,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                OwnerId = 2,
                //Owner = owner!
            };
            addressBookContext.Contact.Add(newContact);
            addressBookContext.SaveChanges();
            return new ResponseModel<ContactEntity>
            {
                Data = newContact,
                Success = true,
                Message = "Contact Added Successfully.",
                StatusCode = 200 // OK
            };
        }

        // Fetch all contact
        public List<ContactEntity> FetchAllContact(int userId)
        {
            return addressBookContext.Contact.ToList();
        }

        //Fetch Contact by Id
        public ResponseModel<ContactEntity> FetchContactById(int id)
        {
            var contact = addressBookContext.Contact.FirstOrDefault(c => c.Id == id );

            if (contact == null)
            {
                return new ResponseModel<ContactEntity>
                {
                    Success = false,
                    Message = "Contact not found",
                    Data = null
                };
            }

            return new ResponseModel<ContactEntity>
            {
                Success = true,
                Message = "Contact fetched successfully",
                Data = contact
            };
        }


        //Delete Contact by Id
        public ResponseModel<ContactEntity> DeleteContactById(int id)
        {
            var contact = addressBookContext.Contact.FirstOrDefault(c => c.Id == id );

            if (contact == null)
            {
                return new ResponseModel<ContactEntity>
                {
                    Success = false,
                    Message = "Contact not found",
                    Data = null
                };
            }

            addressBookContext.Contact.Remove(contact);
            addressBookContext.SaveChanges();

            return new ResponseModel<ContactEntity>
            {
                Success = true,
                Message = "Contact Deleted successfully",
                Data = contact
            };
        }

        public ResponseModel<ContactEntity> UpdateContactById(int id, UpdateContactRequestModel contact)
        {
            var updatedContact = addressBookContext.Contact.FirstOrDefault(c => c.Id == id );
            if (updatedContact == null)
            {
                return new ResponseModel<ContactEntity>
                {
                    Success = false,
                    Message = "Contact not found",
                    Data = null
                };
            }

            if (contact.Name != null) updatedContact.Name = contact.Name;
            if (contact.Email != null) updatedContact.Email = contact.Email;
            if (contact.PhoneNumber != null) updatedContact.PhoneNumber = contact.PhoneNumber;
            addressBookContext.SaveChanges();




            return new ResponseModel<ContactEntity>
            {
                Success = true,
                Message = "Contact Updated successfully",
                Data = updatedContact
            };
        }
    }
}
