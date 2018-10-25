using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quiz.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AdminAuthorize : AuthorizeAttribute
    {
        //override the method that is called if the user is not correctly authorized
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if the user managed to log in correctly (i.e., is authenticated), then the problem is authorization
            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //set TempData name-value pair so that the View can give the user a message so they understand why
                //they need to log in again
                filterContext.Controller.TempData.Add("RedirectReason", "Unauthorized");
            }
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}