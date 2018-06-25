using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XSIS.Shop.Models;
using XSIS.Shop.ViewModels;
using System.Data.Entity;

namespace XSIS.Shop.Repository
{
    public class ProductRepository
    {
        //select * from product
        public List<ProductViewModels> GetAllProduct()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var list = db.Product.Include(p => p.Supplier);

                List<ProductViewModels> listVM = new List<ProductViewModels>();

                foreach (var item in list)
                {
                    ProductViewModels viewModel = new ProductViewModels();
                    viewModel.Id = item.Id;
                    viewModel.ProductName = item.ProductName;
                    viewModel.SupplierId = item.SupplierId;
                    viewModel.UnitPrice = item.UnitPrice;
                    viewModel.Package = item.Package;
                    viewModel.IsDiscontinued = item.IsDiscontinued;
                    viewModel.SupplierName = item.Supplier.CompanyName;
                    listVM.Add(viewModel);
                }
                return listVM;
            }
        }

        //select * from supplier where id = "id"
        public ProductViewModels GetProductById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Product product = db.Product.Find(id);
                ProductViewModels viewModel = new ProductViewModels();
                viewModel.Id = product.Id;
                viewModel.ProductName = product.ProductName;
                viewModel.SupplierId = product.SupplierId;
                viewModel.UnitPrice = product.UnitPrice;
                viewModel.Package = product.Package;
                viewModel.IsDiscontinued = product.IsDiscontinued;
                viewModel.SupplierName = product.Supplier.CompanyName;

                return viewModel;
            }
        }

        //create supplier
        public void AddNewProduct(ProductViewModels product)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Product model = new Product();
                model.Id = product.Id;
                model.ProductName = product.ProductName;
                model.SupplierId = product.SupplierId;
                model.UnitPrice = product.UnitPrice;
                model.Package = product.Package;
                model.IsDiscontinued = product.IsDiscontinued;
                db.Product.Add(model);
                db.SaveChanges();
            }
        }

        //update costumer
        public void UpdateProduct(ProductViewModels product)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Product model = new Product();
                model.Id = product.Id;
                model.ProductName = product.ProductName;
                model.SupplierId = product.SupplierId;
                model.UnitPrice = product.UnitPrice;
                model.Package = product.Package;
                model.IsDiscontinued = product.IsDiscontinued;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //delete customer
        public void DeleteProduct(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Product product = db.Product.Find(id);
                db.Product.Remove(product);
                db.SaveChanges();
            }
        }

        //ambil data nama supplier
        public List<Supplier> GetCompanyName()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var result = db.Supplier.ToList();
                return result;
            }
        }
    }
}
