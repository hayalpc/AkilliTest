using AkilliTest.Models;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliTest.Business.Interfaces
{
    public interface IAkilliTestBusiness
    {
        OrderStatistics getOrderStatistics(List<Order> orders);
        List<Product> getProductsOfCategoryAndDescendants(int categoryID);
    }
}
