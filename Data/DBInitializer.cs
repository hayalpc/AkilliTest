using Data.Contexts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliTest.Data
{
    public static class DBInitializer
    {
        public static void Seed(BaseDbContext context)
        {
            SeedCategories(context);
            SeedProducts(context);
            SeedOrders(context);
        }

        public static void SeedCategories(BaseDbContext context)
        {
            if (!context.Category.Any())
            {
                var countries = new List<Category>
                {
                    new Category{ Name = "Test1" },
                    new Category{ Name = "Test2" },
                    new Category{ Name = "Test3" },
                };
                context.AddRange(countries);
                context.SaveChanges();

                var cat = context.Category.FirstOrDefault();
                if (cat != null)
                {
                    countries = new List<Category>
                    {
                        new Category{ Name = "Test4",ParentID=cat.ID },
                        new Category{ Name = "Test5",ParentID=cat.ID },
                    };
                    context.AddRange(countries);
                    context.SaveChanges();
                }

            }
        }

        public static void SeedProducts(BaseDbContext context)
        {
            if (!context.Product.Any())
            {
                var cat = context.Category.FirstOrDefault();
                var products = new List<Product>
                {
                    new Product{ Name = "Product1",CategoryID = cat.ID },
                    new Product{ Name = "Product2",CategoryID = cat.ID },
                };
                context.AddRange(products);
                context.SaveChanges();
            }
        }

        public static void SeedOrders(BaseDbContext context)
        {
            try
            {
                if (!context.Order.Any())
                {
                    var products = context.Product.ToList();

                    var order = new Order
                    {
                        Price = new Random().Next(10, 100),
                        CustomerID = 1,
                    };
                    context.Add(order);
                    context.SaveChanges();

                    var orderProducts = new List<OrderProduct>();
                    foreach (var product in products)
                    {
                        orderProducts.Add(new OrderProduct
                        {
                            OrderID = order.ID,
                            ProductID = product.ID,
                            Price = order.Price / products.Count()
                        });
                    }

                    context.AddRange(orderProducts);
                    context.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

    }
}
