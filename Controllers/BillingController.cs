using GundamShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GundamShop.Controllers
{
    [ShopAuthorize("Admin")]
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

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "MaKH,NgayDH,TriGia,DaGiao,NgayGiaoHang,TenNguoiNhan,DiaChiNhan,DienThoaiNhan")] DONDATHANG modBil, FormCollection form)//id==soDH
        {
            ViewBag.BillingDetails = GetBillingsDetails(id);
            if (ModelState.IsValid)
            {
                db.DONDATHANGs.DeleteOnSubmit(GetBillingByID(id));
                db.DONDATHANGs.InsertOnSubmit(modBil);
                db.SubmitChanges();
                return RedirectToAction("ShowBillings");
            }
            return View(GetBillingByID(id));
        }

        public ActionResult Delete(int? id)
        {
            return View(GetBillingByID(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var billing = GetBillingByID(id);
            var details = GetBillingsDetails(id);
            
            foreach (CTDATHANG ctdh in details)
            {
                db.CTDATHANGs.DeleteOnSubmit(ctdh);
            }
            db.DONDATHANGs.DeleteOnSubmit(billing);

            db.SubmitChanges();
            return RedirectToAction("ShowBillings");
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