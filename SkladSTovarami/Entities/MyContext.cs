using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    class MyContext : DbContext
    {
        public MyContext()
          : base("DbConnection")
        { }

        public DbSet<Check> Checks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Emloyees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryNote> DeliveryNotes { get; set; }
        public DbSet<Product> Product { get; set; }//возможно s добавить
        public DbSet<DeliveryInfo> DeliveryInfos { get; set; }
        public DbSet<OrderInfo> OrderInfos { get; set; }
        public DbSet<CheckInfo> CheckInfos { get; set; }       
        public DbSet<MainProduct> MainProducts { get; set; }
        public DbSet<Secondary> Secondaries { get; set; } 
    }
}
