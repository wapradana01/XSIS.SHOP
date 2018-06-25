using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XSIS.Shop.ViewModels;
using XSIS.Shop.Models;

namespace XSIS.Shop.Repository
{
    public class OrderRepository
    {
        public List<OrderViewModels> GetAllOrder()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var list = db.Order.ToList();
                List<OrderViewModels> listVM = new List<OrderViewModels>();

                foreach(var item in list)
                {
                    OrderViewModels viewModel = new OrderViewModels();
                    viewModel.Id = item.Id;
                    viewModel.CustomerName = item.Customer.FirstName + " " + item.Customer.LastName;
                    viewModel.OrderDate = item.OrderDate;
                    viewModel.OrderNumber = item.OrderNumber;
                    viewModel.TotalAmount = item.TotalAmount;
                    listVM.Add(viewModel);
                }
                return listVM;
            }
        }

        public List<CustomerViewModels> GetCustomerName()
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
                    viewModel.FullName = item.FirstName + " " + item.LastName;
                    listVM.Add(viewModel);
                }
                return listVM;
            }
        }

        public List<ProductViewModels> GetProductName()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var list = db.Product.ToList();
                List<ProductViewModels> listVM = new List<ProductViewModels>();

                foreach (var item in list)
                {
                    ProductViewModels viewModel = new ProductViewModels();
                    viewModel.Id = item.Id;
                    viewModel.ProductName = item.ProductName;
                    viewModel.UnitPrice = item.UnitPrice;
                    viewModel.Package = item.Package;
                    viewModel.SupplierId = item.SupplierId;
                    listVM.Add(viewModel);
                }
                return listVM;
            }
        }

        public OrderViewModels GetDetailOrderById(int id)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                OrderViewModels model = new OrderViewModels();
                model = (from a in db.Order
                         join b in db.Customer on a.CustomerId equals b.Id
                         where a.Id == id
                         select new OrderViewModels
                         {
                             Id = a.Id,
                             OrderDate = a.OrderDate,
                             OrderNumber = a.OrderNumber,
                             CustomerName = b.FirstName + " " + b.LastName,
                             CustomerId = b.Id,
                             TotalAmount = a.TotalAmount
                         }
                        ).Single();

                model.ListOrderItem = (from a in db.Order
                                       join b in db.OrderItem on a.Id equals b.OrderId
                                       join c in db.Product on b.ProductId equals c.Id
                                       where a.Id == id
                                       select new OrderItemViewModels
                                       {
                                           Id = b.Id,
                                           OrderId = b.OrderId,
                                           OrderNumber = a.OrderNumber,
                                           ProductId = c.Id,
                                           ProductName = c.ProductName,
                                           UnitPrice = b.UnitPrice,
                                           Quantity = b.Quantity,
                                           TotalAmount = b.UnitPrice * b.Quantity
                                       }
                                       ).ToList();
                return model;
            }
        }

        //searching
        public List<OrderViewModels> SearchBy(string orderNumber, string date, string CustId)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var list = db.Order.ToList();
                List<OrderViewModels> listVM = new List<OrderViewModels>();

                DateTime tanggal = new DateTime();
                
                if (!string.IsNullOrEmpty(date))
                {
                    tanggal = DateTime.Parse(date.Replace("-","/")).Date;
                }

                int idCust = 0;

                if (!string.IsNullOrEmpty(CustId))
                {
                    idCust = int.Parse(CustId);
                }

                foreach (var item in list)
                {
                    if (((item.OrderDate == tanggal) || string.IsNullOrEmpty(date) || string.IsNullOrWhiteSpace(date))
                        && (item.OrderNumber.Contains(orderNumber) || string.IsNullOrEmpty(orderNumber) || string.IsNullOrWhiteSpace(orderNumber))
                        && ((item.CustomerId == idCust && idCust != 0) || string.IsNullOrEmpty(CustId) || string.IsNullOrWhiteSpace(CustId)))
                    {
                        OrderViewModels viewModel = new OrderViewModels();
                        viewModel.Id = item.Id;
                        viewModel.CustomerName = item.Customer.FirstName + " " + item.Customer.LastName;
                        viewModel.OrderDate = item.OrderDate;
                        viewModel.OrderNumber = item.OrderNumber;
                        viewModel.TotalAmount = item.TotalAmount;
                        listVM.Add(viewModel);
                    }
                }

                return listVM;
            }
        }

        public List<OrderItemViewModels> GroupListItem(List<OrderItemViewModels> ListItem)
        {
            var CountVarian = (ListItem.GroupBy(x => x.ProductId).Select(a => new OrderItemViewModels
            {
                ProductId = a.Key,
                ProductName = a.First().ProductName,
                UnitPrice = a.First().UnitPrice,
                Quantity = a.Sum(s => s.Quantity),
                TotalAmount = a.Sum(s => s.TotalAmount)
            })).ToList();
            return CountVarian;
        }

        public List<OrderItemViewModels> RemoveItem(OrderRemoveViewModels OrderRemoveItem)
        {
            for (int i = 0; i < OrderRemoveItem.ListOrderItem.Count; i++)
            {
                if (OrderRemoveItem.ListOrderItem[i].ProductId == OrderRemoveItem.ProductId)
                {
                    OrderRemoveItem.ListOrderItem.Remove(OrderRemoveItem.ListOrderItem[i]);
                    break;
                }
            }
            return OrderRemoveItem.ListOrderItem;
        }

        public int GetLatestOrderID()
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                var result = db.Order.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                return result;
            }
        }

        public void AddNewOrder(OrderViewModels order)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                //string[] formats = { "dd/MM/yyyy" };
                //DateTime oDate = new DateTime();//
                //oDate = DateTime.Parse(order.OrderDate).Date;    //.Pars e(order.OrderDate);

                Order model = new Order();
                model.CustomerId = order.CustomerId;
                model.OrderDate = order.OrderDate;
                model.OrderNumber = order.OrderNumber;
                model.TotalAmount = order.TotalAmount;

                db.Order.Add(model);
                db.SaveChanges();

                foreach (var item in order.ListOrderItem)
                {
                    OrderItem modelItem = new OrderItem();
                    modelItem.OrderId = model.Id;
                    modelItem.ProductId = item.ProductId;
                    modelItem.Quantity = item.Quantity;
                    modelItem.UnitPrice = item.UnitPrice;

                    db.OrderItem.Add(modelItem);
                    db.SaveChanges();

                }
            }
        }

        public void createOrder(OrderViewModels order)
        {
            using (ShopDBEntities db = new ShopDBEntities())
            {
                Order model = new Order();
                model.Id = order.Id;
                model.OrderDate = order.OrderDate;
                model.OrderNumber = order.OrderNumber;
                model.TotalAmount = order.TotalAmount;
                //model.
               

                db.Order.Add(model);
                db.SaveChanges();
            }
        }
    }
}
