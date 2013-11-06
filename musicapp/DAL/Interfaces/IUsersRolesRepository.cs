using musicapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musicapp.DAL.Interfaces
{
    public interface IUsersRolesRepository
    {
        void AddRole(string roleName);
        IQueryable<UserRoles> GetAllRoles();
        UserRoles GetRoleByName(string roleName);
        UserRoles GetRoleById(int roleId);
        UserRoles GetUserRole(string userName);
        void DeleteRole(string roleName);
        void AddUserToRole(int userId, int roleId);
    }
}
