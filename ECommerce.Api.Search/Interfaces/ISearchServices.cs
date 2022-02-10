using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Interfaces
{
   public interface ISearchServices
    {
       Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerID);
    }
}
