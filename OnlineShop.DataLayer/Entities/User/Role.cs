using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DataLayer.Entities.User
{
    public class Role
    {
        public Role()
        {
            
        }

        [Key]
        public int RoleID { get; set; }

        [Display(Name = "نقش کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر داشته باشد.")]
        public string? RoleTitle { get; set; }

        #region Relations
        public virtual List<UserRole> UserRole { get; set; }
        #endregion
    }
}
