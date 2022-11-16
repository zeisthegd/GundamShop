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

        public ActionResult ViewCart()
        {
            List<Cart> lstCart = GetCart();
            if(lstCart.Count==0)
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