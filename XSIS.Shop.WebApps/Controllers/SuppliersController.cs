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

namespace XSIS.Shop.WebApps.Controllers
{
    public class SuppliersController : Controller
    {
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];
        private SupplierRepository service = new SupplierRepository();

        // GET: Suppliers
        public ActionResult Index()
        {
            List<SupplierViewModels> list = null;
            string ApiEndPoint = ApiUrl + "api/SupplierApi/";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            list = JsonConvert.DeserializeObject<List<SupplierViewModels>>(ListResult);
            return View(list);
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id ?? 0;

            string ApiEndPoint = ApiUrl + "api/SupplierApi/Get/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            SupplierViewModels supVM = JsonConvert.DeserializeObject<SupplierViewModels>(result);

            //SupplierViewModels result = service.GetSupplierById(idx);

            if (supVM == null)
            {
                return HttpNotFound();
            }
            return View(supVM);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SupplierViewModels supplier)
        {
            if (ModelState.IsValid)
            {
                //API Akses
                string json = JsonConvert.SerializeObject(supplier);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string ApiEndPoint = ApiUrl + "api/SupplierApi/post/";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(ApiEndPoint, byteContent).Result;

                string result = response.Content.ReadAsStringAsync().Result.ToString();
                int success = int.Parse(result);

                if (success == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(supplier);
                }
            }

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int idx = id ?? 0;
            SupplierViewModels result = service.GetSupplierById(idx);

            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SupplierViewModels supplier)
        {
            if (ModelState.IsValid)
            {
                //API Akses
                string json = JsonConvert.SerializeObject(supplier);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string ApiEndPoint = ApiUrl + "api/SupplierApi/";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.PutAsync(ApiEndPoint, byteContent).Result;

                string result = response.Content.ReadAsStringAsync().Result.ToString();
                int success = int.Parse(result);

                if (success == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(supplier);
                }
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idx = id ?? 0;
            var result = service.GetSupplierById(idx);
            if (result == null)
            {
                return HttpNotFound();
            }

            return View(result);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //API Akses
            string ApiEndPoint = ApiUrl + "api/SupplierApi/" + id;
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
