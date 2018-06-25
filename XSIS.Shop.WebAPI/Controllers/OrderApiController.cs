using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XSIS.Shop.ViewModels;
using XSIS.Shop.Repository;


namespace XSIS.Shop.WebAPI.Controllers
{
    public class OrderApiController : ApiController
    {
        private OrderRepository service = new OrderRepository();

        [HttpGet]
        public List<OrderViewModels> GetAllOrder()
        {
            var result = service.GetAllOrder();
            return result;
        }

        [HttpGet]
        public OrderViewModels Get(int id)
        {
            var result = service.GetDetailOrderById(id);
            return result;
        }

        [HttpGet]
        public List<CustomerViewModels> GetCustName()
        {
            var result = service.GetCustomerName();
            return result;
        }

        [HttpGet]
        public List<ProductViewModels> GetProductName()
        {
            var result = service.GetProductName();
            return result;
        }

        //search bar
        [HttpGet]
        public List<OrderViewModels> SearchByKey(string id)
        {
            string[] Parameters = id.Split('|');

            string param1 = Parameters[0];
            string param2 = Parameters[1];
            string param3 = Parameters[2];

            var result = service.SearchBy(param1, param2, param3);
            return result;
        }

        [HttpPost]
        public List<OrderItemViewModels> GroupListItem(List<OrderItemViewModels> ListItem)
        {
            var result = service.GroupListItem(ListItem);
            return result;
        }

        [HttpPost]
        public List<OrderItemViewModels> RemoveItem(OrderRemoveViewModels OrderRemoveItem)
        {
            var result = service.RemoveItem(OrderRemoveItem);
            return result;
        }

        [HttpGet]
        public int GetLatestOrderID()
        {
            var result = service.GetLatestOrderID();
            return result;
        }
    }
}
