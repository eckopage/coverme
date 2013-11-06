using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using JQueryUIHelpers;
using Microsoft.Web.WebPages.OAuth;
using musicapp.DAL;
using musicapp.Models;
using WebMatrix.WebData;

namespace musicapp.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork UOW = new UnitOfWork();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AutoComplete(string phrazeAutomplete)
        {
            var query = UOW.VideoRepository.GetCoverFileNameAutoComplete(phrazeAutomplete);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}
