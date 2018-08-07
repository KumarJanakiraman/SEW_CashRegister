using CashRegister.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly ILogger<ItemRepository> _logger;
        private CashRegisterContext _context;

        public ItemRepository(ILoggerFactory loggerFactory, CashRegisterContext context)
        {
            _logger = loggerFactory.CreateLogger<ItemRepository>();
            _context = context;
        }


        public async Task<List<Item>> GetItemsAsync()
        {
            var data = await _context.Items.ToListAsync();
            return data;

        }

        public async Task<Item> GetItemMasterAsync(long ?id)
        {
            return await _context.Items.SingleOrDefaultAsync(x => x.ItemID == id);
        }

        public async Task<Item> GetItemByNameAsync(string itemName)
        {
            return await _context.Items.SingleOrDefaultAsync(x => x.ItemName.Equals(itemName));
        }


        public async Task<Item> AddItemMasterAsync(Item itemMaster)
        {           

            var data = _context.Items.Add(itemMaster);
            await _context.SaveChangesAsync();
            _logger.LogInformation("item added succesfully.");
            return await GetItemMasterAsync(data.Entity.ItemID);
        }

        public async Task<Item> UpdateItemMasterAsync(Item itemMaster)
        {
            var item = _context.Items.SingleOrDefault(x => x.ItemID == itemMaster.ItemID);
            item.ItemName = itemMaster.ItemName;
            item.ItemDescription = itemMaster.ItemDescription;
            item.ItemQty = itemMaster.ItemQty;
            item.ItemWeight = itemMaster.ItemWeight;
            item.BasePrice = itemMaster.BasePrice;        

            var data =  _context.Items.Update(item);
            await _context.SaveChangesAsync();
            _logger.LogInformation("item updated succesfully.");
            return await GetItemMasterAsync(data.Entity.ItemID);
        }

    
    }
}
