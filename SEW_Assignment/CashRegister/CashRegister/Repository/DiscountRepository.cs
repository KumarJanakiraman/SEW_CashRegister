using CashRegister.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ILogger<ItemRepository> _logger;
        private CashRegisterContext _context;

        public DiscountRepository(ILoggerFactory loggerFactory, CashRegisterContext context)
        {
            _logger = loggerFactory.CreateLogger<ItemRepository>();
            _context = context;
        }


        public async Task<List<Discount>> GetDiscountsAsync()
        {
            var data = await _context.Discounts.ToListAsync();
            return data;

        }

        public async Task<Discount> GetDiscountAsync(long id)
        {
            return await _context.Discounts.SingleOrDefaultAsync(x => x.DiscountID == id);
        }

        public async Task<Discount> GetDiscountByItemEffDateAsync(long? itemID, DateTime? effectiveDateFrom, DateTime? effectiveDateTo)
        {
            return await _context.Discounts.SingleOrDefaultAsync(x => x.ItemID == itemID &&
                            ((x.EffectiveDateFrom >= effectiveDateFrom && x.EffectiveDateFrom <= effectiveDateTo)
                            || (x.EffectiveDateTo >= effectiveDateFrom && x.EffectiveDateTo <= effectiveDateTo)));
        }

        public async Task<Discount> GetDiscountByItemAsync(long? itemID)
        {
            return await _context.Discounts.SingleOrDefaultAsync(x => x.ItemID == itemID &&
                                                                x.EffectiveDateFrom <= DateTime.Now && 
                                                                x.EffectiveDateTo >= DateTime.Now
                                                                );
        }
        public async Task<Discount> AddDiscountAsync(Discount discount)
        {
            
            var data = _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
            _logger.LogInformation("item added succesfully.");
            return await GetDiscountAsync(data.Entity.DiscountID);
        }

        public async Task<Discount> UpdateDiscountAsync(Discount discount)
        {
            var _discount = _context.Discounts.SingleOrDefault(x => x.DiscountID == discount.DiscountID);
            _discount.BuyQty = discount.BuyQty;
            _discount.DiscountDescription = discount.DiscountDescription;
            _discount.DiscountPercentage = discount.DiscountPercentage;
            _discount.EffectiveDateFrom = discount.EffectiveDateFrom;
            _discount.EffectiveDateTo = discount.EffectiveDateTo;
            _discount.FreeItemID = discount.FreeItemID;
            _discount.ItemID = discount.ItemID;

            var data =  _context.Discounts.Update(_discount);
            await _context.SaveChangesAsync();
            _logger.LogInformation("discount updated succesfully.");
            return await GetDiscountAsync(data.Entity.DiscountID);
        }

      
    }
}
