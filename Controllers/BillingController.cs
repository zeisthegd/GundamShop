using GundamShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GundamShop.Controllers
{
    public class BillingController : Controller
    {
        private dbQLShopGundamDataContext db = new dbQLShopGundamDataContext();
        private GundamFinder gFinder = new GundamFinder();

        public ActionResult ShowBillings()
        {
            return View(GetBillingsByAddedDate());
        }

        public ActionResult Edit(int? id)//id==soDH
        {
            ViewBag.BillingDetails = GetBillingsDetails(id);
            return View(GetBillingByID(id));
        }

        public ActionResult Delete(int? id)
        {
            return View(GetBillingByID(id));
        }

        public List<CTDATHANG> GetBillingsDetails(int? id)
        {
            return (from x in db.CTDATHANGs where x.SoDH == id select x).ToList();
        }

        public List<DONDATHANG> GetBillingsByAddedDate(int count = 0)
        {
            count = count == 0 ? db.DONDATHANGs.Count() : count;
            return db.DONDATHANGs.OrderByDescending(sp => sp.NgayDH).Take(count).ToList();
        }

        public DONDATHANG GetBillingByID(int? id)
        {
            var result = from x in db.DONDATHANGs where x.SoDH == id select x;
            return result.ToList()[0];
        }
    }
}