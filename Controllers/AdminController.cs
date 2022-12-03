using GundamShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GundamShop.Attributes;
using System.Net;

namespace GundamShop.Controllers
{
    [ShopAuthorize("Admin")]
    public class AdminController : Controller
    {
        dbQLShopGundamDataContext data = new dbQLShopGundamDataContext();

        public ActionResult ShowAdmins()
        {
            return View(data.ADMINs.OrderByDescending(x => x.MaAdmin));
        }

        public ActionResult ShowUsers()
        {
            return View(data.KHACHHANGs.OrderByDescending(x => x.MaKH));
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(FormCollection collection)
        {
            var tenDN = collection["TenDangNhap"];
            var matKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tenDN))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập.";
            }
            else if (String.IsNullOrEmpty(matKhau))
            {
                ViewBag.Error = "Vui lòng nhập mật khẩu.";
            }
            else
            {
                ADMIN admin = data.ADMINs.SingleOrDefault(ad => ad.TenDNAdmin == tenDN && ad.MatKhauAdmin == matKhau);
                if (admin != null)
                {
                    ViewBag.ThongBaoSignIn = $"Đăng nhập thành công. Chào mừng {admin.TenDNAdmin}";
                    Session["Admin"] = admin;
                    Session["TenAdmin"] = admin.TenDNAdmin;
                    Session["Role"] = "Admin";
                    return RedirectToAction("ShowAdmins", "Admin");
                }
                else
                {
                    ViewBag.ThongBaoSignIn = $"Đăng nhập thất bại.";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "HoTenAdmin,DiaChiAdmin,DienThoaiAdmin,TenDNAdmin,MatKhauAdmin,NgaySinhAdmin,GioiTinhAdmin,EmailAdmin")] ADMIN newAdmin)
        {
            if (ModelState.IsValid)
            {
                ADMIN exstAd = data.ADMINs.SingleOrDefault(ad => ad.TenDNAdmin == newAdmin.TenDNAdmin);
                if (exstAd != null)
                {
                    ViewBag.Title = "Tên Đăng Nhập Đã Tồn Tại";
                    return View();
                }
                if (newAdmin != null)
                {
                    data.ADMINs.InsertOnSubmit(newAdmin);
                    data.SubmitChanges();
                    return RedirectToAction("ShowAdmins");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return View((ADMIN)data.ADMINs.Where(x => x.MaAdmin == id).ToList()[0]);
        }


        [HttpPost]
        public ActionResult Edit(int? id, ADMIN modAd, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                data.ADMINs.DeleteOnSubmit((ADMIN)data.ADMINs.Where(x => x.MaAdmin == id).ToList()[0]);
                data.ADMINs.InsertOnSubmit(modAd);
                data.SubmitChanges();
                return RedirectToAction("ShowAdmins");
            }
            return View(modAd);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADMIN ad = (ADMIN)data.ADMINs.Where(x => x.MaAdmin == id).ToList()[0];
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            ADMIN ad = (ADMIN)data.ADMINs.Where(x => x.MaAdmin == id).ToList()[0];
            data.ADMINs.DeleteOnSubmit(ad);
            data.SubmitChanges();
            return RedirectToAction("ShowAdmins");
        }
    }
}