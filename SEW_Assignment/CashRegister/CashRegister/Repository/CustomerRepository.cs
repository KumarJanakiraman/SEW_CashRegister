using CashRegister.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<ItemRepository> _logger;
        private CashRegisterContext _context;

        public CustomerRepository(ILoggerFactory loggerFactory, CashRegisterContext context)
        {
            _logger = loggerFactory.CreateLogger<ItemRepository>();
            _context = context;
        }


        public async Task<List<Customer>> GetCustomersAsync()
        {
            var data = await _context.Customers.ToListAsync();
            return data;

        }

        public async Task<Customer> GetCustomerAsync(long id)
        {
            return await _context.Customers.SingleOrDefaultAsync(x => x.CustID == id);
        }

       


        public async Task<Customer> AddCustomerAsync(Customer Customer)
        {
            var data = _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("item added succesfully.");
            return await GetCustomerAsync(data.Entity.CustID);
        }

        public async Task<Customer> UpdateCustomerAsync(Customer Customer)
        {
            var _Customer = _context.Customers.SingleOrDefault(x => x.CustID == Customer.CustID);
            //_Customer.BuyQty = Customer.BuyQty;
            //_Customer.CustomerDescription = Customer.CustomerDescription;
            //_Customer.CustomerPercentage = Customer.CustomerPercentage;
            //_Customer.EffectiveDateFrom = Customer.EffectiveDateFrom;
            //_Customer.EffectiveDateTo = Customer.EffectiveDateTo;
            //_Customer.FreeItemID = Customer.FreeItemID;
            //_Customer.ItemID = Customer.ItemID;

            var data =  _context.Customers.Update(_Customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Customer updated succesfully.");
            return await GetCustomerAsync(data.Entity.CustID);
        }

      
    }
}
