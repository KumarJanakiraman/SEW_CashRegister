using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashRegister.Model;

namespace CashRegister.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderAsync(long orderID);
    }
}
