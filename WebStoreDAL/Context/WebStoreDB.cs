using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;
using WebStore.Domain.Orders;

namespace WebStoreDAL.Context
{
    public class WebStoreDB:IdentityDbContext<User, Role, string>
    {
        public DbSet<Product> Products { get; set; } // добавление таблиц
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Section> Sections { get; set; }

        public DbSet<Order> Orders { get; set; }
        public WebStoreDB(DbContextOptions<WebStoreDB> options) :base(options){ }  
    }
}
