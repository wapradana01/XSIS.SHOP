using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using XSIS.Shop.ViewModels;
using XSIS.Shop.Repository;
using XSIS.Shop.Models;

namespace XSIS.Shop.WebAPI.Controllers
{
    public class ProductApiController : ApiController
    {
        private ProductRepository service = new ProductRepository();

        //ambil semua data product
        [HttpGet]
        public List<ProductViewModels> GetAllProduct()
        {
            var result = service.GetAllProduct();
            return result;
        }

        //ambil data product berdasarkan id
        [HttpGet]
        public ProductViewModels Get(int id)
        {
            var result = service.GetProductById(id);
            return result;
        }

        //tambah product
        [HttpPost]
        public int post(ProductViewModels supplier)
        {
            try
            {
                service.AddNewProduct(supplier);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //update product
        [HttpPut]
        public int put(ProductViewModels supplier)
        {
            try
            {
                service.UpdateProduct(supplier);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        //delete product by id
        [HttpDelete]
        public int delete(int id)
        {
            try
            {
                service.DeleteProduct(id);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpGet]
        public List<Supplier> getCompanyName()
        {
            var result = service.GetCompanyName();
            return result;
        }
    }
}
