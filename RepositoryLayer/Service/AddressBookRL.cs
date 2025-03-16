using System;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
   public class AddressBookRL: IAddressBookRL
    {
        private readonly AddressBookDbContext _dbContext;

        public AddressBookRL(AddressBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<AddressBookEntity> GetAllContacts()
        {
            return _dbContext.AddressBooks.ToList();
        }

        public AddressBookEntity GetContactById(int id)
        {
            return _dbContext.AddressBooks.Find(id);
        }

        public AddressBookEntity AddContact(AddressBookEntity contact)
        {
            _dbContext.AddressBooks.Add(contact);
            _dbContext.SaveChanges();
            return contact;
        }

        public AddressBookEntity UpdateContact(int id, AddressBookEntity contact)
        {
            var existingContact = _dbContext.AddressBooks.FirstOrDefault(c => c.Id == id);

            if (existingContact == null)
            {
                return null;
            }

            // Update fields
            existingContact.Name = contact.Name;
            existingContact.PhoneNumber = contact.PhoneNumber;
            existingContact.Email = contact.Email;
            existingContact.Address = contact.Address;

            _dbContext.AddressBooks.Update(existingContact);
            _dbContext.SaveChanges();
            return existingContact;
        }

        public bool DeleteContact(int id)
        {
            var entry = _dbContext.AddressBooks.Find(id);
            if (entry == null) return false;

            _dbContext.AddressBooks.Remove(entry);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
