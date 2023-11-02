using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.DTOs.Users
{
    public class WalletChargeViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Amount { get; set; }
    }

    public class WalletViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Amount { get; set; }

        [Display(Name = "شرح")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد.")]
        public string Description { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int TypeID { get; set; }

        [Display(Name = "زمان پرداخت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime PayDate { get; set; }
    }
}
