using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Model
{
    public class OrderMaster
    {
      
        [Key]
        public long OrderID { get; set; }
        public long? CustID { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalCost { get; set; }

    }
}
