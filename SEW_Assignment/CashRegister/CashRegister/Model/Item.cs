using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Model
{
    public class Item 
    {
        [Key]       
        public long ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int ItemQty { get; set; }
        public decimal ?ItemWeight { get; set; }
        public decimal BasePrice { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();
            if (ItemName.Trim().Length == 0)
            {
                results.Add(new ValidationResult("Item Name should not be blank", new[] { "ItemName" }));
            }

            if (ItemQty < 1)
            {
                results.Add(new ValidationResult("Invalid number of units", new[] { "ItemQty" }));
            }

            if (BasePrice < 1)
            {
                results.Add(new ValidationResult("Base price should be greater than zero", new[] { "BasePrice" }));
            }

            return results;
        }
    }
}
