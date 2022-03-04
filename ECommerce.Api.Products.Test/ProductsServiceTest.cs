using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Test
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                          .UseInMemoryDatabase(nameof(GetProductsReturnsAllProducts))
                          .Options;
            var dbContext = new ProductDbContext(options);           
            CreateProduct(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductsAsync();
            Assert.True(products.IsSuccess);
            Assert.True(products.products.Any());
            Assert.Null(products.ErrorMessage);


        }

        [Fact]
        public async Task GetProductsReturnsProductsUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                          .UseInMemoryDatabase(nameof(GetProductsReturnsProductsUsingValidId))
                          .Options;
            var dbContext = new ProductDbContext(options);
            CreateProduct(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductsAsync(1);
            Assert.True(products.IsSuccess);
            Assert.NotNull(products.products);
            Assert.True(products.products.Id == 1);
            Assert.Null(products.ErrorMessage);


        }

        [Fact]
        public async Task GetProductsReturnsProductsUsingInValidId()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                          .UseInMemoryDatabase(nameof(GetProductsReturnsProductsUsingInValidId))
                          .Options;
            var dbContext = new ProductDbContext(options);
            CreateProduct(dbContext);

            var productProfile = new ProductProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductsAsync(-1);
            Assert.False(products.IsSuccess);
            Assert.Null(products.products);
            Assert.NotNull(products.ErrorMessage);

        }
        private void CreateProduct(ProductDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i = 10,
                    Price = (decimal) (i*3.14)
                });
                dbContext.SaveChanges();
                
            }
        }
    }
}
