using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GundamShop.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        public ActionResult Header()
        {
            return PartialView();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        public ActionResult AdminNavigation()
        {
            return PartialView();
        }

        public ActionResult Breadcrumb()
        {
            return PartialView();
        }
    }
}