using musicapp.DAL.Interfaces;
using musicapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace musicapp.DAL.Repositories
{
    public class UsersRolesRepository: IUsersRolesRepository
    {
        private DatabaseContext context;

        public UsersRolesRepository(DatabaseContext context)
        {
            this.context = context;
        }


        public void AddRole(string roleName)
        {
            context.UsersRoles.Add(new UserRoles { RoleName = roleName });
            context.SaveChanges();
        }


        public IQueryable<UserRoles> GetAllRoles()
        {
            return context.UsersRoles.AsQueryable();
        }


        public UserRoles GetRoleByName(string roleName)
        {
            return context.UsersRoles.SingleOrDefault(x => x.RoleName == roleName);
        }


        public UserRoles GetRoleById(int roleId)
        {
            return context.UsersRoles.SingleOrDefault(x => x.RoleId == roleId);
        }


        public UserRoles GetUserRole(string userName)
        {
            var userRole = (from user in context.Users
                            join roles in context.UsersRoles on user.RoleId equals roles.RoleId
                            where user.UserName == userName
                            select roles).FirstOrDefault();

            return userRole;
        }


        public void DeleteRole(string roleName)
        {
            var role = context.UsersRoles.SingleOrDefault(x => x.RoleName == roleName);
            if (role != null)
            {
                context.UsersRoles.Remove(role);
                context.SaveChanges();
            }
        }


        public void AddUserToRole(int userId, int roleId)
        {
            Users user = (from u in context.Users where (u.UserId == userId) select u).FirstOrDefault();
            UserRoles role = (from r in context.UsersRoles where (r.RoleId == roleId) select r).FirstOrDefault();

            if (user != null && role != null)
            {
                user.RoleId = roleId;
                context.SaveChanges();
            }
        }


        public void AddUserToRole(string userName, string roleName)
        {
            Users user = (from u in context.Users where (u.UserName == userName) select u).FirstOrDefault();
            UserRoles role = (from r in context.UsersRoles where (r.RoleName == roleName) select r).FirstOrDefault();

            if (user != null && role != null)
            {
                user.RoleId = role.RoleId;
                context.SaveChanges();
            }
        }
    }
}