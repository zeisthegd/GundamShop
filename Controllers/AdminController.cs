using GundamShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GundamShop.Attributes;

namespace GundamShop.Controllers
{

    public class AdminController : Controller
    {
        dbQLShopGundamDataContext db = new dbQLShopGundamDataContext();
        
        public ActionResult ShowUsers()
        {
            return View(db.KHACHHANGs.OrderByDescending(x => x.MaKH));
        }

        public ActionResult Edit(int? id)
        {
            return View();
        }
    }
}