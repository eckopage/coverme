using musicapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace musicapp.DAL.Interfaces
{
    public interface IUsersRepository
    {
        MembershipUser CreateUser(string userName, string password, string email, bool isCreatedByAdmin);
        string GetUserNameByEmail(string email);
        MembershipUser GetUser(string userName);
        MembershipUser GetUser(int userId);
        Users GetUserByUserId(int userId);
        Users GetUserByUserName(string userName);
        Users GetUserByEmail(string email);
        bool ValidateUser(string userName, string password);
        bool ActivateUser(int userId, string code);
        void ResetPassword(int userId);
        bool CheckPasswordResetCode(int userId, string code);
        void SetLastLoginDate(string userName);
        bool ChangePassword(string userName, string newPassword);
        bool ChangeEmail(string userName, string email);
        int GetNumberOfUsersByEmail(string email);
    }
}
