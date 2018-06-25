using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    public class OrdersController : Controller
    {
        private OrderRepository service = new OrderRepository();
        private ShopDBEntities db = new ShopDBEntities();
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];
        private ProductRepository service2 = new ProductRepository();

        public List<OrderItemViewModels> ListItem = new List<OrderItemViewModels>();

        // GET: Orders
        public ActionResult Index()
        {
            List<OrderViewModels> list = null;
            string ApiEndPoint = ApiUrl + "api/OrderApi/GetAllOrder";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            list = JsonConvert.DeserializeObject<List<OrderViewModels>>(ListResult);

            //var order = service.GetAllOrder();

            List<CustomerViewModels> list2 = null;
            string ApiEndPoint2 = ApiUrl + "api/OrderApi/GetCustName";
            HttpClient client2 = new HttpClient();
            HttpResponseMessage response2 = client2.GetAsync(ApiEndPoint2).Result;

            string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
            list2 = JsonConvert.DeserializeObject<List<CustomerViewModels>>(ListResult2);

            ViewBag.CustomerId = new SelectList(list2, "Id", "FullName");
            return View(list.ToList());
        }

        //ambil buat searching
        [HttpPost]
        public ActionResult Index(FormCollection input)
        {
            string tanggal = input["date"].Replace("/", "-");
            List<OrderViewModels> list = null;
            string ApiEndPoint = ApiUrl + "api/OrderApi/SearchByKey/" + (input["orderNumber"] + "|" + tanggal + "|" + input["CustomerId"]);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            list = JsonConvert.DeserializeObject<List<OrderViewModels>>(ListResult);


            List<CustomerViewModels> list2 = null;
            string ApiEndPoint2 = ApiUrl + "api/OrderApi/GetCustName";
            HttpClient client2 = new HttpClient();
            HttpResponseMessage response2 = client2.GetAsync(ApiEndPoint2).Result;

            string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
            list2 = JsonConvert.DeserializeObject<List<CustomerViewModels>>(ListResult2);

            ViewBag.CustomerId = new SelectList(list2, "Id", "FullName");
            return View(list);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idx = id ?? 0;
            string ApiEndPoint = ApiUrl + "api/OrderApi/Get/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            OrderViewModels orderVM = JsonConvert.DeserializeObject<OrderViewModels>(result);
            
            
            if (orderVM == null)
            {
                return HttpNotFound();
            }
            return View(orderVM);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            //untuk cari nama customer
            List<CustomerViewModels> list2 = null;
            string ApiEndPoint2 = ApiUrl + "api/OrderApi/GetCustName";
            HttpClient client2 = new HttpClient();
            HttpResponseMessage response2 = client2.GetAsync(ApiEndPoint2).Result;

            string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
            list2 = JsonConvert.DeserializeObject<List<CustomerViewModels>>(ListResult2);

            ViewBag.CustomerId = new SelectList(list2, "Id", "FullName");


            ViewBag.GrandTotal = 0;
            if (Session["ListOrderItem"] != null)
            {
                ListItem = (List<OrderItemViewModels>)Session["ListOrderItem"];
                ViewBag.GrandTotal = ListItem.Sum(s => s.TotalAmount);
            }
            return View();
            
            
        }

        //munculi pop up Add Item
        public ActionResult AddItem()
        {
            //untuk cari nama produk
            List<ProductViewModels> list3 = null;
            string ApiEndPoint3 = ApiUrl + "api/OrderApi/GetProductName";
            HttpClient client3 = new HttpClient();
            HttpResponseMessage response3 = client3.GetAsync(ApiEndPoint3).Result;

            string ListResult3 = response3.Content.ReadAsStringAsync().Result.ToString();
            list3 = JsonConvert.DeserializeObject<List<ProductViewModels>>(ListResult3);

            ViewBag.ProductId = new SelectList(list3, "Id", "ProductName");
            return PartialView("_AddItem");
        }

        //buat method untuk tampilin list item di halaman add order
        public ActionResult AddItemToCurrentOrder(int ProductId, int OrderQuantity)
        {
            if (Session["ListOrderItem"] != null)
            {
                ListItem = (List<OrderItemViewModels>)Session["ListOrderItem"];
            }

            string ApiEndPoint = ApiUrl + "api/ProductApi/Get/" + ProductId;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            var DetailProduct = JsonConvert.DeserializeObject<ProductViewModels>(ListResult);

            ListItem.Add(new OrderItemViewModels
            {
                ProductId = DetailProduct.Id,
                ProductName = DetailProduct.ProductName,
                UnitPrice = DetailProduct.UnitPrice.HasValue ? DetailProduct.UnitPrice.Value : 0,
                Quantity = OrderQuantity,
                TotalAmount = (DetailProduct.UnitPrice.HasValue ? DetailProduct.UnitPrice.Value : 0) * OrderQuantity
            });

            string json = JsonConvert.SerializeObject(ListItem);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            string ApiEndPoint2 = ApiUrl + "api/OrderApi/GroupListItem/";
            HttpClient client2 = new HttpClient();

            HttpResponseMessage response2 = client2.PostAsync(ApiEndPoint2, byteContent).Result;

            string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
            var ListItemCount = JsonConvert.DeserializeObject<List<OrderItemViewModels>>(ListResult2);

            //Session["ListOrderItem"] = ListItemCount;

            //ViewBag.GrandTotal = ListItemCount.Sum(s => s.TotalAmount);
            return PartialView("_ListOrderItem", ListItemCount);
        }

        public ActionResult RemoveItemFromCurrentOrder(int ProductId)
        {
            if (Session["ListOrderItem"] != null)
            {
                ListItem = (List<OrderItemViewModels>)Session["ListOrderItem"];
            }

            OrderRemoveViewModels model = new OrderRemoveViewModels();
            model.ProductId = ProductId;
            model.ListOrderItem = ListItem;

            string json = JsonConvert.SerializeObject(model);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            string ApiEndPoint2 = ApiUrl + "api/OrderApi/RemoveItem/";
            HttpClient client2 = new HttpClient();

            HttpResponseMessage response2 = client2.PostAsync(ApiEndPoint2, byteContent).Result;

            string ListResult2 = response2.Content.ReadAsStringAsync().Result.ToString();
            var RemoveItemvarian = JsonConvert.DeserializeObject<List<OrderItemViewModels>>(ListResult2);

            Session["ListOrderItem"] = RemoveItemvarian;
            ViewBag.GrandTotal = RemoveItemvarian.Sum(s => s.TotalAmount);
            return PartialView("_ListOrderItem", RemoveItemvarian);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customer, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
