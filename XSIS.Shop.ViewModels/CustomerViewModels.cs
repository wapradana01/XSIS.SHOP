using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XSIS.Shop.ViewModels
{
    public class CustomerViewModels
    {
        public int Id { get; set; }
        [Display(Name = "Nama Depan")]
        [Required(ErrorMessage = "Nama Pertama Harus Isi")]
        [StringLength(40)]
        public string FirstName { get; set; }

        [Display(Name = "Nama Belakang")]
        [Required(ErrorMessage = "Nama Pertama Harus Isi")]
        [StringLength(40)]
        public string LastName { get; set; }

        [Display(Name = "Kota")]
        [StringLength(40)]
        public string City { get; set; }

        [Display(Name = "Negara")]
        [StringLength(40)]
        public string Country { get; set; }

        [Display(Name = "Phone")]
        [StringLength(20)]
        [RegularExpression("^[-()0-9 ]*$", ErrorMessage = "Maaf format telepon salah, no hanya boleh mengandung (, ), -, dan angka")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [StringLength(35)]
        public string Email { get; set; }

        public string FullName { get; set; }
    }
}
