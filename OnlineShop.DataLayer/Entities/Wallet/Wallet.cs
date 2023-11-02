using OnlineShop.DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DataLayer.Entities.Wallet
{
    public class Wallet
    {
        public Wallet()
        {
            
        }

        [Key]
        public int WalletID { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int TypeID { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int UserID { get; set; }

        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int Amount { get; set; }

        [Display(Name = "شرح")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد.")]
        public string Description { get; set; }

        [Display(Name = "پرداخت شده؟")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsPaid { get; set; }

        [Display(Name = "زمان پرداخت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public DateTime PayDate { get; set; }

        #region Relations

        public virtual User.User Users { get; set; }
        [ForeignKey(nameof(TypeID))]
        public virtual WalletType WalletType { get; set; }

        #endregion
    }
}
