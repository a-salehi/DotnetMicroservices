using Catalog.API.Entities;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.UnitTests.System.Controllers
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient httpClient;
        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetProductsTest()
        {
            /// Arrange
            string _token = await GetToken();
            httpClient.SetBearerToken(_token);

            /// Act
            var response = await httpClient.GetAsync("api/v1/Catalog");

            /// Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductByIdTest()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f5";
            string _token = await GetToken();
            httpClient.SetBearerToken(_token);

            /// Act
            var response = await httpClient.GetAsync("api/v1/Catalog/" + id);

            /// Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var item = JsonSerializer.Deserialize<Product>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(id, item.Id);
        }
        [Fact]
        public async Task GetProductByCategoryTest()
        {
            /// Arrange
            var category = "Smart Phone";
            string _token = await GetToken();
            httpClient.SetBearerToken(_token);

            /// Act
            var response = await httpClient.GetAsync("api/v1/Catalog/GetProductByCategory/" + category);

            /// Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<List<Product>>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(items, t => t.Category == category);
        }

        [Fact]
        public async Task GetProductByNameTest()
        {
            /// Arrange
            var name = "IPhone X";
            string _token = await GetToken();
            httpClient.SetBearerToken(_token);

            /// Act
            var response = await httpClient.GetAsync("api/v1/Catalog/GetProductByName/" + name);

            /// Assert
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<List<Product>>(stringResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(items, t => t.Name == name);
        }

        [Fact]
        public async Task CreateProductTest()
        {
            /// Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Catalog/");

            request.Content = new StringContent(JsonSerializer.Serialize(new
            {
                Id = "602d2149e773f2a3990b47f4",
                Name = "iPhone X7",
                Category = "Smart phone",
                Summary = "",
                Description = "iPhone X mobile phone",
                ImageFile = "",
                Price = 120
            }), Encoding.UTF8, "application/json");

            string _token = await GetToken();
            httpClient.SetBearerToken(_token);

            /// Act
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpClient.SendAsync(request);

            /// Assert            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProductTest()
        {
            /// Arrange
            var request = new HttpRequestMessage(HttpMethod.Put, "api/v1/Catalog/");

            request.Content = new StringContent(JsonSerializer.Serialize(new
            {
                Id = "602d2149e773f2a3990b47f6",
                Name = "iPhone X7",
                Category = "Smart phone",
                Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                Description = "iPhone X mobile phone",
                ImageFile = "product-2.png",
                Price = 520
            }), Encoding.UTF8, "application/json");

            string _token = await GetToken();
            httpClient.SetBearerToken(_token);

            /// Act
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpClient.SendAsync(request);

            /// Assert            
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteProductByIdTest()
        {
            /// Arrange
            var id = "602d2149e773f2a3990b47f4";
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/Catalog/"+id);
            var _token = await GetToken();
            httpClient.SetBearerToken(_token);

            /// Act
            HttpResponseMessage response = new HttpResponseMessage();
            response = await httpClient.SendAsync(request);

            /// Assert            
            Assert.True(response.IsSuccessStatusCode);
        }
                
        private async Task<string> GetToken()
        {
            var client = new HttpClient();

            // discover endpoints from metadata
            var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://localhost:7000/.well-known/openid-configuration",
                Policy =
                {
                    ValidateIssuerName = false
                }
            });

            if (disco.IsError)
            {
                Assert.True(false);
            }

            // TODO: Get secret from Azure Key Vault
            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint, // "https://localhost:7000/connect/token"

                ClientId = "catalogClient",
                ClientSecret = "secret",
                Scope = "catalogAPI"
            });

            if (tokenResponse.IsError)
            {
                Assert.True(false);
            }

            return tokenResponse.AccessToken;
        }
    }
}
