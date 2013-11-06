using musicapp.DAL;
using musicapp.DAL.Repositories;
using musicapp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace musicapp.Infrastructure
{
    public class MyRoleProvider: RoleProvider
    {
        //wlasciwosci z web.config:
        private string _ApplicationName;


        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException("config");

            if (name == null || name.Length == 0) name = "CustomRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Role Provider");
            }

            base.Initialize(name, config);


            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                _ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                _ApplicationName = config["applicationName"];
            }
        }


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UsersRolesRepository usersRolesRepository = new UsersRolesRepository(db);
                UserRoles role = usersRolesRepository.GetUserRole(username);

                if (role == null) return new string[] { string.Empty };

                return new string[] { role.RoleName };
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UsersRepository usersRepository = new UsersRepository(db);
                UsersRolesRepository usersRolesRepository = new UsersRolesRepository(db);

                Users user = usersRepository.GetUserByUserName(username);
                UserRoles role = usersRolesRepository.GetRoleByName(roleName);

                if (user == null) return false;
                if (role == null) return false;

                return user.UsersRoles.RoleName == role.RoleName;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}