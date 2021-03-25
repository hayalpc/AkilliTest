using AkilliTest.Business.Interfaces;
using AkilliTest.Models;
using Data.Contexts;
using Data.Contexts.Interfaces;
using Data.Models;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliTest.Business
{
    public class AkilliTestBusiness : IAkilliTestBusiness
    {
        private readonly IBaseRepository<Product> productRepository;
        private readonly IBaseRepository<Order> orderRepository;
        private readonly IBaseRepository<OrderProduct> orderProductRepository;
        private readonly IBaseRepository<Category> categoryRepository;

        public AkilliTestBusiness(IBaseRepository<Product> productRepository, IBaseRepository<Category> categoryRepository, IBaseRepository<Order> orderRepository, IBaseRepository<OrderProduct> orderProductRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.orderRepository = orderRepository;
            this.orderProductRepository = orderProductRepository;
        }

        public List<Order> getOrders()
        {
            return orderRepository.GetQuery().ToList();
        }

        public OrderStatistics getOrderStatistics(List<Order> orders)
        {
            try
            {
                var orderIds = orders.Select(x => x.ID).ToList();

                var orderProducts = orderProductRepository.GetQuery(x => orderIds.Contains(x.OrderID)).ToList();

                var productIds = orderProducts.Select(x => x.ProductID).ToList();

                var products = productRepository.GetQuery(x => productIds.Contains(x.ID)).ToList();

                foreach (var order in orders)
                {
                    if (order.OrderProducts == null)
                    {
                        order.OrderProducts = orderProducts.Where(x => x.OrderID == order.ID).ToList();
                        foreach (var orderProduct in orderProducts)
                        {
                            if (orderProduct.Product == null)
                                orderProduct.Product = products.Where(x => x.ID == orderProduct.ProductID).FirstOrDefault();
                        }
                    }
                }

                //stored procedure ile daha hızlı sonuç alınabilir.
                var orderStatistics = orders.GroupBy(x => x.OrderProducts.GroupBy(y => y.Product.CategoryID))
                    .Select(o => new OrderStatisticCategory
                    {
                        NumberOfProductsSold = o.Count(),
                        TotalPriceOfProductsSold = (double)o.Sum(k => k.Price)
                    })
                    .ToList();

                return new OrderStatistics { categories = orderStatistics };
            }
            catch (Exception exp)
            {
                //log
                throw exp;
            }
        }

        public List<Product> getProductsOfCategoryAndDescendants(int categoryID)
        {
            try
            {
                var categories = categoryRepository.GetQuery(x => x.ID == categoryID || x.ParentID == categoryID).Select(c => c.ID).ToList();

                return productRepository.GetQuery(x => categories.Contains(x.CategoryID)).ToList();
            }
            catch (Exception exp)
            {
                //log
                throw exp;
            }
        }

    }
}
