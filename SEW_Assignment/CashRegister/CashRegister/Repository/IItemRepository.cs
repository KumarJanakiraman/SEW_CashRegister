using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashRegister.Model;

namespace CashRegister.Repository
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsAsync();
        Task<Item> GetItemMasterAsync(long ?id);
        Task<Item> GetItemByNameAsync(string itemName);
        Task<Item> AddItemMasterAsync(Item itemMaster);
        Task<Item> UpdateItemMasterAsync(Item itemMaster);
        
    }
}
