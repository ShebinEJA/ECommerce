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

        public SearchServices(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerID)
        {
            var orderResult = await ordersService.GetOrdersAsync(customerID);
            if (orderResult.IsSuccess)
            {
                var result = new
                {
                    Orders = orderResult.orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
