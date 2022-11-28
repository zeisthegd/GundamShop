using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GundamShop.Models;
using PagedList;

namespace GundamShop.Controllers
{
    public class GundamShopController : Controller
    {
        dbQLShopGundamDataContext db = new dbQLShopGundamDataContext();

        private GundamFinder gFinder = new GundamFinder();

        public ActionResult Index()
        {
            return View(gFinder.GetGundamsByManuDate(5));
        }
        public ActionResult FullShopContent(int? page, string searchString, int? maCD)
        {

            if (page == null) page = 1;
            int pageSize = 9;

            int pageNumber = (page ?? 1);
            List<GUNDAM> lstProd = gFinder.GetGundamsByManuDate(100);
            if (searchString != null)
            {
                lstProd = gFinder.GetGundamsByName(searchString);
            }
            else if (maCD != null)
            {
                lstProd = gFinder.GetGundamsByLevel(maCD);
            }
            else
            {
                lstProd = gFinder.GetGundamsByManuDate(100);
            }
            ViewBag.TotalProducts = lstProd.Count;

            ViewBag.CurrentPage = "Cửa Hàng";
            return View(lstProd.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult LatestProducts()
        {
            return PartialView(gFinder.GetGundamsByManuDate(9));
        }

        public ActionResult BestSellerProducts()
        {
            return PartialView(gFinder.GetGundamsBySoLuongBan(9));
        }

        public ActionResult MostWatchedProducts()
        {
            return PartialView(gFinder.GetGundamsByLuotXem(9));
        }


        public ActionResult ListCapDo()
        {
            var capDo = from cd in db.CAPDOs select cd;
            return PartialView(capDo);
        }

        public ActionResult SPTheoCapDo(int? maCD, int? page)
        {
            if (page == null) page = 1;
            var lstProd = gFinder.GetGundamsByLevel(maCD);
            int pageSize = 9;

            int pageNumber = (page ?? 1);
            ViewBag.TotalProducts = lstProd.Count;

            return View(lstProd.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            GUNDAM gundam = gFinder.GetGundamByID(id);
            return gundam != null ? View(gundam) : View();
        }

    }
}