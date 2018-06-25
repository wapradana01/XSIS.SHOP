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
    public class CustomersController : Controller
    {
        private string ApiUrl = WebConfigurationManager.AppSettings["XSIS.Shop.API"];
        private CustomerRepository service = new CustomerRepository();

        // GET: Customers
        public ActionResult Index()
        {
            List<CustomerViewModels> list = null;
            string ApiEndPoint = ApiUrl + "api/CustomerApi/";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            list = JsonConvert.DeserializeObject<List<CustomerViewModels>>(ListResult);
            return View(list);
        }

        //ambil buat searching
        [HttpPost]
        public ActionResult Index(FormCollection input)
        {
            List<CustomerViewModels> list = null;
            string ApiEndPoint = ApiUrl + "api/CustomerApi/SearchByKey/" + (input["nama"] + "|" + input["city"] + "|" + input["email"]);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string ListResult = response.Content.ReadAsStringAsync().Result.ToString();
            list = JsonConvert.DeserializeObject<List<CustomerViewModels>>(ListResult);
            return View(list);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idx = id ?? 0;

            //API Akses
            string ApiEndPoint = ApiUrl + "api/CustomerApi/Get/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            CustomerViewModels custVM = JsonConvert.DeserializeObject<CustomerViewModels>(result);

            //CustomerViewModels custVM = service.GetCustomerById(idx);

            if (custVM == null)
            {
                return HttpNotFound();
            }
            return View(custVM);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModels customer)
        {
            if (ModelState.IsValid)
            {
                //panggil api untuk cek nama
                // Check Nama API Akses http://localhost:2099/api/CustomerApi/CekNamaExisting?param=Maria&param2=Anders
                string ApiEndPoint1 = ApiUrl + "api/CustomerApi/cekNama/" + customer.FirstName + "/" + customer.LastName;
                HttpClient client1 = new HttpClient();
                HttpResponseMessage response1 = client1.GetAsync(ApiEndPoint1).Result;
                bool nama = bool.Parse(response1.Content.ReadAsStringAsync().Result.ToString());

                //panggil api untuk cek email
                string ApiEndPoint2 = ApiUrl + "api/CustomerApi/cekEmail/" + customer.Email;
                HttpClient client2 = new HttpClient();
                HttpResponseMessage response2 = client2.GetAsync(ApiEndPoint2).Result;
                bool email = bool.Parse(response2.Content.ReadAsStringAsync().Result.ToString());

                if (nama && email)
                {
                    ModelState.AddModelError("", "Maaf nama sudah ada di database");
                    ModelState.AddModelError("", "Maaf email sudah ada di database");
                    return View(customer);
                }
                else if (nama)
                {
                    ModelState.AddModelError("", "Maaf nama sudah ada di database");
                    return View(customer);
                }
                else if (email)
                {
                    ModelState.AddModelError("", "Maaf email sudah ada di database");
                    return View(customer);
                }
                else
                {
                    //API Akses
                    string json = JsonConvert.SerializeObject(customer);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string ApiEndPoint = ApiUrl + "api/CustomerApi/Post/";
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
                        return View(customer);
                    }
                }
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idx = id ?? 0;

            //API Akses
            string ApiEndPoint = ApiUrl + "api/CustomerApi/Get/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            CustomerViewModels custVM = JsonConvert.DeserializeObject<CustomerViewModels>(result);

            //CustomerViewModels custVM = service.GetCustomerById(idx);

            if (custVM == null)
            {
                return HttpNotFound();
            }
            return View(custVM);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerViewModels customer)
        {
            if (ModelState.IsValid)
            {
                //API Akses
                string json = JsonConvert.SerializeObject(customer);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string ApiEndPoint = ApiUrl + "api/CustomerApi/";
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
                    return View(customer);
                }
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idx = id ?? 0;

            //API Akses
            string ApiEndPoint = ApiUrl + "api/CustomerApi/" + idx;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(ApiEndPoint).Result;

            string result = response.Content.ReadAsStringAsync().Result.ToString();
            CustomerViewModels custVM = JsonConvert.DeserializeObject<CustomerViewModels>(result);

            //CustomerViewModels custVM = service.GetCustomerById(idx);

            if (custVM == null)
            {
                return HttpNotFound();
            }
            return View(custVM);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //API Akses
            string ApiEndPoint = ApiUrl + "api/CustomerApi/" + id;
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
