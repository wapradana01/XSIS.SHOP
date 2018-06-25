using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.ViewModels
{
    public class SupplierViewModels
    {
        public int Id { get; set; }

        [Display(Name = "Nama Perusahaan")]
        [Required(ErrorMessage = "Nama Perusahaan harus diisi")]
        [StringLength(40)]
        public string CompanyName { get; set; }

        [Display(Name = "Nama Kontak")]
        [StringLength(50)]
        public string ContactName { get; set; }

        [Display(Name = "Gelar")]
        [StringLength(40)]
        public string ContactTitle { get; set; }

        [Display(Name = "Kota")]
        [StringLength(40)]
        public string City { get; set; }

        [Display(Name = "Negara")]
        [StringLength(40)]
        public string Country { get; set; }

        [Display(Name = "Telepon")]
        [StringLength(30)]
        public string Phone { get; set; }

        [Display(Name = "Fax")]
        [StringLength(30)]
        public string Fax { get; set; }

        [Display(Name = "Nama Produk")]
        public string ProductName { get; set; }

        [Display(Name = "Harga Satuan")]
        public Nullable<decimal> UnitPrice { get; set; }

        [Display(Name = "Jumlah Produk")]
        public string Package { get; set; }

        public bool IsDiscontinued { get; set; }

        public List<ProductViewModels> listproduk { get; set; }
    }
}
