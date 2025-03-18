using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class MyAddressBookDbContext : DbContext
    {
        public MyAddressBookDbContext(DbContextOptions<MyAddressBookDbContext> options) : base(options) { }

        // Add DbSet for each entity
        public DbSet<ContactEntity> Contact { get; set; }
        public DbSet<UserEntity> Users { get; set; }

      
    }
}

