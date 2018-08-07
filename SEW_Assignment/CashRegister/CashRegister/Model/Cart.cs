using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Model
{
    public class Cart
    {
        [Key]
        public long CartID { get; set; }
        public long? CustID { get; set; }
        public long? ItemID { get; set; }
        public int? Qty { get; set; }
        public string DiscountName {get;set;}
        public decimal? Discount { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalCost { get; set; }



    }

    public class CartByCustomer
    {
        public string CustomerName { get; set; }
        public List<Cart> CartItems { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal Total { get; set; }

    }

}
