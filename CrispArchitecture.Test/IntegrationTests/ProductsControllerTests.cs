using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CrispArchitecture.Application.Contracts.v1.Products;
using FluentAssertions;
using Xunit;

namespace CrispArchitecture.Test.IntegrationTests
{
    public class ProductsControllerTests : IntegrationTest
    {
        private const string Url = BaseUrl + "products/";

        [Fact]
        public async Task Get_WithWrongProductId_ReturnsNotFound()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync(BaseUrl + "products/" + Guid.NewGuid());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_WithMatchingId_ReturnsProduct()
        {
            // Arrange
            await AuthenticateAsync();
            var productResponse = await CreateProductAsync();

            // Act
            var response = await TestClient.GetAsync(BaseUrl + "products/" + productResponse.Id);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<ProductResponseDto>()).Should().BeEquivalentTo(productResponse);
        }

        [Fact]
        public async Task GetAll_WithoutAnyProductsInDb_ReturnsEmpty()
        {
            // Arrange

            // Act
            var response = await TestClient.GetAsync(BaseUrl + "products");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<List<ProductResponseDto>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_WhenProductExists_ReturnsProduct()
        {
            // Arrange
            await AuthenticateAsync();
            var productResponse = await CreateProductAsync();

            // Act
            var response = await TestClient.GetAsync(BaseUrl + "products");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedProducts = await response.Content.ReadFromJsonAsync<List<ProductResponseDto>>();

            returnedProducts.Should().HaveCount(1);
            returnedProducts.Should().ContainEquivalentOf(productResponse);
        }

        [Fact]
        public async Task GetAll_WhenProductsExists_ReturnsAllProducts()
        {
            // Arrange
            await AuthenticateAsync();
            var products = new List<ProductResponseDto>();

            for (int i = 0; i < 10; i++)
            {
                products.Add(await CreateProductAsync());
            }

            // Act
            var response = await TestClient.GetAsync(BaseUrl + "products");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedProducts = await response.Content.ReadFromJsonAsync<List<ProductResponseDto>>();

            returnedProducts.Should().HaveCount(10);
            returnedProducts.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task CreateProduct_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange
            var product = new ProductCommandDto
            {
                Name = "Test",
                Price = 500
            };

            // Act
            var response =
                await TestClient.PostAsJsonAsync(BaseUrl + "products", product);

            var httpResponse = await TestClient.GetAsync(Url);
            var returnedProducts = await httpResponse.Content.ReadFromJsonAsync<List<ProductResponseDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            returnedProducts.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateProduct_WithWrongPriceType_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response =
                await TestClient.PostAsJsonAsync(BaseUrl + "products", new { name = "Test", price = "hello" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateProduct_SaveToDatabase_ReturnsProductResponse()
        {
            // Arrange
            await AuthenticateAsync();

            var product = new ProductCommandDto
            {
                Name = "Test",
                Price = 500
            };

            // Act
            var response =
                await TestClient.PostAsJsonAsync(BaseUrl + "products", product);

            // Assert
            var productResponse = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            productResponse.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task UpdateProduct_WhenNoProductExists_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.PutAsJsonAsync(Url + Guid.NewGuid(),
                new { name = "test", price = 500 });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateProduct_WhenProductExists_ReturnsNoContent()
        {
            // Arrange
            await AuthenticateAsync();
            var productToUpdate = await CreateProductAsync();

            var newProduct = new ProductCommandDto
            {
                Name = "New Test",
                Price = 520
            };

            // Act
            var response = await TestClient.PutAsJsonAsync(Url + productToUpdate.Id, newProduct);
            bool result = await ContainsEquivalentProduct(productToUpdate.Id, newProduct);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteProduct_WhenNoProductExists_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.DeleteAsync(Url + Guid.NewGuid());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteProduct_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange

            // Act
            var response = await TestClient.DeleteAsync(Url + Guid.NewGuid());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductExists_ReturnsNoContent()
        {
            // Arrange
            await AuthenticateAsync();
            var createdProduct = await CreateProductAsync();

            // Act
            var response = await TestClient.DeleteAsync(Url + createdProduct.Id);
            bool result = await ContainsEquivalentProduct(createdProduct.Id,
                new ProductCommandDto { Name = createdProduct.Name, Price = createdProduct.Price });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            result.Should().BeFalse();
        }

        private async Task<bool> ContainsEquivalentProduct(Guid id, ProductCommandDto product)
        {
            var httpResponse = await TestClient.GetAsync(Url + id);
            var returnedProduct = await httpResponse.Content.ReadFromJsonAsync<ProductResponseDto>();

            if (returnedProduct != null && product.Name != returnedProduct.Name)
                return false;

            if (returnedProduct != null && Math.Abs(product.Price - returnedProduct.Price) > 0.1)
                return false;

            return true;
        }

        // AUTHENTICATE BEFORE USING
        private async Task<ProductResponseDto> CreateProductAsync(ProductCommandDto productRequest = null)
        {
            var product = productRequest ?? new ProductCommandDto
            {
                Name = "Test product",
                Price = 300
            };

            var response = await TestClient.PostAsJsonAsync(BaseUrl + "products", product);
            return await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        }
    }
}