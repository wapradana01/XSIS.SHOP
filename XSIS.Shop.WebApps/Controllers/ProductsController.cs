using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using XSIS.Shop.Repository;
using XSIS.Shop.ViewModels;
using System.Web.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using XSIS.Shop.Models;

namespace XSIS.Shop.WebApps.Controllers
{
    public class ProductsController : Controller
    {
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];
        private ProductRepository service = new ProductRepository();

        // GET: Products
        public ActionResult Index()
        {
            List<ProductViewModels> list = null;
            string ApiEndPoint = ApiUrl + "api/ProductApi/GetAllProduct";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            list = JsonConvert.DeserializeObject<List<ProductViewModels>>(ListResult);

            //var result = service.GetAllProduct();
            return View(list);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            int idx = id ?? 0;

            string ApiEndPoint = ApiUrl + "api/ProductApi/Get/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            ProductViewModels ProdVM = JsonConvert.DeserializeObject<ProductViewModels>(result);

            //ProductViewModels result = service.GetProductById(idx);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(ProdVM);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            //List<Supplier> list = null;
            //string ApiEndPoint = ApiUrl + "api/ProductApi/getCompanyName/";
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            //string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            //list = JsonConvert.DeserializeObject<List<Supplier>>(ListResult);

            ViewBag.SupplierId = new SelectList(service.GetCompanyName(), "id", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModels product)
        {
            if (ModelState.IsValid)
            {
                service.AddNewProduct(product);
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(service.GetCompanyName(), "id", "CompanyName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id ?? 0;
            ProductViewModels result = service.GetProductById(idx);

            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierId = new SelectList(service.GetCompanyName(), "id", "CompanyName");
            return View(result);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModels product)
        {
            if (ModelState.IsValid)
            {
                service.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(service.GetCompanyName(), "id", "CompanyName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id ?? 0;

            string ApiEndPoint = ApiUrl + "api/ProductApi/Get/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            ProductViewModels ProdVM = JsonConvert.DeserializeObject<ProductViewModels>(result);

            //ProductViewModels result = service.GetProductById(idx);

            if (result == null)
            {
                return HttpNotFound();
            }
            //ViewBag.SupplierId = new SelectList(service.GetCompanyName(), "id", "CompanyName");
            return View(ProdVM);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //API Akses
            string ApiEndPoint = ApiUrl + "api/ProductApi/delete/" + id;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.DeleteAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            int succes = int.Parse(result);
            if (succes == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}
