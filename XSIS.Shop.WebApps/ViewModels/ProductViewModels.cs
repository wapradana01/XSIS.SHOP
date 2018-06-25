using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.WebApps.ViewModels
{
    public class ProductViewModels
    {
        public int Id { get; set; }

        [Display(Name = "Nama Produk")]
        [Required(ErrorMessage = "Nama Produk Harus Diisi")]
        [StringLength(50)]
        public string ProductName { get; set; }

        public int SupplierId { get; set; }

        [Display(Name = "Nama Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Harga Satuan")]
        public Nullable<decimal> UnitPrice { get; set; }

        [Display(Name = "Jumlah Produk")]
        [StringLength(30)]
        public string Package { get; set; }

        public bool IsDiscontinued { get; set; }
    }
}