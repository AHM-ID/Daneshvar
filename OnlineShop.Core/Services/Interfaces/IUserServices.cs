using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using OnlineShop.Core.DTOs.Users;
using OnlineShop.DataLayer.Entities.User;
using OnlineShop.DataLayer.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Services.Interfaces
{
    public interface IUserServices
    {
        bool IsUserNameExist(string userName);
        bool IsEmailExist(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        bool ActiveAccount(string activationCode);
        User GetUserByEmail(string email);
        User GetUserByUserName(string userName);
        User GetUserByActivationCode(string activationCode);
        int GetUserIDByUserName(string userName);
        void UpdateUser(User user);

        #region User Panel

        UserInformationViewModel GetUserInformation(string userName);
        EditProfileViewModel GetUserForEditProfile(string userName);
        void EditProfile(string userName, EditProfileViewModel editProfile);
        bool CompareOldPassword(string userName, string oldPassword);
        void ChangePassword(string userName, string newPassword);

        #endregion

        #region Wallet

        List<WalletViewModel> GetWalletsUser(string userName);
        int ChargeWallet(string userName,int amount,string description,bool isPaid = false);
        int AddWallet(Wallet wallet);
        Wallet GetWalletByWalletID(int walletID);
        void UpdateWallet(Wallet wallet);
        void WalletBalance(string userName, int amount, int type, bool isPaid = false);

        #endregion

        #region Admin Panel

        UsersFormAdminViewModel GetUsers(int pageId = 1, string filterFullName = "", string filterUserName = "", string filterEmail = "");

        #endregion 
    }
}
