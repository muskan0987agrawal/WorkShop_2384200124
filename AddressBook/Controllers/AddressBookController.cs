using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace AddressBook.Controllers
{
    /// <summary>
    /// Controller for managing address book contacts.
    /// Provides endpoints to add, update, retrieve, and delete contacts.
    /// </summary>
    [ApiController]
    [Route("api/addressbook")]
    public class AddressBookController : ControllerBase
    {
        private static List<ResponseAddressBook> _contacts = new List<ResponseAddressBook>();
        private static int _nextId = 1;

        /// <summary>
        /// Retrieves all contacts from the address book.
        /// </summary>
        /// <returns>A list of contacts or a NotFound response if no contacts exist.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<ResponseAddressBook>> GetAllContacts()
        {
            if (_contacts.Count == 0)
                return NotFound(new { message = "No contacts found" });

            return Ok(new { message = "Contacts retrieved successfully", data = _contacts });
        }

        /// <summary>
        /// Retrieves a contact by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the contact.</param>
        /// <returns>The contact details or a NotFound response if not found.</returns>
        [HttpGet("get/{id}")]
        public ActionResult<ResponseAddressBook> GetContactById(int id)
        {
            var contact = _contacts.Find(c => c.Id == id);
            if (contact == null)
                return NotFound(new { message = $"Contact with ID {id} not found" });

            return Ok(new { message = "Contact retrieved successfully", data = contact });
        }

        /// <summary>
        /// Adds a new contact to the address book.
        /// </summary>
        /// <param name="request">The contact details to be added.</param>
        /// <returns>The created contact with its assigned ID.</returns>
        [HttpPost("add")]
        public ActionResult<ResponseAddressBook> AddContact([FromBody] RequestAddressBook request)
        {
            if (request == null)
                return BadRequest(new { message = "Invalid request data" });

            var newContact = new ResponseAddressBook
            {
                Id = _nextId++,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Address = request.Address
            };

            _contacts.Add(newContact);
            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id },
                new { message = "Contact added successfully", data = newContact });
        }

        /// <summary>
        /// Updates an existing contact in the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="request">The updated contact details.</param>
        /// <returns>The updated contact details or a NotFound response if the contact does not exist.</returns>
        [HttpPut("update/{id}")]
        public ActionResult<ResponseAddressBook> UpdateContact(int id, [FromBody] RequestAddressBook request)
        {
            var contact = _contacts.Find(c => c.Id == id);
            if (contact == null)
                return NotFound(new { message = $"Contact with ID {id} not found" });

            contact.Name = request.Name;
            contact.PhoneNumber = request.PhoneNumber;
            contact.Email = request.Email;
            contact.Address = request.Address;

            return Ok(new { message = "Contact updated successfully", data = contact });
        }

        /// <summary>
        /// Deletes a contact from the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to delete.</param>
        /// <returns>A success message or a NotFound response if the contact does not exist.</returns>
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteContact(int id)
        {
            var contact = _contacts.Find(c => c.Id == id);
            if (contact == null)
                return NotFound(new { message = $"Contact with ID {id} not found" });

            _contacts.Remove(contact);
            return Ok(new { message = "Contact deleted successfully" });
        }
    }
}
