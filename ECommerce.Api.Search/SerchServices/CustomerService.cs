﻿using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Modles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.SerchServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<CustomerService> logger;

        public CustomerService(IHttpClientFactory httpClientFactory, ILogger<CustomerService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        public async Task<(bool IsSuccess, Customer customer, string ErrorMessage)> GetCustomerAsync(int customerId)
        {
            try
            {
                var client = httpClientFactory.CreateClient("CustomersService");
                var response = await client.GetAsync($"api/customers/{customerId}");
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<Customer>(content, options);
                    return (true, result, null);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.ToString());
            }
            
        }
    }
}
