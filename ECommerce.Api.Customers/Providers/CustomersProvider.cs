using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext customersDbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext customersDbContext ,
            ILogger<CustomersProvider> logger,IMapper mapper)
        {
            this.customersDbContext = customersDbContext;
            this.logger = logger;
            this.mapper = mapper;
            seedData();
        }

        private void seedData()
        {
           if(!customersDbContext.Customers.Any())
            {
                customersDbContext.Customers.Add(new Db.Customer { Id = 101, Name = "Shebin", Address = "Address123" });
                customersDbContext.Customers.Add(new Db.Customer { Id = 102, Name = "Emmanuel", Address = "Address456" });
                customersDbContext.Customers.Add(new Db.Customer { Id = 103, Name = "Joseph", Address = "Address789" });
                customersDbContext.Customers.Add(new Db.Customer { Id = 104, Name = "John", Address = "Address101112" });

                customersDbContext.SaveChanges();
            }
        }

       public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await customersDbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
            

        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomersAsync(int id)
        {
            try
            {
                var customer = await customersDbContext.Customers.FirstOrDefaultAsync(x=>x.Id==id) ;
                if (customer != null )
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
        }

       
    }
}
