using AutoMapper;
using BusinessLayer.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using System.Collections.Generic;
using System.Linq;

namespace AddressBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;
        private readonly IValidator<RequestAddressBook> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressBookController"/> class.
        /// </summary>
        /// <param name="addressBookBL">Business logic layer interface.</param>
        /// <param name="validator">Fluent validation for request model.</param>
        public AddressBookController(IAddressBookBL addressBookBL, IValidator<RequestAddressBook> validator)
        {
            _addressBookBL = addressBookBL;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves all contacts.
        /// </summary>
        /// <returns>List of contacts.</returns>
        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<ResponseAddressBook>>> GetAllContacts()
        {
            var contacts = _addressBookBL.GetAllContacts();
            return Ok(new ResponseBody<IEnumerable<ResponseAddressBook>>
            {
                Success = true,
                Message = "Contacts retrieved successfully.",
                Data = contacts
            });
        }

        /// <summary>
        /// Retrieves a contact by ID.
        /// </summary>
        /// <param name="id">Contact ID.</param>
        /// <returns>Contact details.</returns>
        [HttpGet("{id}")]
        public ActionResult<ResponseBody<ResponseAddressBook>> GetContactById(int id)
        {
            var contact = _addressBookBL.GetContactById(id);
            if (contact == null)
            {
                return NotFound(new ResponseBody<ResponseAddressBook>
                {
                    Success = false,
                    Message = "Contact not found.",
                    Data = null
                });
            }

            return Ok(new ResponseBody<ResponseAddressBook>
            {
                Success = true,
                Message = "Contact retrieved successfully.",
                Data = contact
            });
        }

        /// <summary>
        /// Adds a new contact.
        /// </summary>
        /// <param name="dto">Contact request data.</param>
        /// <returns>Created contact details.</returns>
        [HttpPost]
        public ActionResult<ResponseBody<ResponseAddressBook>> AddContact([FromBody] RequestAddressBook dto)
        {
            if (dto == null)
            {
                return BadRequest(new ResponseBody<object>
                {
                    Success = false,
                    Message = "Request body cannot be null."
                });
            }

            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseBody<object>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var newContact = _addressBookBL.AddContact(dto);
            if (newContact == null)
            {
                return StatusCode(500, new ResponseBody<ResponseAddressBook>
                {
                    Success = false,
                    Message = "Failed to add contact. Please try again."
                });
            }

            return CreatedAtAction(nameof(GetContactById), new { id = newContact.Id }, new ResponseBody<ResponseAddressBook>
            {
                Success = true,
                Message = "Contact added successfully.",
                Data = newContact
            });
        }

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        /// <param name="id">Contact ID.</param>
        /// <param name="dto">Updated contact data.</param>
        /// <returns>Updated contact details.</returns>
        [HttpPut("{id}")]
        public ActionResult<ResponseBody<ResponseAddressBook>> UpdateContact(int id, [FromBody] RequestAddressBook dto)
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ResponseBody<object>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Data = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var updatedContact = _addressBookBL.UpdateContact(id, dto);
            if (updatedContact == null)
            {
                return NotFound(new ResponseBody<ResponseAddressBook>
                {
                    Success = false,
                    Message = "Contact not found.",
                    Data = null
                });
            }

            return Ok(new ResponseBody<ResponseAddressBook>
            {
                Success = true,
                Message = "Contact updated successfully.",
                Data = updatedContact
            });
        }

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <param name="id">Contact ID.</param>
        /// <returns>Deletion status.</returns>
        [HttpDelete("{id}")]
        public ActionResult<ResponseBody<string>> DeleteContact(int id)
        {
            var isDeleted = _addressBookBL.DeleteContact(id);
            if (!isDeleted)
            {
                return NotFound(new ResponseBody<string>
                {
                    Success = false,
                    Message = "Contact not found.",
                    Data = null
                });
            }

            return Ok(new ResponseBody<string>
            {
                Success = true,
                Message = "Contact deleted successfully.",
                Data = "Deleted"
            });
        }
    }
}
