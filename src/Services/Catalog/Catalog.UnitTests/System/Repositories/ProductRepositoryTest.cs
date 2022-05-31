using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.UnitTests.MockData;
using FluentAssertions;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.UnitTests.System.Repositories
{
    public class ProductRepositoryTest : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly ProductRepository sut;
        public ProductRepositoryTest(DbFixture fixture)
        {
            _fixture = fixture;
            sut = new ProductRepository(_fixture.DbContext);
        }

        [Fact]
        public async Task GetProducts_ReturnProductCollection()
        {
            /// Arrange

            /// Act
            var result = await sut.GetProducts();

            /// Assert
            result.Should().HaveCountGreaterThan(1);
        }

        [Fact]
        public async Task GetProduct_ReturnProductById()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f8";

            /// Act
            var result = await sut.GetProduct(id);

            /// Assert
            result.Should().NotBeNull();
            var item = Assert.IsType<Product>(result);
            Assert.Equal(id, item.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnProductByCategory()
        {
            /// Arrange
            var category = "Smart Phone";

            /// Act
            var result = await sut.GetProductByCategory(category);

            /// Assert
            var item = Assert.IsType<List<Product>>(result);
            Assert.All(item, x => x.Category = category);
        }

        [Fact]
        public async Task GetProduct_ReturnProductByName()
        {
            /// Arrange
            string name = "iPhone X7";

            /// Act
            var result = await sut.GetProductByName(name);

            /// Assert
            var item = Assert.IsType<List<Product>>(result);
            Assert.All(item, x => x.Name = name);
        }

        [Fact]
        public async Task CreateProduct_should_insert_new_product()
        {
            /// Arrange
            var newProduct = new Product
            {
                Id = "602d2149e773f2a3990b47fb",
                Name = "Samsung Galaxy S11+",
                Description = "Samsung Galaxy S11+ mobile phone",
                Price = 1400
            };

            /// Act   
            var loadedProduct = await sut.GetProduct(newProduct.Id);

            ///Assert
            loadedProduct.Should().BeNull();

            /// Act 
            await sut.CreateProduct(newProduct);
            loadedProduct = await sut.GetProduct(newProduct.Id);

            ///Assert
            loadedProduct.Should().NotBeNull();
            loadedProduct.Id.Should().Be(newProduct.Id);
            loadedProduct.Name.Should().Be(newProduct.Name);
        }

        [Fact]
        public async Task UpdateProduct_should_edit_product()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f8";

            /// Act   
            await sut.UpdateProduct(new Product { Id = id, Name = "Samsung Galaxy S10+", Description = "Samsung Galaxy S10+ mobile phone", Price = 1100 });

            var loadedProduct = await sut.GetProduct(id);

            ///Assert
            loadedProduct.Should().NotBeNull();
            loadedProduct.Id.Should().Be(id);
            loadedProduct.Name.Should().Be("Samsung Galaxy S10+");
        }

        [Fact]
        public async Task DeleteProduct_should_remove_product()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f9";

            /// Act   
            var loadedProduct = await sut.GetProduct(id);

            ///Assert
            loadedProduct.Should().NotBeNull();

            /// Act  
            await sut.DeleteProduct(id);
            loadedProduct = await sut.GetProduct(id);

            ///Assert
            loadedProduct.Should().BeNull();
        }
    }
}
