using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrispArchitecture.Domain.Entities;

namespace CrispArchitecture.Infrastructure.Data
{
    public class SeedData
    {
        public static async Task SeedDatabase(AppDbContext context)
        {
            Guid product1 = Guid.NewGuid();
            Guid product2 = Guid.NewGuid();
            Guid product3 = Guid.NewGuid();
            Guid product4 = Guid.NewGuid();

            if (!context.Products.Any())
            {
                List<Product> products = new List<Product>
                {
                    new Product
                    {
                        Id = product1,
                        Name = "Office Chair",
                        Price = 149.99
                    },
                    new Product
                    {
                        Id = product2,
                        Name = "Height Adjustable Desk",
                        Price = 409.99
                    },
                    new Product
                    {
                        Id = product3,
                        Name = "Desk Drawer",
                        Price = 129.99
                    },
                    new Product
                    {
                        Id = product4,
                        Name = "L-Shaped Desk",
                        Price = 399.99
                    },
                };

                await context.Products.AddRangeAsync(products);
            }

            if (!context.Customers.Any() && !context.Orders.Any())
            {
                Guid customer1 = Guid.NewGuid();
                Guid customer2 = Guid.NewGuid();
                
                List<Customer> customers = new List<Customer>
                {
                    new Customer
                    {
                       Id = customer1,
                       Name = "Ben Hansel",
                       Email = "benhansel@gmail.com",
                       Phone = "12345678"
                    },
                    new Customer
                    {
                       Id = customer2,
                       Name = "Ben Hansel",
                       Email = "benhansel@gmail.com",
                       Phone = "12345678"
                    }
                };

                Guid order1 = Guid.NewGuid();
                Guid order2 = Guid.NewGuid();

                List<Order> orders = new List<Order>
                {
                    new Order
                    {
                        Id = order1,
                        CustomerId = customer1,
                        Total = 149.99,
                        IsPaid = false,
                    },
                    new Order
                    {
                        Id = order2,
                        CustomerId = customer2,
                        Total = 939.97,
                        IsPaid = false,
                    },
                };

                List<LineItem> lineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        OrderId = order1,
                        ProductId = product1,
                        Amount = 1
                    },
                    new LineItem
                    {
                        OrderId = order2,
                        ProductId = product2,
                        Amount = 1
                    },
                    new LineItem
                    {
                        OrderId = order2,
                        ProductId = product3,
                        Amount = 1
                    },
                    new LineItem
                    {
                        OrderId = order2,
                        ProductId = product4,
                        Amount = 1
                    }
                };

                await context.Customers.AddRangeAsync(customers);
                await context.Orders.AddRangeAsync(orders);
                await context.LineItems.AddRangeAsync(lineItems);
            }
            
            await context.SaveChangesAsync();
        }
    }
}