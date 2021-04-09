using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CrispArchitecture.Api;
using CrispArchitecture.Application.Contracts.v1.Users;
using CrispArchitecture.Infrastructure.Data;
using CrispArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrispArchitecture.Test.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        protected const string BaseUrl = "http://localhost/api/v1/";
        private readonly IServiceProvider _serviceProvider;

        protected IntegrationTest()
        {
            WebApplicationFactory<Startup> appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var appDescriptor = services.SingleOrDefault(x =>
                            x.ServiceType == typeof(DbContextOptions<AppDbContext>));

                        var identityDescriptor = services.SingleOrDefault(x =>
                            x.ServiceType == typeof(DbContextOptions<AppIdentityDbContext>));

                        if (appDescriptor != null && identityDescriptor != null)
                        {
                            services.Remove(appDescriptor);
                            services.Remove(identityDescriptor);
                        }

                        services.AddDbContext<AppDbContext>(opt =>
                            opt.UseInMemoryDatabase("AppTestDb"));

                        services.AddDbContext<AppIdentityDbContext>(opt =>
                            opt.UseInMemoryDatabase("AppIdentityTestDb"));
                    });
                });

            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", await RegisterAndGetJwtAsync());
        }

        private async Task<string> RegisterAndGetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(BaseUrl + "account/register",
                new RegisterCommandDto
                {
                    Email = "testemail@test.com",
                    Password = "Pa$$w0rd",
                    DisplayName = "TestName"
                });

            var registrationResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            return registrationResponse?.Token;
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();

            var appContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            var identityContext = serviceScope.ServiceProvider.GetService<AppIdentityDbContext>();

            appContext?.Database.EnsureDeleted();
            identityContext?.Database.EnsureDeleted();
        }
    }
}