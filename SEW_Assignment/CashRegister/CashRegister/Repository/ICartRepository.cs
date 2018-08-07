using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashRegister.Model;

namespace CashRegister.Repository
{
    public interface ICartRepository
    {
        Task<CartByCustomer> GetCartsByCustomerAsync(long customerId);
        Task<Cart> GetCartsByCustomerItemAsync(long? customerId, long? itemID);
        Task<Cart> GetCartAsync(long id);        
        Task<Cart> AddCartAsync(Cart cart);
        Task<Cart> UpdateCartAsync(Cart cart);
        Task<Order> CreateOrderAsync(long customerId, decimal totalCost, decimal totalDiscount, List<Cart> cartItems);


    }
}
