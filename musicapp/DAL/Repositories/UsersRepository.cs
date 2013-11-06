using musicapp.DAL.Interfaces;
using musicapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace musicapp.DAL.Repositories
{
    public class UsersRepository:IUsersRepository
    {
        private DatabaseContext context;

        public UsersRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public MembershipUser CreateUser(string userName, string password, string email, bool isCreatedByAdmin)
        {
            Users user = new Users();

            user.UserName = userName;
            user.Email = email;
            user.PasswordSalt = CreateSalt();
            user.Password = CreatePasswordHash(password, user.PasswordSalt);
            user.CreatedDate = DateTime.Now;
            user.IsActivated = false;
            if (isCreatedByAdmin) user.IsActivated = true;
            user.AccountActivationCode = GenerateKey();
            user.RoleId = 1; //1 to id roli, np. o nazwie 'administrator', ktora wkrotce dodamy

            context.Users.Add(user);
            context.SaveChanges();

            //tutaj mozemy dodac jeszcze wysylanie emaila z kodem aktywacyjnym do uzytkownika

            return GetUser(userName);
        }


        public string GetUserNameByEmail(string email)
        {
            var result = from u in context.Users where (u.Email == email) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.FirstOrDefault();
                return dbuser.UserName;
            }
            else
            {
                return "";
            }
        }


        public MembershipUser GetUser(string userName)
        {
            var result = from u in context.Users where (u.UserName == userName) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.FirstOrDefault();

                string _userName = dbuser.UserName;
                int _providerUserKey = dbuser.UserId;
                string _email = dbuser.Email;
                string _passwordQuestion = "";
                string _comment = "";
                bool _isApproved = dbuser.IsActivated;
                bool _isLockedOut = false;
                DateTime _creationDate = dbuser.CreatedDate;
                DateTime _lastLoginDate = dbuser.LastLoginDate ?? DateTime.Now;
                DateTime _lastActivityDate = DateTime.Now;
                DateTime _lastPasswordChangedDate = DateTime.Now;
                DateTime _lastLockedOutDate = DateTime.Now;

                MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                            _userName,
                                                            _providerUserKey,
                                                            _email,
                                                            _passwordQuestion,
                                                            _comment,
                                                            _isApproved,
                                                            _isLockedOut,
                                                            _creationDate,
                                                            _lastLoginDate,
                                                            _lastActivityDate,
                                                            _lastPasswordChangedDate,
                                                            _lastLockedOutDate);

                return user;
            }
            else
            {
                return null;
            }
        }


        public MembershipUser GetUser(int userId)
        {
            var result = from u in context.Users where (u.UserId == userId) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.FirstOrDefault();

                string _userName = dbuser.UserName;
                int _providerUserKey = dbuser.UserId;
                string _email = dbuser.Email;
                string _passwordQuestion = "";
                string _comment = "";
                bool _isApproved = dbuser.IsActivated;
                bool _isLockedOut = false;
                DateTime _creationDate = dbuser.CreatedDate;
                DateTime _lastLoginDate = dbuser.LastLoginDate ?? DateTime.Now;
                DateTime _lastActivityDate = DateTime.Now;
                DateTime _lastPasswordChangedDate = DateTime.Now;
                DateTime _lastLockedOutDate = DateTime.Now;

                MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                            _userName,
                                                            _providerUserKey,
                                                            _email,
                                                            _passwordQuestion,
                                                            _comment,
                                                            _isApproved,
                                                            _isLockedOut,
                                                            _creationDate,
                                                            _lastLoginDate,
                                                            _lastActivityDate,
                                                            _lastPasswordChangedDate,
                                                            _lastLockedOutDate);

                return user;
            }
            else
            {
                return null;
            }
        }


        public Users GetUserByUserId(int userId)
        {
            var user = from u in context.Users where (u.UserId == userId) select u;

            return user.FirstOrDefault();
        }


        public Users GetUserByUserName(string userName)
        {
            var user = from u in context.Users where (u.UserName == userName) select u;

            return user.FirstOrDefault();
        }


        public Users GetUserByEmail(string email)
        {
            var user = from u in context.Users where (u.Email == email) select u;

            return user.FirstOrDefault();
        }


        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }


        private static string CreatePasswordHash(string password, string salt)
        {
            string saltAndPassword = String.Concat(password, salt);
            Byte[] bytes = Encoding.UTF8.GetBytes(saltAndPassword);
            Byte[] hashedBytes = SHA1.Create().ComputeHash(bytes);
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "");

            return hashedPassword;
        }


        public bool ValidateUser(string userName, string password)
        {
            var result = from u in context.Users where (u.UserName == userName) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                // POWINNO BYC DODANE IF IS ACTIVATED !!!
                //if (dbuser.Password == CreatePasswordHash(password, dbuser.PasswordSalt) && dbuser.IsActivated == true) return true;
                if (dbuser.Password == CreatePasswordHash(password, dbuser.PasswordSalt)) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }


        private static string GenerateKey()
        {
            Guid emailKey = Guid.NewGuid();

            return emailKey.ToString();
        }


        public bool ActivateUser(int userId, string code)
        {
            var result = from u in context.Users where (u.UserId == userId && u.IsActivated == false) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                if (dbuser.AccountActivationCode == code)
                {
                    dbuser.IsActivated = true;
                    dbuser.LastModifiedDate = DateTime.Now;
                    dbuser.AccountActivationCode = null;

                    context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public void ResetPassword(int userId)
        {
            var result = from u in context.Users where (u.UserId == userId) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                dbuser.LastModifiedDate = DateTime.Now;
                dbuser.PasswordResetCode = GenerateKey();

                context.SaveChanges();

                //tutaj mozna dodac wysylanie emaila z kodem do resetu hasla
            }
        }


        public bool CheckPasswordResetCode(int userId, string code)
        {
            var result = from u in context.Users where (u.UserId == userId) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                if (dbuser.PasswordResetCode == code)
                {
                    string password = RandomString(6);

                    dbuser.LastModifiedDate = DateTime.Now;
                    dbuser.PasswordResetCode = null;
                    dbuser.PasswordSalt = CreateSalt();
                    dbuser.Password = CreatePasswordHash(password, dbuser.PasswordSalt);

                    context.SaveChanges();

                    //tutaj mozna dodac wysylanie emaila z nowym haslem po resecie hasla

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        private static string RandomString(int size)
        {
            string allowedCharacters = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                int x = random.Next(0, allowedCharacters.Length);

                builder.Append(allowedCharacters[x]);
            }

            return builder.ToString();
        }


        public void SetLastLoginDate(string userName)
        {
            var result = from u in context.Users where (u.UserName == userName) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                dbuser.LastLoginDate = DateTime.Now;

                context.SaveChanges();
            }
        }


        public bool ChangePassword(string userName, string newPassword)
        {
            var result = from u in context.Users where (u.UserName == userName) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                dbuser.LastModifiedDate = DateTime.Now;
                dbuser.PasswordSalt = CreateSalt();
                dbuser.Password = CreatePasswordHash(newPassword, dbuser.PasswordSalt);

                context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public bool ChangeEmail(string userName, string email)
        {
            var result = from u in context.Users where (u.UserName == userName) select u;

            if (result.Count() != 0)
            {
                var dbuser = result.First();

                dbuser.LastModifiedDate = DateTime.Now;
                dbuser.Email = email;

                context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }


        public int GetNumberOfUsersByEmail(string email)
        {
            int count = (from x in context.Users
                         where x.Email == email
                         select x).Count();

            return count;
        }

    }
}