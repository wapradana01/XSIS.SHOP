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
    public class CustomerRepository
    {
        //select * from customer
        public List<CustomerViewModels> GetAllCustomer()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var list = db.Customer.ToList();
                List<CustomerViewModels> listVM = new List<CustomerViewModels>();

                foreach (var item in list)
                {
                    CustomerViewModels viewModel = new CustomerViewModels();
                    viewModel.Id = item.Id;
                    viewModel.FirstName = item.FirstName;
                    viewModel.LastName = item.LastName;
                    viewModel.City = item.City;
                    viewModel.Country = item.Country;
                    viewModel.Phone = item.Phone;
                    viewModel.Email = item.Email;
                    listVM.Add(viewModel);
                }

                return listVM;
            }
        }

        //select * from customer where id = "id"
        public CustomerViewModels GetCustomerById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer customer = db.Customer.Find(id);

                CustomerViewModels CustVM = new CustomerViewModels();
                CustVM.Id = customer.Id;
                CustVM.FirstName = customer.FirstName;
                CustVM.LastName = customer.LastName;
                CustVM.City = customer.City;
                CustVM.Country = customer.Country;
                CustVM.Phone = customer.Phone;
                CustVM.Email = customer.Email;

                return CustVM;
            }
        }

        //create customer
        public void AddNewCustomer(CustomerViewModels customer)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer model = new Customer();
                model.FirstName = customer.FirstName;
                model.LastName = customer.LastName;
                model.City = customer.City;
                model.Country = customer.Country;
                model.Phone = customer.Phone;
                model.Email = customer.Email;
                db.Customer.Add(model);
                db.SaveChanges();
            }
        }

        //update costumer
        public void UpdateCustomer(CustomerViewModels customer)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer model = new Customer();
                model.Id = customer.Id;
                model.FirstName = customer.FirstName;
                model.LastName = customer.LastName;
                model.City = customer.City;
                model.Country = customer.Country;
                model.Phone = customer.Phone;
                model.Email = customer.Email;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        //delete customer
        public void DeleteCostumer(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Customer customer = db.Customer.Find(id);
                db.Customer.Remove(customer);
                db.SaveChanges();
            }
        }

        //searching
        public List<CustomerViewModels> SearchBy(string nama, string city, string email)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var list = db.Customer.ToList();
                List<CustomerViewModels> listVM = new List<CustomerViewModels>();

                foreach (var item in list)
                {
                    if (item.Email == null)
                    {
                        item.Email = " ";
                    }
                    if (((item.FirstName.ToLower()+ " " +item.LastName.ToLower()).Contains(nama.ToLower()) || string.IsNullOrEmpty(nama) || string.IsNullOrWhiteSpace(nama))
                        && (item.City.ToLower().Contains(city.ToLower()) || item.Country.ToLower().Contains(city.ToLower()) || string.IsNullOrEmpty(city) || string.IsNullOrWhiteSpace(city))
                        && (item.Email.ToLower().Contains(email.ToLower()) || string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email)))
                    {
                        CustomerViewModels viewModel = new CustomerViewModels();
                        viewModel.Id = item.Id;
                        viewModel.FirstName = item.FirstName;
                        viewModel.LastName = item.LastName;
                        viewModel.City = item.City;
                        viewModel.Country = item.Country;
                        viewModel.Phone = item.Phone;
                        viewModel.Email = item.Email;
                        listVM.Add(viewModel);    
                    }
                }

                return listVM;
            }
        }

        //cek nama
        public bool cekNama(string namadepan, string namabelakang)
        {
            ShopDBEntities db = new ShopDBEntities();
            Customer c1 = (from a in db.Customer
                           where a.FirstName == namadepan
                           && a.LastName == namabelakang
                           select a).SingleOrDefault();
            if (c1 != null)
            {
                //data ketemu
                return true;
            }
            else
            {
                //data tidak ketemu
                return false;
            }
        }

        //cek email
        public bool cekEmail(string email)
        {
            if (email == null)
            {
                return false;
            }
            else
            {
                ShopDBEntities db = new ShopDBEntities();
                Customer c1 = (from a in db.Customer
                               where a.Email == email
                               select a).SingleOrDefault();
                if (c1 != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
