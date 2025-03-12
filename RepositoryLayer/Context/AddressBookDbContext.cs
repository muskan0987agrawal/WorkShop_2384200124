using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class AddressBookDbContext:DbContext
    {
        public AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) : base(options) { }

        // Add DbSet for each entity
        public DbSet<AddressBookEntity> AddressBooks { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        
            // Additional configurations if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBookEntity>()
                .HasOne(a => a.User)         // AddressBook has one User
                .WithMany()                  // One User can have many AddressBooks
                .HasForeignKey(a => a.UserId) // UserId is the Foreign Key
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
