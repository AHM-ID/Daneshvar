using OnlineShop.Core.Convertors;
using OnlineShop.Core.DTOs.Users;
using OnlineShop.Core.Generators;
using OnlineShop.Core.Security;
using OnlineShop.Core.Services.Interfaces;
using OnlineShop.DataLayer.Context;
using OnlineShop.DataLayer.Entities.User;
using OnlineShop.DataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Services
{
    public class UserServices : IUserServices
    {
        OnlineShopContext _context;
        public UserServices(OnlineShopContext context)
        {
            _context = context;
        }

        public bool ActiveAccount(string activationCode)
        {
            var user = _context.Users.SingleOrDefault(u => u.ActivationCode == activationCode);
            if(user == null || user.IsActive)
                return false;

            user.IsActive = true;
            user.ActivationCode = UserCodeGenerator.UniqueCodeGenerator();
            _context.SaveChanges();
            return true;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserID;
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletID;
        }

        public void ChangePassword(string userName, string newPassword)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordSecurity.HashPassword(newPassword);
            UpdateUser(user);
        }

        public int ChargeWallet(string userName, int amount,string description, bool isPaid = false)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                PayDate = DateTime.Now,
                Description = description,
                IsPaid = isPaid,
                TypeID = 1,
                UserID = GetUserIDByUserName(userName)
            };

           return AddWallet(wallet);
        }

        public bool CompareOldPassword(string userName, string oldPassword)
        {
            string hashOldPassword = PasswordSecurity.HashPassword(oldPassword);

            return _context.Users.Any(u => u.UserName == userName && u.Password == hashOldPassword);
        }

        public void EditProfile(string userName, EditProfileViewModel editProfile)
        {
            if(editProfile.UserAvatar != null)
            {
                string path = "";
                if(editProfile.Avatar != "Male.png" &&  editProfile.Avatar != "Female.png")
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editProfile.Avatar);
                    if(File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                editProfile.Avatar = UserCodeGenerator.UniqueCodeGenerator() + Path.GetExtension(editProfile.UserAvatar.FileName);
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editProfile.Avatar);
                using(var stream = new FileStream(path, FileMode.Create))
                {
                    editProfile.UserAvatar.CopyTo(stream);
                }
            }

            var user = GetUserByUserName(userName);
            user.FullName = editProfile.FullName;
            user.UserName = editProfile.UserName;
            user.Email = editProfile.Email;
            if ((user.Gender != editProfile.Gender) && (user.UserAvatar == editProfile.Avatar))
                user.UserAvatar = $"{editProfile.Gender}.png";
            else
                user.UserAvatar = editProfile.Avatar;

            user.Gender = editProfile.Gender;

            UpdateUser(user);
        }

        public User GetUserByActivationCode(string activationCode)
        {
            return _context.Users.SingleOrDefault(u => u.ActivationCode == activationCode);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public EditProfileViewModel GetUserForEditProfile(string userName)
        {
            return _context.Users.Where(u => u.UserName == userName).Select(u => new EditProfileViewModel (){ 
                FullName = u.FullName,
                UserName = u.UserName,
                Email = u.Email,
                Gender = u.Gender,
                Avatar = u.UserAvatar
            }).Single();
        }

        public int GetUserIDByUserName(string userName)
        {
            return _context.Users.Single(u => u.UserName == userName).UserID;
        }

        public UserInformationViewModel GetUserInformation(string userName)
        {
            // TODO Wallet Balance Should Update With Every Transaction
            var user = GetUserByUserName(userName);
            UserInformationViewModel information = new UserInformationViewModel()
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                UserAvatar = user.UserAvatar,
                Wallet = user.WalletBalance
            };

            return information;
        }

        public UsersFormAdminViewModel GetUsers(int pageId = 1, string filterFullName = "", string filterUserName = "", string filterEmail = "")
        {
            IQueryable<User> result = _context.Users;

            if (!string.IsNullOrEmpty(filterFullName))
            {
                result = result.Where(u => u.FullName.Contains(filterFullName));
            }

            else if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }

            else if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            // Show Item in Page
            int take = 10;
            int skip = (pageId - 1) * take;

            UsersFormAdminViewModel list = new UsersFormAdminViewModel()
            {
                Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList(),
                CurrentPage = pageId,
                PageCount = result.Count() / take
            };
            if (result.Count() % take != 0 && list.PageCount >= 1)
                list.PageCount++;

            return list;
            
        }

        public Wallet GetWalletByWalletID(int walletID)
        {
            return _context.Wallets.Find(walletID);
        }

        public List<WalletViewModel> GetWalletsUser(string userName)
        {
            var userId = GetUserIDByUserName(userName);

            return _context.Wallets
                .Where(w => w.IsPaid && w.UserID == userId)
                .Select(w => new WalletViewModel
                {
                    Amount = w.Amount,
                    Description = w.Description,
                    PayDate = w.PayDate,
                    TypeID = w.TypeID
                })
                .ToList();
        }

        public bool IsEmailExist(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool IsUserNameExist(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public User LoginUser(LoginViewModel login)
        {
            return _context.Users.SingleOrDefault(u => u.Email == StringExtensions.EmailFix(login.Email) && u.Password == PasswordSecurity.HashPassword(login.Password));
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public void UpdateWallet(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }

        public void WalletBalance(string userName, int amount, int type, bool isPaid = false)
        {
            if (isPaid)
            {
                var user = GetUserByUserName(userName);
                if (type == 1)
                    user.WalletBalance += amount;
                else
                    user.WalletBalance -= amount;

                UpdateUser(user);
            }
        }
    }
}
