using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.ViewModels
{
    public class OrderViewModels
    {
        public int Id { get; set; }

        [Display(Name = "Tanggal Pemesanan")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Nomer Pemesanan")]
        public string OrderNumber { get; set; }

        [Display(Name = "Nama Customer")]
        public string CustomerName { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Jumlah")]
        public Nullable<decimal> TotalAmount { get; set; }

        public List<OrderItemViewModels> ListOrderItem { get; set; }
    }

    public class OrderItemViewModels
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string CustomerName { get; set; }
    }

    public class OrderRemoveViewModels
    {
        public int ProductId { get; set; }
        public List<OrderItemViewModels> ListOrderItem { get; set; }
    }
}
