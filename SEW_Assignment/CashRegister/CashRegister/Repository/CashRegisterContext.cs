using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashRegister.Model;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Repository
{
    public class CashRegisterContext : DbContext
    {
        public CashRegisterContext(DbContextOptions<CashRegisterContext> options)
          : base(options) { }
        public CashRegisterContext() { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OrderMaster> OrderMasters { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}
