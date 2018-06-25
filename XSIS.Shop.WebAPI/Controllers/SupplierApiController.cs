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
    public class SupplierApiController : ApiController
    {
        private SupplierRepository service = new SupplierRepository();

        //ambil semua data supplier
        [HttpGet]
        public List<SupplierViewModels> Get()
        {
            var result = service.GetAllSupplier();
            return result;
        }

        //ambil data supplier berdasarkan id
        [HttpGet]
        public SupplierViewModels Get(int id)
        {
            var result = service.GetSupplierById(id);
            return result;
        }

        //tambah supplier
        [HttpPost]
        public int post(SupplierViewModels supplier)
        {
            try
            {
                service.AddNewSupplier(supplier);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //update supplier
        [HttpPut]
        public int put(SupplierViewModels supplier)
        {
            try
            {
                service.UpdateSupplier(supplier);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //delete supplier by id
        [HttpDelete]
        public int delete(int id)
        {
            try
            {
                service.DeleteSupplier(id);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
