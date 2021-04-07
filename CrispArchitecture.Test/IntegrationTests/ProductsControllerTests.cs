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
        public async Task Create_WithWrongPriceType_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response =
                await TestClient.PostAsJsonAsync(BaseUrl + "products", new { name = "Test", price = "hello" });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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