using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace musicapp.Controllers
{
    public class artistController : Controller
    {
        //
        // GET: /artist/

        public ActionResult Index()
        {
            return View();
        }

    }
}
