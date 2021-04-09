using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CrispArchitecture.Application.Contracts.v1.ProductGroups;
using CrispArchitecture.Application.Contracts.v1.Products;
using CrispArchitecture.Application.Specifications;
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
            var productResponse = await CreateProductAndGroupAsync();

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
            (await response.Content.ReadFromJsonAsync<Pagination<ProductResponseDto>>())?.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_WhenProductExists_ReturnsProduct()
        {
            // Arrange
            await AuthenticateAsync();
            var productResponse = await CreateProductAndGroupAsync();

            // Act
            var response = await TestClient.GetAsync(BaseUrl + "products");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedProducts = await response.Content.ReadFromJsonAsync<Pagination<ProductResponseDto>>();

            returnedProducts?.Data.Should().HaveCount(1);
            returnedProducts?.Data.Should().ContainEquivalentOf(productResponse);
        }

        [Fact]
        public async Task GetAll_WhenProductsExists_ReturnsAllProducts()
        {
            // Arrange
            await AuthenticateAsync();
            var products = new List<ProductResponseDto>();

            for (int i = 0; i < 10; i++)
            {
                products.Add(await CreateProductAndGroupAsync());
            }

            // Act
            var response = await TestClient.GetAsync(BaseUrl + "products?pageSize=10");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedProducts = await response.Content.ReadFromJsonAsync<Pagination<ProductResponseDto>>();

            returnedProducts?.Data.Should().HaveCount(10);
            returnedProducts?.Data.Should().BeEquivalentTo(products);
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
            var returnedProducts = await httpResponse.Content.ReadFromJsonAsync<Pagination<ProductResponseDto>>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            returnedProducts?.Data.Should().BeEmpty();
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
                Price = 500,
            };

            // Act
            var response =
                await TestClient.PostAsJsonAsync(BaseUrl + "products", product);

            // Assert
            var productResponse = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            productResponse.Should().BeEquivalentTo(new {Name = "Test", Price = 500});
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
            var productToUpdate = await CreateProductAndGroupAsync();

            var newProduct = new ProductCommandDto
            {
                Name = "New Test",
                Price = 520,
                ProductGroupId = Guid.Parse("AD729B2B-CEAD-4829-6E2D-08D8FB4E8816")
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
            var createdProduct = await CreateProductAndGroupAsync();

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
        private async Task<ProductResponseDto> CreateProductAndGroupAsync()
        {
            var productGroup = new ProductGroupCommandDto
            {
                Name = "Test"
            };
            
            var responseProductGroup = await TestClient.PostAsJsonAsync(BaseUrl + "ProductGroups/", productGroup);
            var pgResponse = await responseProductGroup.Content.ReadFromJsonAsync<ProductGroupResponseDto>();
            
            var product = new ProductCommandDto
            {
                Name = Guid.NewGuid().ToString(),
                Price = 999,
                ProductGroupId = pgResponse!.Id
            };

            var response = await TestClient.PostAsJsonAsync(Url, product);
            return await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        }
    }
}