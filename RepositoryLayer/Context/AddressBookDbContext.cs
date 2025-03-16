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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBookEntity>()
                .HasOne(ab => ab.User)
                .WithMany(u => u.AddressBookEntries)
                .HasForeignKey(ab => ab.UserId);
        }
    }
}

  