using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext ordersDbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext ordersDbContext,
            ILogger<OrdersProvider> logger ,IMapper mapper)
        {
            this.ordersDbContext = ordersDbContext;
            this.logger = logger;
            this.mapper = mapper;
            
            seedData();
        }
 
        public async Task<(bool IsSuccess, IEnumerable<Models.Order> orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await ordersDbContext.Orders.Where(x => x.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
           
        }

        private void seedData()
        {
            List<Db.OrderItem> orderItems1 = new List<Db.OrderItem>();
            orderItems1.Add(new Db.OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 3, UnitPrice = 100, });
            orderItems1.Add(new Db.OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 10, });

            ordersDbContext.Orders.Add(new Db.Order
            {
                Id = 1,
                CustomerId = 101,
                OrderDate = DateTime.Now,
                Total = 310,
                Items =  orderItems1
                        
            }); ;

            List<Db.OrderItem> orderItems2 = new List<Db.OrderItem>();
            orderItems2.Add(new Db.OrderItem { Id = 3, OrderId = 2, ProductId = 2, Quantity = 3, UnitPrice = 10, });
            orderItems2.Add(new Db.OrderItem { Id = 4, OrderId = 2, ProductId = 3, Quantity = 1, UnitPrice = 200, });

            ordersDbContext.Orders.Add(new Db.Order
            {
                Id = 2,
                CustomerId = 102,
                OrderDate = DateTime.Now,
                Total = 230,
                Items = orderItems2
            });

            List<Db.OrderItem> orderItems3 = new List<Db.OrderItem>();
            orderItems3.Add(new Db.OrderItem { Id = 5, OrderId = 3, ProductId = 1, Quantity = 3, UnitPrice = 100, });
            orderItems3.Add(new Db.OrderItem { Id = 6, OrderId = 3, ProductId = 4, Quantity = 1, UnitPrice = 200, });

            ordersDbContext.Orders.Add(new Db.Order
            {
                Id = 3,
                CustomerId = 101,
                OrderDate = DateTime.Now,
                Total = 900,
                Items = orderItems3
            });

            List<Db.OrderItem> orderItems4 = new List<Db.OrderItem>();
            orderItems4.Add(new Db.OrderItem { Id = 7, OrderId = 4, ProductId = 3, Quantity = 1, UnitPrice = 150, });
            orderItems4.Add(new Db.OrderItem { Id = 8, OrderId = 4, ProductId = 4, Quantity = 1, UnitPrice = 200, });

            ordersDbContext.Orders.Add(new Db.Order
            {
                Id = 4,
                CustomerId = 103,
                OrderDate = DateTime.Now,
                Total = 350,
                Items = orderItems4
            });

            List<Db.OrderItem> orderItems5 = new List<Db.OrderItem>();
            orderItems5.Add(new Db.OrderItem { Id = 9, OrderId = 4, ProductId = 1, Quantity = 10, UnitPrice = 100, });
            orderItems5.Add(new Db.OrderItem { Id = 10, OrderId = 4, ProductId = 3, Quantity = 1, UnitPrice = 150, });

            ordersDbContext.Orders.Add(new Db.Order
            {
                Id = 5,
                CustomerId = 103,
                OrderDate = DateTime.Now,
                Total = 1150,
                Items = orderItems5
            });

            ordersDbContext.SaveChanges();
        }

    }
}
