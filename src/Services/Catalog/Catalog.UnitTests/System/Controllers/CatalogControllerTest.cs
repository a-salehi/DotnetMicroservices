using Catalog.API.Controllers;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Catalog.UnitTests.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.UnitTests.System.Controllers
{
    public class CatalogControllerTest
    {
        private readonly CatalogController _controller;
        List<Product> _items = ProductMockData.GetProducts();

        public CatalogControllerTest()
        {            
            var mockRepo = new Mock<IProductRepository>();
            var logger = new Mock<ILogger<CatalogController>>();
            mockRepo.Setup(repo => repo.GetProducts()).ReturnsAsync(_items);
            mockRepo.Setup(repo => repo.GetProduct(It.IsAny<string>()))
            .Returns<string>((id) => {
                Product returnValue = _items.FindAll(i => i.Id == id)[0];
                return Task.FromResult(returnValue);
            });
            mockRepo.Setup(repo => repo.GetProductByName(It.IsAny<string>()))
                    .Returns<string>((id) => {
                        IEnumerable<Product> returnValue = _items.FindAll(i => i.Name == id);
                        return Task.FromResult(returnValue);
                    });
            mockRepo.Setup(repo => repo.GetProductByCategory(It.IsAny<string>()))
                .Returns<string>((id) => {
                IEnumerable<Product> returnValue = _items.FindAll(i => i.Category == id);
                return Task.FromResult(returnValue);
            });
            mockRepo.Setup(repo => repo.CreateProduct(It.IsAny<Product>()))
            .Callback<Product>(i => _items.Add(i));
            mockRepo.Setup(repo => repo.UpdateProduct(It.IsAny<Product>()))
            .Callback<Product>(i =>
            {
                var item = _items.Find(i => i.Id == i.Id);
                if (item != null)
                {
                    item.Name = i.Name;
                    item.Description = i.Description;
                    item.Price = i.Price;
                }
            });
            mockRepo.Setup(repo => repo.DeleteProduct(It.IsAny<string>()))
            .Callback<string>(id => _items.RemoveAll(i => i.Id == id));
            _controller = new CatalogController(mockRepo.Object, logger.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturn200Status()
        {
            /// Arrange

            /// Act
            var result = await _controller.GetProducts();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            /// Assert
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetProductsTest()
        {
            /// Arrange
            
            /// Act
            var okObjectResult = await _controller.GetProducts();

            /// Assert
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            var items = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public async Task GetProductByIdTest()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f1";

            /// Act
            var okObjectResult = await _controller.GetProductById(id);

            /// Assert
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            var item = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(id, item.Id);
        }

        [Fact]
        public async Task GetProductByCategoryTest()
        {
            /// Arrange
            var category = "Smart phone";

            /// Act
            var okObjectResult = await _controller.GetProductByCategory(category);

            /// Assert
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            var items = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(3, items.Count);
        }

        [Fact]
        public async Task GetProductByNameTest()
        {
            /// Arrange
            var name = "iPhone X";

            /// Act
            var okObjectResult = await _controller.GetProductByName(name);

            /// Assert
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult.Result);
            var items = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }


        [Fact]
        public async Task CreateProductTest()
        {
            /// Arrange
            var newProduct = ProductMockData.NewProduct();
            
            /// Act
            var createdResponse = await _controller.CreateProduct(newProduct);

            /// Assert
            var response = Assert.IsType<CreatedAtRouteResult>(createdResponse.Result);
            var item = Assert.IsType<Product>(response.Value);
            Assert.Equal("iPhone X7", item.Name);
        }

        [Fact]
        public async Task UpdateProductTest()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f1";

            /// Act
            var okObjectResult = await _controller.UpdateProduct(new Product { Id = id, Name = "Samsung Galaxy S10+", Description = "Samsung Galaxy S10+ mobile phone", Price = 1100 });

            /// Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
            var item = _items.Find(i => i.Id == id);
            Assert.Equal("Samsung Galaxy S10+", item.Name);
        }

        [Fact]
        public async Task DeleteProductByIdTest()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f1";
            var item = _items.Find(i => i.Id == id);


            Assert.NotNull(item);

            /// Act
            var okObjectResult = await _controller.DeleteProductById(id);

            /// Assert
            Assert.IsType<OkObjectResult>(okObjectResult);
            item = _items.Find(i => i.Id == id);
            Assert.Null(item);
        }
    }
}
