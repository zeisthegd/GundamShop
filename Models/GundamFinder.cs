using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GundamShop.Models
{
    public class GundamFinder
    {
        dbQLShopGundamDataContext db;
        public GundamFinder() { db = new dbQLShopGundamDataContext(); }

        public GUNDAM GetGundamByID(int? id)
        {
            var result = from x in db.GUNDAMs where x.MaGundam == id select x;
            return result.ToList()[0];
        }
        public List<GUNDAM> GetGundamsByName(string name, int count = 0)
        {
            count = count == 0 ? db.GUNDAMs.Count() : count;
            return db.GUNDAMs.ToList().FindAll(x => x.TenGundam.Contains(name));
        }

        public List<GUNDAM> GetGundamsByManuDate(int count = 0)
        {
            count = count == 0 ? db.GUNDAMs.Count() : count;
            return db.GUNDAMs.OrderByDescending(sp => sp.NgaySanXuat).Take(count).ToList();
        }

        public List<GUNDAM> GetGundamsByLuotXem(int count = 0)
        {
            count = count == 0 ? db.GUNDAMs.Count() : count;
            return db.GUNDAMs.OrderByDescending(sp => sp.SoLanXem).Take(count).ToList();
        }

        public List<GUNDAM> GetGundamsBySoLuongBan(int count = 0)
        {
            count = count == 0 ? db.GUNDAMs.Count() : count;
            return db.GUNDAMs.OrderByDescending(sp => sp.SoLuongBan).Take(count).ToList();
        }

        public List<GUNDAM> GetGundamsByLevel(int? maCD, int count = 0)
        {
            count = count == 0 ? db.GUNDAMs.Count() : count;
            return db.GUNDAMs.Where(sp => sp.MaCD == maCD).Take(count).ToList();
        }
    }
}