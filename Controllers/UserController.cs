using GundamShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GundamShop.Controllers
{
    public class UserController : Controller
    {
        dbQLShopGundamDataContext data = new dbQLShopGundamDataContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
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
                KHACHHANG khach = data.KHACHHANGs.SingleOrDefault(kh => kh.TenDN == tenDN && kh.MatKhau == matKhau);
                if (khach != null)
                {
                    ViewBag.ThongBaoSignIn = $"Đăng nhập thành công. Chào mừng {khach.HoTenKH}";
                    Session["KhachHang"] = khach;
                    Session["TenKhachHang"] = khach.HoTenKH;   
                    return RedirectToAction("ShowUsers", "Admin");
                }
                else
                {
                    ViewBag.ThongBaoSignIn = $"Đăng nhập thất bại.";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection, KHACHHANG kh)
        {
            var hoTen = collection["HoTen"];
            var tenDN = collection["TenDangNhap"];
            var matKhau = collection["MatKhau"];
            var diaChi = collection["DiaChi"];
            var email = collection["Email"];
            var sdt = collection["DienThoai"];
            var ngaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(hoTen))
            {
                ViewBag.Error = "Vui lòng nhập tên khách hàng.";
            }
            else if (String.IsNullOrEmpty(matKhau))
            {
                ViewBag.Error = "Vui lòng nhập mật khẩu.";
            }
            else if (String.IsNullOrEmpty(tenDN))
            {
                ViewBag.Error = "Vui lòng nhập tên đăng nhập.";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Vui lòng nhập email.";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewBag.Error = "Vui lòng nhập số điện thoại.";
            }
            else if (String.IsNullOrEmpty(diaChi))
            {
                ViewBag.Error = "Vui lòng nhập địa chỉ.";
            }
            else
            {
                kh.HoTenKH = hoTen;
                kh.TenDN = tenDN;
                kh.MatKhau = matKhau;
                kh.Email = email;
                kh.NgaySinh = DateTime.Parse(ngaySinh);
                kh.DienThoaiKH = sdt;
                kh.DiaChiKH = diaChi;
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.Login();
        }



    }
}