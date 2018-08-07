using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashRegister.Model;

namespace CashRegister.Repository
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetDiscountsAsync();
        Task<Discount> GetDiscountAsync(long id);
        Task<Discount> GetDiscountByItemAsync(long? itemID);
        Task<Discount> GetDiscountByItemEffDateAsync(long ?itemID, DateTime ?effectiveDateFrom, DateTime ?effectiveDateTo);
        Task<Discount> AddDiscountAsync(Discount discount);
        Task<Discount> UpdateDiscountAsync(Discount discount);
        
    }
}
