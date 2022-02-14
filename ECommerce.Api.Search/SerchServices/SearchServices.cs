using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.SerchServices
{
    public class SearchServices : ISearchServices
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomerService customerService;

        public SearchServices(IOrdersService ordersService,
                              IProductsService productsService,
                              ICustomerService customerService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerID)
        {
            var orderResult = await ordersService.GetOrdersAsync(customerID);
            var productResult = await productsService.GetProcutsAsync();
            var customerResult = await customerService.GetCustomerAsync(customerID);
            if (orderResult.IsSuccess)
            {
                foreach(var order in orderResult.orders)
                {  
                    foreach (var items in order.Items)
                    {
                        items.ProductName = productResult.IsSuccess? productResult.products.FirstOrDefault(x => x.Id == items.ProductId)?.Name.ToString()
                            :"Product information is not available";

                    }
                }
                var result = new
                {
                    Customer = customerResult.IsSuccess ?
                               customerResult.customer : 
                               new Modles.Customer { Name = "Could not found the name", Address = "" }
                    Orders = orderResult.orders,
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
