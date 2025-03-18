
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class ContactEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; }

        [Required, Phone, MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [EmailAddress, MaxLength(255)]
        public string? Email { get; set; }

        [Required]
        public int OwnerId { get; set; }

        // Navigation property to establish the relationship
        [ForeignKey("OwnerId")]
        public  UserEntity Owner { get; set; }
    }
}
