using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arsha.Controllers
{
    public class HomeController : Controller
    {
        private bool IsAuth
        {
            get
            {
                return Session["IsAuth"] == null ? false : (bool)Session["IsAuth"];
            }
        }
        // GET: Home
        public ActionResult Index()
        {
            if (IsAuth)
                return View();
            else
                return RedirectToAction("Login", "Account");
        }
    }
}