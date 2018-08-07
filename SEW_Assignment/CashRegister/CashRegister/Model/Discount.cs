using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Model
{
    public class Discount
    {
        [Key]       
        public long DiscountID { get; set; }
        public string DiscountDescription { get; set; }
        public long ItemID { get; set; }
        public long ?FreeItemID { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }        
        public decimal ?DiscountPercentage { get; set; }
        public int ?BuyQty { get; set; }
        public int ?FreeQty { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();
            if (ItemID  == 0)
            {
                results.Add(new ValidationResult("Item should not be blank", new[] { "ItemID" }));
            }
            if (FreeItemID == 0 && DiscountPercentage==0)
            {
                results.Add(new ValidationResult("Please enter Free Item or Discount Percentage", new[] { "FreeItemID" }));
            }

            if(DiscountPercentage>0 && FreeQty > 0)
            {
                results.Add(new ValidationResult("Discount Percentage Free qty cannot be updated", new[] { "FreeItemID" }));
            }

            if (EffectiveDateFrom ==null)
            {
                results.Add(new ValidationResult("Effective From Date  should not be null", new[] { "EffectiveDateFrom" }));
            }

            if (EffectiveDateTo ==null)
            {
                results.Add(new ValidationResult("Effective To Date  should not be null", new[] { "EffectiveDateTo" }));
            }

            if (EffectiveDateFrom != null && EffectiveDateTo != null)
            {
                if(EffectiveDateTo < EffectiveDateFrom)
                results.Add(new ValidationResult("Effective To Date  should be greater than Effective from Date", new[] { "EffectiveDateTo" }));
            }

                return results;
        }
    }
}
