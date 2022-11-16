using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GundamShop.Attributes
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string redirectUrl = "https://localhost:44347/User/Login";
        public RoleAuthorizeAttribute() : base()
        {
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }

        public void IsUserAuthorized(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewDataDictionary dic = new ViewDataDictionary();
                dic.Add("Message", "You didn't login.");

                var result = new RedirectResult(redirectUrl);
                filterContext.Result = result;
            }
        }
    }
}