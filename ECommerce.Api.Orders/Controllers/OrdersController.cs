using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }

        [HttpGet("{customerId}")]
       public async Task<IActionResult> GetOrdersAsync(int customerId)
        {
            var restult = await ordersProvider.GetOrdersAsync(customerId);
            if(restult.IsSuccess)
            {
                return Ok(restult.orders);
            }
            return NotFound();
        }
    }
}
