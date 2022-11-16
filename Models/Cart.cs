using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GundamShop.Models
{
    public class Cart
    {
        private dbQLShopGundamDataContext db = new dbQLShopGundamDataContext();
        public int MaGundam { get; set; }
        public string TenGundam { get; set; }
        public string HinhMinhHoa { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien
        {
            get { return SoLuong * (int)DonGia; }
        }

        public Cart(int maGundam)
        {
            MaGundam = maGundam;
            GUNDAM gd = db.GUNDAMs.Single(g => g.MaGundam == MaGundam);
            TenGundam = gd.TenGundam;
            HinhMinhHoa = gd.HinhMinhHoa;
            DonGia = (double)gd.DonGia;
            SoLuong = 1;
        }
    }
}