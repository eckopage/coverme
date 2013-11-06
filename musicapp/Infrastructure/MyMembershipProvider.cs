using musicapp.DAL;
using musicapp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace musicapp.Infrastructure
{
    public class MyMembershipProvider: ExtendedMembershipProvider
    {
        //wlasciwosci z Web.config:
        private string _ApplicationName;
        private int _MaxInvalidPasswordAttempts;
        private int _PasswordAttemptWindow;
        private int _MinRequiredNonalphanumericCharacters;
        private int _MinRequiredPasswordLength;
        private bool _EnablePasswordReset;
        private string _PasswordStrengthRegularExpression;
        private bool _EnablePasswordRetrieval;
        private bool _RequiresQuestionAndAnswer;
        private bool _RequiresUniqueEmail;
        private MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;


        //funkcja pomocnicza pobierajaca wartosci z pliku konfiguracyjnego:
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue)) return defaultValue;
            return configValue;
        }


        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null) throw new ArgumentNullException("config");

            if (name == null || name.Length == 0) name = "CustomMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Membership Provider");
            }

            base.Initialize(name, config);

            _ApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _MaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _PasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            _MinRequiredNonalphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _MinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _EnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _PasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            _EnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            _RequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            _RequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
        }


        public override string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword)) return false;

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

            using (DatabaseContext db = new DatabaseContext())
            {
                UsersRepository usersRepository = new UsersRepository(db);
                if (usersRepository.ChangePassword(username, newPassword))
                {
                    return true;
                }

                return false;
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(username, false);

            if (u == null)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    UsersRepository usersRepository = new UsersRepository(db);
                    usersRepository.CreateUser(username, password, email, false);
                }

                status = MembershipCreateStatus.Success;
                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }

            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { return _EnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _EnablePasswordRetrieval; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UsersRepository usersRepository = new UsersRepository(db);
                return usersRepository.GetUser(username);
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UsersRepository usersRepository = new UsersRepository(db);
                return usersRepository.GetUser(Convert.ToInt32(providerUserKey));
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UsersRepository usersRepository = new UsersRepository(db);
                return usersRepository.GetUserNameByEmail(email);
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _MaxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _MinRequiredNonalphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _MinRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _PasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _PasswordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _PasswordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _RequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _RequiresUniqueEmail; }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                UsersRepository usersRepository = new UsersRepository(db);
                return usersRepository.ValidateUser(username, password);
            }
        }

        public override bool ConfirmAccount(string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override bool ConfirmAccount(string userName, string accountConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override string CreateAccount(string userName, string password, bool requireConfirmationToken)
        {
            throw new NotImplementedException();
        }

        public override string CreateUserAndAccount(string userName, string password, bool requireConfirmation, IDictionary<string, object> values)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteAccount(string userName)
        {
            throw new NotImplementedException();
        }

        public override string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow)
        {
            throw new NotImplementedException();
        }

        public override ICollection<OAuthAccountData> GetAccountsForUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreateDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastPasswordFailureDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetPasswordChangedDate(string userName)
        {
            throw new NotImplementedException();
        }

        public override int GetPasswordFailuresSinceLastSuccess(string userName)
        {
            throw new NotImplementedException();
        }

        public override int GetUserIdFromPasswordResetToken(string token)
        {
            throw new NotImplementedException();
        }

        public override bool IsConfirmed(string userName)
        {
            throw new NotImplementedException();
        }

        public override bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}