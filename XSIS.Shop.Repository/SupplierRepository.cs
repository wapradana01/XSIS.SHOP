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
    public class SupplierRepository
    {
        //select * from supplier
        public List<SupplierViewModels> GetAllSupplier()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var list = db.Supplier.ToList();
                List<SupplierViewModels> listVM = new List<SupplierViewModels>();

                foreach (var item in list)
                {
                    SupplierViewModels viewModel = new SupplierViewModels();
                    viewModel.Id = item.Id;
                    viewModel.CompanyName = item.CompanyName;
                    viewModel.ContactName = item.ContactName;
                    viewModel.ContactTitle = item.ContactTitle;
                    viewModel.City = item.City;
                    viewModel.Country = item.Country;
                    viewModel.Phone = item.Phone;
                    viewModel.Fax = item.Fax;
                    listVM.Add(viewModel);
                }
                return listVM;
            }
        }

        //select * from supplier where id = "id"
        public SupplierViewModels GetSupplierById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier supplier = db.Supplier.Find(id);
                SupplierViewModels suppVM = new SupplierViewModels();
                suppVM.Id = supplier.Id;
                suppVM.CompanyName = supplier.CompanyName;
                suppVM.ContactName = supplier.ContactName;
                suppVM.ContactTitle = supplier.ContactTitle;
                suppVM.City = supplier.City;
                suppVM.Country = supplier.Country;
                suppVM.Phone = supplier.Phone;
                suppVM.Fax = supplier.Fax;

                List<ProductViewModels> ListProduct = new List<ProductViewModels>();
                ListProduct = (from d in db.Product
                               where d.SupplierId == suppVM.Id
                               select new ProductViewModels
                               {
                                   Id = d.Id,
                                   ProductName = d.ProductName,
                                   SupplierId = d.SupplierId,
                                   UnitPrice = d.UnitPrice,
                                   Package = d.Package,
                                   IsDiscontinued = d.IsDiscontinued
                               }).ToList();

                if (ListProduct == null)
                {
                    suppVM.listproduk = null;
                }
                else
                {
                    suppVM.listproduk = ListProduct;
                }

                return suppVM;
            }
        }

        //create supplier
        public void AddNewSupplier(SupplierViewModels supplier)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier model = new Supplier();
                model.Id = supplier.Id;
                model.CompanyName = supplier.CompanyName;
                model.ContactName = supplier.ContactName;
                model.ContactTitle = supplier.ContactTitle;
                model.City = supplier.City;
                model.Country = supplier.Country;
                model.Phone = supplier.Phone;
                model.Fax = supplier.Fax;
                db.Supplier.Add(model);
                db.SaveChanges();
            }
        }

        //update costumer
        public void UpdateSupplier(SupplierViewModels supplier)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier model = new Supplier();
                model.Id = supplier.Id;
                model.CompanyName = supplier.CompanyName;
                model.ContactName = supplier.ContactName;
                model.ContactTitle = supplier.ContactTitle;
                model.City = supplier.City;
                model.Country = supplier.Country;
                model.Phone = supplier.Phone;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //delete customer
        public void DeleteSupplier(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Supplier supplier = db.Supplier.Find(id);
                db.Supplier.Remove(supplier);
                db.SaveChanges();
            }
        }
    }
}
