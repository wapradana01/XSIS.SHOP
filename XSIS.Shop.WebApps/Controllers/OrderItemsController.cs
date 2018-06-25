using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using XSIS.Shop.Models;
using XSIS.Shop.Repository;
using XSIS.Shop.ViewModels;
using System.Web.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace XSIS.Shop.WebApps.Controllers
{
    public class OrderItemsController : Controller
    {
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];

        // GET: OrderItems
        public ActionResult CreateItem()
        {
            //untuk cari nama produk
            List<ProductViewModels> list3 = null;
            string ApiEndPoint3 = ApiUrl + "api/OrderApi/GetProductName";
            HttpClient client3 = new HttpClient();
            HttpResponseMessage response3 = client3.GetAsync(ApiEndPoint3).Result;

            string ListResult3 = response3.Content.ReadAsStringAsync().Result.ToString();
            list3 = JsonConvert.DeserializeObject<List<ProductViewModels>>(ListResult3);

            ViewBag.ProductId = new SelectList(list3, "Id", "ProductName");
            return PartialView();
        }
    }
}