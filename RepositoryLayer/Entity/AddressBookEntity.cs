using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class AddressBookEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        // Foreign Key Relationship
        public int UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }  // Navigation Property
    }

}
