using AutoMapper;
using BusinessLayer.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Linq;

namespace AddressBook.Controllers
{
    [ApiController]
    [Route("api/addressbook")]
    public class AddressBookController : ControllerBase
    {
        private static List<AddressBookEntity> _contacts = new List<AddressBookEntity>();
        private static int _nextId = 1;
        private readonly IMapper _mapper;
        private readonly IValidator<RequestAddressBook> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookController"/> class.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="validator">The FluentValidation instance.</param>
        public AddressBookController(IMapper mapper, IValidator<RequestAddressBook> validator)
        {
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves all contacts from the address book.
        /// </summary>
        /// <returns>A list of contacts.</returns>
        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<ResponseAddressBook>>> GetAllContacts()
        {
            var response = _mapper.Map<IEnumerable<ResponseAddressBook>>(_contacts);
            return Ok(new ResponseBody<IEnumerable<ResponseAddressBook>>
            {
                Success = true,
                Message = "Contacts retrieved successfully",
                Data = response
            });
        }

        /// <summary>
        /// Retrieves a contact by the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the contact.</param>
        /// <returns>The contact details if found.</returns>
        [HttpGet("get/{id}")]
        public ActionResult<ResponseBody<ResponseAddressBook>> GetContactById(int id)
        {
            var contact = _contacts.Find(c => c.Id == id);
            if (contact == null)
                return NotFound(new ResponseBody<ResponseAddressBook>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found"
                });

            var response = _mapper.Map<ResponseAddressBook>(contact);
            return Ok(new ResponseBody<ResponseAddressBook>
            {
                Success = true,
                Message = "Contact retrieved successfully",
                Data = response
            });
        }

        /// <summary>
        /// Adds a new contact to the address book.
        /// </summary>
        /// <param name="request">The contact details to be added.</param>
        /// <returns>The created contact with its assigned ID.</returns>
        [HttpPost("add")]
        public ActionResult<ResponseBody<ResponseAddressBook>> AddContact([FromBody] RequestAddressBook request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(new ResponseBody<ResponseAddressBook>
                {
                    Success = false,
                    Message = "Validation failed",
                    Data = null
                });

            var newContact = _mapper.Map<AddressBookEntity>(request);
            newContact.Id = _nextId++;

            _contacts.Add(newContact);

            var response = _mapper.Map<ResponseAddressBook>(newContact);
            return CreatedAtAction(nameof(GetContactById), new { id = response.Id }, new ResponseBody<ResponseAddressBook>
            {
                Success = true,
                Message = "Contact added successfully",
                Data = response
            });
        }

        /// <summary>
        /// Updates an existing contact in the address book.
        /// </summary>
        /// <param name="id">The ID of the contact to update.</param>
        /// <param name="request">The updated contact details.</param>
        /// <returns>The updated contact details.</returns>
        [HttpPut("update/{id}")]
        public ActionResult<ResponseBody<ResponseAddressBook>> UpdateContact(int id, [FromBody] RequestAddressBook request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                return BadRequest(new ResponseBody<ResponseAddressBook>
                {
                    Success = false,
                    Message = "Validation failed"
                });

            var contact = _contacts.Find(c => c.Id == id);
            if (contact == null)
                return NotFound(new ResponseBody<ResponseAddressBook>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found"
                });

            _mapper.Map(request, contact);
            var response = _mapper.Map<ResponseAddressBook>(contact);

            return Ok(new ResponseBody<ResponseAddressBook>
            {
                Success = true,
                Message = "Contact updated successfully",
                Data = response
            });
        }

        /// <summary>
        /// Deletes a contact by the specified ID.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to delete.</param>
        /// <returns>A success message if the deletion is successful.</returns>
        [HttpDelete("delete/{id}")]
        public ActionResult<ResponseBody<string>> DeleteContact(int id)
        {
            var contact = _contacts.Find(c => c.Id == id);
            if (contact == null)
                return NotFound(new ResponseBody<string>
                {
                    Success = false,
                    Message = $"Contact with ID {id} not found"
                });

            _contacts.Remove(contact);
            return Ok(new ResponseBody<string>
            {
                Success = true,
                Message = "Contact deleted successfully",
                Data = $"Deleted contact ID: {id}"
            });
        }
    }
}
