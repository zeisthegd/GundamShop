using GundamShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GundamShop.Views.GundamShop
{
    public class StorageController : Controller
    {
        dbQLShopGundamDataContext db = new dbQLShopGundamDataContext();
        public ActionResult ListStorage()
        {
            return View(GetGundams(100));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CAPDOs.ToList(), "MaCD", "TenCapDo");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "TenGundam,DonGia,MoTa,HinhMinhHoa,MaCD,NgaySanXuat,SoLuongBan,SoLanXem,SoLuongConLai")] GUNDAM newGD, HttpPostedFileBase HinhMinhHoa)
        {
            ViewBag.MaCD = new SelectList(db.CAPDOs.ToList(), "MaCD", "TenCapDo");
            if (ModelState.IsValid)
            {
                if (newGD != null)
                {
                    if (HinhMinhHoa.ContentLength > 0)
                    {
                        newGD.HinhMinhHoa = SaveImage(HinhMinhHoa);
                    }
                    db.GUNDAMs.InsertOnSubmit(newGD);
                    db.SubmitChanges();
                    return RedirectToAction("ListStorage");


                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var gundam = GetGundamByID(id);
            if (gundam == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCD = new SelectList(db.CAPDOs.ToList(), "MaCD", "TenCapDo");

            return View(gundam);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, [Bind(Include = "TenGundam,DonGia,MoTa,HinhMinhHoa,MaCD,NgaySanXuat,SoLuongBan,SoLanXem,SoLuongConLai")] GUNDAM modGD, HttpPostedFileBase HinhMinhHoa)
        {
            if (ModelState.IsValid)
            {
                if (HinhMinhHoa.ContentLength > 0)
                {
                    modGD.HinhMinhHoa = SaveImage(HinhMinhHoa);
                }
                db.GUNDAMs.DeleteOnSubmit(GetGundamByID(id));
                db.GUNDAMs.InsertOnSubmit(modGD);
                db.SubmitChanges();
            }
            return RedirectToAction("ListStorage");
            ViewBag.MaCD = new SelectList(db.CAPDOs.ToList(), "MaCD", "TenCapDo");
            return View(modGD);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GUNDAM gundam = GetGundamByID(id);
            if (gundam == null)
            {
                return HttpNotFound();
            }
            return View(gundam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var gundam = GetGundamByID(id);
            db.GUNDAMs.DeleteOnSubmit(gundam);
            db.SubmitChanges();
            return RedirectToAction("ListStorage");
        }

        public List<GUNDAM> GetGundams(int count = 0)
        {
            return db.GUNDAMs.Take(count).ToList();
        }

        public GUNDAM GetGundamByID(int? id)
        {
            var result = from x in db.GUNDAMs where x.MaGundam == id select x;
            return result.ToList()[0];
        }

        public string SaveImage(HttpPostedFileBase HinhMinhHoa)
        {
            string fileName = Path.GetFileName(HinhMinhHoa.FileName);
            string path = Path.Combine(Server.MapPath("~/Storage/HinhMinhHoa/Gundam"), fileName);
            HinhMinhHoa.SaveAs(path);
            return fileName;
        }
    }
}