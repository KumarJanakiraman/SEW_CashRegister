using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Model
{
    public class Customer
    {
        [Key]       
        public long CustID { get; set; }
        public string CustFName { get; set; }
        public string CustLName { get; set; }
        public string CustEmailID { get; set; }
        /*
         we can add more fields like address , .....etc
     */
    }
}
