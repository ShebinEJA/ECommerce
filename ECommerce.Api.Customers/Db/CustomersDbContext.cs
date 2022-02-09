using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Db
{
    public class CustomersDbContext : DbContext
    {
        public CustomersDbContext(DbContextOptions options ) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
    }
}
