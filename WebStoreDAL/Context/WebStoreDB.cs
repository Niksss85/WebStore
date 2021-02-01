using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;

namespace WebStoreDAL.Context
{
    public class WebStoreDB:DbContext
    {
        public DbSet<Product> Products { get; set; } // добавление таблиц
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Section> Section { get; set; }
        public WebStoreDB(DbContextOptions<WebStoreDB> options) :base(options){ }

    }
}
