using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace GundamShop.Models
{
    public class ShopAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public ShopAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var role = Convert.ToString(httpContext.Session["Role"]);
            if (role != null && allowedroles.Contains(role))
            {
                return true;
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary
            {
                { "controller", allowedroles.Contains("Admin") ? "Admin" : "GundamShop" },
                { "action", "Login" }
            });
        }
    }

}