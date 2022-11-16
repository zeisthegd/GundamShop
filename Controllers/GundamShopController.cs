using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GundamShop.Models;

namespace GundamShop.Controllers
{
    public class GundamShopController : Controller
    {
        dbQLShopGundamDataContext db = new dbQLShopGundamDataContext();

        public ActionResult Index()
        {
            return View(GetGundams(5));
        }
        public ActionResult FullShopContent()
        {
            return View(GetGundams(5));
        }

        public List<GUNDAM> GetGundams(int count = 0)
        {
            return db.GUNDAMs.OrderByDescending(sp => sp.NgaySanXuat).Take(count).ToList();
        }

        public ActionResult ListCapDo()
        {
            var capDo = from cd in db.CAPDOs select cd;
            return PartialView(capDo);
        }

        public ActionResult SPTheoCapDo(int? id)
        {
            var spTheoCD = from sp in db.GUNDAMs where sp.MaCD == id select sp;
            return View();
        }

        public ActionResult Details(int? id)
        {
            GUNDAM gundam = GetGundamByID(id);
            return gundam != null ? View(gundam) : View();
        }

        public GUNDAM GetGundamByID(int? id)
        {
            var result = from x in db.GUNDAMs where x.MaGundam == id select x;
            return result.ToList()[0];
        }
    }
}