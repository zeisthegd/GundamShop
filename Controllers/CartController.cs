using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GundamShop.Models;

namespace GundamShop.Controllers
{
    public class CartController : Controller
    {
        private dbQLShopGundamDataContext data = new dbQLShopGundamDataContext();

        private List<Cart> checkedOutCart = new List<Cart>();
        public ActionResult ViewCart()
        {
            List<Cart> lstCart = GetCart();
            if (lstCart.Count == 0)
            {
                return View();
            }
            ViewBag.TotalAmount = GetTotalAmountInCart();
            ViewBag.TotalCost = GetTotalCostInCart();
            return View(lstCart);
        }

        public ActionResult CartButton()
        {
            ViewBag.TotalAmount = GetTotalAmountInCart();
            ViewBag.TotalCost = GetTotalCostInCart();
            return PartialView();
        }

        public List<Cart> GetCart()
        {
            List<Cart> lstCart = Session["GioHang"] as List<Cart>;
            if (lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["GioHang"] = lstCart;
            }
            return lstCart;
        }

        public ActionResult AddCart(int maGundam, string rqUrl)
        {
            List<Cart> lstCart = GetCart();
            Cart cartItem = lstCart.Find(g => g.MaGundam == maGundam);
            if (cartItem == null)
            {
                cartItem = new Cart(maGundam);
                lstCart.Add(cartItem);
            }
            else
            {
                cartItem.SoLuong++;
            }
            return RedirectToAction("ViewCart");
        }

        public ActionResult RemoveCart(int maGundam, string rqUrl)
        {
            List<Cart> lstCart = GetCart();
            Cart cartItem = lstCart.Find(g => g.MaGundam == maGundam);
            if (cartItem != null)
            {
                lstCart.Remove(cartItem);
            }
            return RedirectToAction("ViewCart");
        }

        public ActionResult UpdateCart(FormCollection form)
        {
            List<Cart> lstCart = GetCart();
            foreach (Cart cart in lstCart)
            {
                if (form[$"prod_count_{cart.MaGundam}"] != null)
                {
                    cart.SoLuong = int.Parse(form[$"prod_count_{cart.MaGundam}"]);
                }
            }
            return RedirectToAction("ViewCart");
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            if (Session["KhachHang"] == null || (string)Session["TenKhachHang"] == "")
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("FullShopContent", "GundamShop");
            }

            List<Cart> lstCart = GetCart();
            ViewBag.TotalAmount = GetTotalAmountInCart();
            ViewBag.TotalCost = GetTotalCostInCart();

            return View(lstCart);
        }

        [HttpPost]
        public ActionResult Checkout(FormCollection form)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = Session["KhachHang"] as KHACHHANG;
            List<Cart> lstCart = GetCart();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            ddh.NgayGiaoHang = DateTime.Today.Add(new TimeSpan(7));
            ddh.DaGiao = false;
            ddh.HTGiaoHang = false;
            ddh.HTThanhToan = false;
            ddh.TriGia = (decimal)GetTotalCostInCart();
            ddh.TenNguoiNhan = "Chưa Nhận";
            ddh.DiaChiNhan = "Chưa Nhận";
            ddh.DienThoaiNhan = "Chưa Nhận";
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (Cart item in lstCart)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaGundam = item.MaGundam;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = (decimal)item.DonGia;
                ctdh.ThanhTien = (decimal)item.ThanhTien;
                data.CTDATHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            return RedirectToAction("ConfirmCheckout", "Cart");
        }

        public ActionResult ConfirmCheckout()
        {
            checkedOutCart = new List<Cart>(GetCart());
            Session["GioHang"] = null;
            return View(checkedOutCart);
        }

        private int GetTotalAmountInCart()
        {
            int totalInCart = 0;
            List<Cart> lstCart = Session["GioHang"] as List<Cart>;
            if (lstCart != null)
                totalInCart = lstCart.Sum(item => item.SoLuong);
            return totalInCart;
        }

        private double GetTotalCostInCart()
        {
            double costOfCart = 0;
            List<Cart> lstCart = Session["GioHang"] as List<Cart>;
            if (lstCart != null)
                costOfCart = lstCart.Sum(item => item.ThanhTien);
            return costOfCart;
        }
    }
}