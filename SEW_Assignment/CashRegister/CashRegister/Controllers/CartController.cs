using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CashRegister.Model;
using CashRegister.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repository;
        private readonly IItemRepository _itemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<CartController> _logger;
        public CartController(ICartRepository repository, ILoggerFactory loggerFactory, IItemRepository itemRepository,ICustomerRepository customerRepository,IDiscountRepository discountRepository)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<CartController>();
            _itemRepository = itemRepository;
            _customerRepository = customerRepository;
            _discountRepository = discountRepository;
        }


        [Route("carts/{customerID:long}")]
        [HttpGet()]
        [ProducesResponseType(typeof(CartByCustomer), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCarts(long customerID)
        {
            

            var customerCart = await _repository.GetCartsByCustomerAsync(customerID);
            var customer = await _customerRepository.GetCustomerAsync(customerID);
            customerCart.CustomerName = customer.CustFName + " " + customer.CustLName;
            decimal total = 0;
            decimal totalDiscount =0;
           
            foreach(Cart cart in customerCart.CartItems)
            {
                var item = await _itemRepository.GetItemMasterAsync(cart.ItemID);
                cart.Price =  item.BasePrice;               
                cart.TotalCost = (cart.Qty * item.BasePrice);
                total = total + (cart.TotalCost??0);

                var discount = await _discountRepository.GetDiscountByItemAsync(cart.ItemID);
                if(discount!=null)
                {
                    cart.DiscountName = discount.DiscountDescription;                   
                    if (discount.DiscountPercentage>0)
                    {
                        cart.Discount = ((cart.Qty * item.BasePrice) * discount.DiscountPercentage) / 100;
                    }
                    else
                    {
                        cart.Discount = ((cart.Qty / discount.BuyQty) * discount.FreeQty) * item.BasePrice;
                    }
                    totalDiscount = totalDiscount+ (cart.Discount??0);                  

                }
               
            }
            customerCart.TotalDiscount = totalDiscount;
            customerCart.Total = total;

            return Ok(customerCart);
        }

        [Route("createOrder/{customerID:long}")]
        [HttpGet()]
        [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder(long customerID)
        {
            var customerCart = await _repository.GetCartsByCustomerAsync(customerID);
            var customer = await _customerRepository.GetCustomerAsync(customerID);
            customerCart.CustomerName = customer.CustFName + " " + customer.CustLName;
            decimal total = 0;
            decimal totalDiscount = 0;

            foreach (Cart cart in customerCart.CartItems)
            {
                var item = await _itemRepository.GetItemMasterAsync(cart.ItemID);
                cart.Price = item.BasePrice;
                cart.TotalCost = (cart.Qty * item.BasePrice);
                total = total + (cart.TotalCost ?? 0);

                var discount = await _discountRepository.GetDiscountByItemAsync(cart.ItemID);
                if (discount != null)
                {
                    cart.DiscountName = discount.DiscountDescription;
                    if (discount.DiscountPercentage > 0)
                    {
                        cart.Discount = ((cart.Qty * item.BasePrice) * discount.DiscountPercentage) / 100;
                    }
                    else
                    {
                        cart.Discount = ((cart.Qty / discount.BuyQty) * discount.FreeQty) * item.BasePrice;
                    }
                    totalDiscount = totalDiscount + (cart.Discount ?? 0);
                }

            }
            customerCart.TotalDiscount = totalDiscount;
            customerCart.Total = total;
            var data = await _repository.CreateOrderAsync(customerID, total, totalDiscount, customerCart.CartItems);
            return Ok(data);
        }



        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddCart([FromBody]Cart value)
        {

            var _item = await _itemRepository.GetItemMasterAsync(value.ItemID);

            if(value.Qty > _item.ItemQty) // To Check customer qty in available stock
            {
                return Ok(new { Message = $"No Stock,  Available Qty : { _item.ItemQty }" });
            }

            var _oldCart = await _repository.GetCartsByCustomerItemAsync(value.CustID, value.ItemID);
            if(_oldCart!=null) //check if same item already exist for the same customer
            {
                if((_oldCart.Qty + value.Qty) > _item.ItemQty) // To Check customer qty in available stock
                {
                    return Ok(new { Message = $"Already you have {_oldCart.Qty} items in your cart and requested item qty is {value.Qty}, Total Requested Qty : {(_oldCart.Qty + value.Qty)} , Total Available Qty : { _item.ItemQty }" });
                }

                var _Cart = await _repository.UpdateCartAsync(value); //for updating qty with existing cart
                return Ok(_Cart);
            }
            else
            {
                var _Cart = await _repository.AddCartAsync(value); //for adding new item into cart
                return Ok(_Cart);
            }           
        }

     

    }
}