using AspnetBasics.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspnetBasics.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName);
    }
}
