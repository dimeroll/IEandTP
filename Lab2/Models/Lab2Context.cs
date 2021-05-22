using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class Lab2Context : DbContext
    {
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ShopWorker> ShopWorkers { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<ShopItem> ShopItems { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        public Lab2Context(DbContextOptions<Lab2Context> options)
            :base(options)
        {
            Database.EnsureCreated();
        }
    }
}
