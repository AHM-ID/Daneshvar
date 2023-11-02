using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DataLayer.Entities.User
{
    public class UserRole
    {
        public UserRole()
        {

        }

        [Key]
        public int UserRoleID { get; set; }


        public int UserID { get; set; }


        public int RoleID { get; set; }

        #region Relations
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
        #endregion
    }
}
