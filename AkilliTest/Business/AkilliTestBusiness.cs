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
        private readonly IBaseRepository<Category> categoryRepository;
        private readonly IBaseUnitOfWork<BaseDbContext> baseUnitOfWork;

        public AkilliTestBusiness(IBaseRepository<Product> productRepository, IBaseRepository<Category> categoryRepository, IBaseUnitOfWork<BaseDbContext> baseUnitOfWork)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.baseUnitOfWork = baseUnitOfWork;
        }

        public OrderStatistics getOrderStatistics(List<Order> orders)
        {
            try
            {
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

                return productRepository.GetQuery(x => categories.Contains(x.ID)).ToList();
            }
            catch (Exception exp)
            {
                //log
                throw exp;
            }
        }

    }
}
