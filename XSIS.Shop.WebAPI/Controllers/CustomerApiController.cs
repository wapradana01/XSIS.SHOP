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
    public class CustomerApiController : ApiController
    {
        private CustomerRepository service = new CustomerRepository();

        //ambil semua data customer
        [HttpGet]
        public List<CustomerViewModels> Get()
        {
            var result = service.GetAllCustomer();
            return result;
        }

        //ambil data customer berdasarkan id
        [HttpGet]
        public CustomerViewModels Get(int id)
        {
            var result = service.GetCustomerById(id);
            return result;
        }

        //search bar
        [HttpGet]
        public List<CustomerViewModels> SearchByKey(string id)
        {
            string[] Parameters = id.Split('|');

            string param1 = Parameters[0];
            string param2 = Parameters[1];
            string param3 = Parameters[2];

            var result = service.SearchBy(param1, param2, param3);
            return result;
        }

        //cek nama di dalam database
        [HttpGet]
        public bool cekNama(string id1, string id2)
        {
            bool result = service.cekNama(id1, id2);
            return result;
        }

        //cek email di dalam database
        [HttpGet]
        public bool cekEmail(string id)
        {
            bool result = service.cekEmail(id);
            return result;
        }

        //tambah customer
        [HttpPost]
        public int post(CustomerViewModels customer)
        {
            try
            {
                service.AddNewCustomer(customer);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //update customer
        [HttpPut]
        public int put(CustomerViewModels customer)
        {
            try
            {
                service.UpdateCustomer(customer);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //delete customer by id
        [HttpDelete]
        public int delete(int id)
        {
            try
            {
                service.DeleteCostumer(id);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
