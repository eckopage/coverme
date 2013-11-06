using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using musicapp.Models;
using musicapp.DAL;
using JQueryUIHelpers;

namespace musicapp.Controllers
{
    public class AccountController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();


        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult Manage() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    unitOfWork.UsersRepository.SetLastLoginDate(model.UserName);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nazwa użytkownika lub hasło są nieprawidłowe.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View();
        }


        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    //FormsAuthentication.SetAuthCookie(model.UserName, false); // createPersistentCookie
                    //return RedirectToAction("Index", "Home");

                    return RedirectToAction("RegistrationConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult RegistrationConfirmation()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View();
        }


        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }


        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true); //true - userIsOnline
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    TempData["tempMessage"] = "Dokonano pomyślnej zmiany hasła.";
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Obecne lub nowe hasło jest nieprawidłowe.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize]
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Użytkownik o takiej nazwie już istnieje, proszę wprowadzić inną nazwę.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Użytkownik z takim adresem email już istnieje, proszę wprowadzić inny adres email.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Wprowadzone hasło jest nieprawidłowe, proszę je poprawić.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Wprowadzony adres email jest nieprawidłowy, proszę go poprawić.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Odpowiedź odzyskiwania hasła jest nieprawidłowa, proszę ją poprawić.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Pytanie odzyskiwania hasła jest nieprawidłowe, proszę je poprawić.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Nazwa użytkownika jest nieprawidłowa, proszę ją poprawić.";

                case MembershipCreateStatus.ProviderError:
                    return "Podczas uwierzytelnienia wystąpił błąd, proszę spróbować ponownie.";

                case MembershipCreateStatus.UserRejected:
                    return "Podczas zakładania konta wystąpił błąd, proszę spróbować ponownie.";

                default:
                    return "Wystąpił nieznany błąd, proszę spróbować ponownie.";
            }
        }
        #endregion


        public ActionResult AccountActivation(int userId, string code)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (unitOfWork.UsersRepository.ActivateUser(userId, code) == false)
            {
                TempData["tempMessage"] = "Link jest nieprawidłowy, proszę ponownie kliknąć na linka w emailu.";
                return View();
            }
            else
            {
                FormsAuthentication.SetAuthCookie(unitOfWork.UsersRepository.GetUserByUserId(userId).UserName, false);
                return RedirectToAction("Welcome");
            }
        }


        public ActionResult ResetPassword()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (String.IsNullOrEmpty(model.UserName) && String.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("", "Login lub email jest wymagany.");
            }
            else if (unitOfWork.UsersRepository.GetUserByUserName(model.UserName) == null && unitOfWork.UsersRepository.GetUserByEmail(model.Email) == null)
            {
                ModelState.AddModelError("login", "Brak użytkownika o takim loginie.");
                ModelState.AddModelError("email", "Brak użytkownika o takim emailu.");
            }
            if (!ModelState.IsValid) return View();

            Users user = null;
            if (unitOfWork.UsersRepository.GetUserByUserName(model.UserName) != null)
            {
                user = unitOfWork.UsersRepository.GetUserByUserName(model.UserName);
            }
            else if (unitOfWork.UsersRepository.GetUserByEmail(model.Email) != null)
            {
                user = unitOfWork.UsersRepository.GetUserByEmail(model.Email);
            }

            unitOfWork.UsersRepository.ResetPassword(user.UserId);

            TempData["tempMessage"] = "Sprawdź pocztę by zmienić hasło.";
            return RedirectToAction("Index", "Home");
        }


        public ActionResult VerifyPasswordResetCode(int userId, string code)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

            if (string.IsNullOrEmpty(code))
            {
                TempData["tempMessage"] = "Link jest nieprawidłowy, proszę ponownie kliknąć na linka w emailu.";
                return View();
            }
            else
            {
                if (unitOfWork.UsersRepository.CheckPasswordResetCode(userId, code) == false)
                {
                    TempData["tempMessage"] = "Link jest nieprawidłowy lub hasło zostało już zmienione.";
                    return View();
                }
                else
                {
                    TempData["tempMessage"] = "Hasło zostało zmienione, możesz się teraz zalogować używając nowego hasła wysłanego na Twój adres email.";
                    return RedirectToAction("Login");
                }
            }
        }


        [Authorize]
        public ActionResult ChangeEmail()
        {
            MembershipUser currentUser = Membership.GetUser(); //pobranie zalogowanego uzytkownika

            ChangeEmailModel changeEmailModel = new ChangeEmailModel();
            changeEmailModel.Email = currentUser.Email;

            return View(changeEmailModel);
        }


        [Authorize]
        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = unitOfWork.UsersRepository.GetUserByUserName(Membership.GetUser().UserName);

                if (Membership.ValidateUser(user.UserName, model.Password))
                {
                    //modyfikacja adresu email:
                    unitOfWork.UsersRepository.ChangeEmail(user.UserName, model.Email);

                    TempData["tempMessage"] = "Dokonano pomyślnej zmiany emaila.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Obecne hasło jest nieprawidłowe.");
                }
            }

            return View(model);
        }


        //metoda wzywana automatycznie na koniec pracy kontrolera w celu zamkniecia polaczenia z baza danych:
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }

    }
}
