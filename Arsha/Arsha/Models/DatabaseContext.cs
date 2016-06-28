using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Arsha.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("SqlServerConnection") { }

        public DbSet<User> Users { get; set; }
    }
}