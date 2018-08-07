using CashRegister.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILogger<ItemRepository> _logger;
        private CashRegisterContext _context;

        public OrderRepository(ILoggerFactory loggerFactory, CashRegisterContext context)
        {
            _logger = loggerFactory.CreateLogger<ItemRepository>();
            _context = context;
        }


        public async Task<Order> GetOrderAsync(long orderID)
        {
            var data = new Order();

            var orderMaster=   await _context.OrderMasters.SingleOrDefaultAsync(x => x.OrderID == orderID);
            data.OrderID = orderID;
            data.CustID = orderMaster.CustID;
            data.TotalCost = orderMaster.TotalCost;
            data.TotalDiscount = orderMaster.TotalDiscount;
            var orderDetails= await _context.OrderDetails.Where(x=>x.OrderMasterID==orderID).ToListAsync();
            data.OrderItems = orderDetails;

            return data;

        }
    }
}
