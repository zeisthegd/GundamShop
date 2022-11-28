using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

public class MoneyHelper
{
    public static string FormatMoney(decimal? moneyDec)
    {
        if (moneyDec.HasValue)
            return FormatMoney(moneyDec.Value);
        else return String.Format(CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", 0);
    }
    public static string FormatMoney(decimal moneyDec)
    {
        CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
        return String.Format(cul, "{0:c0}", moneyDec);
    }

    public static string FormatMoney(double moneyDB)
    {
        CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
        return String.Format(cul, "{0:c0}", moneyDB);
    }
}