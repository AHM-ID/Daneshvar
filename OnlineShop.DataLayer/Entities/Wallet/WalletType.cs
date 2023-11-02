using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.DataLayer.Entities.Wallet
{
    public class WalletType
    {
        public WalletType()
        {
            
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TypeID { get; set; }

        [Display(Name = "نوع کیف پول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد.")]
        public string TypeTitle { get; set; }

        #region Relations

        public virtual List<Wallet> Wallets { get; set; }

        #endregion
    }
}
