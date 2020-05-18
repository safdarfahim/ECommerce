using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductProvider : IProductProvider
    {

        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductProvider> logger;
        private readonly IMapper mapper;
        public ProductProvider(ProductsDbContext dbContext,ILogger<ProductProvider> logger,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                dbContext.Products.Add(new Db.Product { Id = 2, Name = "Mouce", Price = 15, Inventory =300 });
                dbContext.Products.Add(new Db.Product { Id = 3, Name = "Mpniter", Price = 150, Inventory = 1000 });
                dbContext.Products.Add(new Db.Product { Id = 4, Name = "CPU", Price = 200, Inventory = 2000 });
                dbContext.SaveChanges();
            }

        }
        public async Task<(bool IsSuccess, IEnumerable<Models.Product> Products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if(products!=null && products.Any())
                {
                   var result= mapper.Map<IEnumerable<Db.Product>,IEnumerable<Models.Product>>(products);
                    return (true,result,null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Product Product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var products = await dbContext.Products.FirstOrDefaultAsync(p=>p.Id==id);
                if (products != null)
                {
                    var result = mapper.Map<Db.Product,Models.Product>(products);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message);
                return (false, null, ex.Message);
            }
        }
    }
}
