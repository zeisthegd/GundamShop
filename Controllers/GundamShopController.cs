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
        public ActionResult Contact()
        {
            return View();
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

        public ActionResult FeaturedProducts()
        {
            List<GUNDAM> result = new List<GUNDAM>();
            ViewBag.ListCapDo = db.CAPDOs.ToList();
            foreach(CAPDO capDo in db.CAPDOs.ToList())
            {
                result.AddRange(gFinder.GetGundamsByLevel(capDo.MaCD, 100).OrderByDescending(sp => sp.NgaySanXuat).OrderByDescending(gd => gd.SoLuongBan).Take(3));
            }
            return PartialView(result);
        }

        public ActionResult ListCapDo()
        {
            var capDo = from cd in db.CAPDOs select cd;
            return PartialView(capDo);
        }

        public ActionResult Details(int? id)
        {
            GUNDAM gundam = gFinder.GetGundamByID(id);
            return gundam != null ? View(gundam) : View();
        }

    }
}