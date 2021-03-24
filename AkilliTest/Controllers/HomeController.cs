using AkilliTest.Business.Interfaces;
using AkilliTest.Models;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliTest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IAkilliTestBusiness business;

        public HomeController(IAkilliTestBusiness business)
        {
            this.business = business;
        }

        public OrderStatistics getOrderStatistics(List<Order> orders)
        {
            return business.getOrderStatistics(orders);
        }

        public List<Product> getProductsOfCategoryAndDescendants(int categoryID)
        {
            return business.getProductsOfCategoryAndDescendants(categoryID);
        }
    }
}
