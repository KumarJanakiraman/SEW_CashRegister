using CashRegister.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ILogger<ItemRepository> _logger;
        private CashRegisterContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        public CartRepository(ILoggerFactory loggerFactory, CashRegisterContext context,IOrderRepository orderRepository,IItemRepository itemRepository)
        {
            _logger = loggerFactory.CreateLogger<ItemRepository>();
            _context = context;
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
        }


        public async Task<CartByCustomer> GetCartsByCustomerAsync(long customerId)
        {

            var data = new CartByCustomer();
            data.CartItems = await _context.Carts.Where(x=>x.CustID == customerId).ToListAsync();
            return data;

        }

        public async Task<Order> CreateOrderAsync(long customerId, decimal totalCost, decimal totalDiscount, List<Cart> cartItems)
        {
            var orderMaster = new OrderMaster()
            {
                CustID = customerId,
                TotalCost = totalCost,
                TotalDiscount = totalDiscount

            };
            var order = _context.OrderMasters.Add(orderMaster);
            await _context.SaveChangesAsync();
            foreach (Cart cart in cartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderMasterID = order.Entity.OrderID,
                    ItemID = cart.ItemID,
                    Qty = cart.Qty,
                    Discount = cart.Discount,
                    DiscountName = cart.DiscountName,
                    Price = cart.Price,
                    TotalCost = cart.TotalCost
                };
                 _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();

                var item = await _itemRepository.GetItemMasterAsync(cart.ItemID);
                item.ItemQty = item.ItemQty - (cart.Qty??0);
                await _itemRepository.UpdateItemMasterAsync(item);               

            }



            //await _context.SaveChangesAsync();
            _logger.LogInformation("item added succesfully.");

            return await _orderRepository.GetOrderAsync(order.Entity.OrderID);


        }


        public async Task<Cart> GetCartsByCustomerItemAsync(long ?customerId,long ?itemID)
        {
            var _cart = await _context.Carts.SingleOrDefaultAsync(x => x.CustID == customerId && x.ItemID == itemID);           
            return _cart;
        }

        public async Task<Cart> GetCartAsync(long id)
        {
            return await _context.Carts.SingleOrDefaultAsync(x => x.CartID == id);
        }


        public async Task<Cart> AddCartAsync(Cart Cart)
        {
            var data = _context.Carts.Add(Cart);
            await _context.SaveChangesAsync();
            _logger.LogInformation("item added succesfully.");
            return await GetCartAsync(data.Entity.CartID);
        }

        public async Task<Cart> UpdateCartAsync(Cart Cart)
        {
            var _cart = await _context.Carts.SingleOrDefaultAsync(x => x.CustID == Cart.CustID && x.ItemID == Cart.ItemID);
            long cartId;

            _cart.Qty = _cart.Qty + Cart.Qty; //Adding Existing Item Qty + Requested Qty
            var data = _context.Carts.Update(_cart);
            cartId = data.Entity.CartID;


            await _context.SaveChangesAsync();
            _logger.LogInformation("item added succesfully.");
            return await GetCartAsync(cartId);
        }

      
    }
}
