using DCP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCP.Filters
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {

        public int RequiredPermission { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CurrentContext.IsLogged() == false)
            {
                filterContext.Result =
                    new RedirectResult("~/Account/Login");

                return;
            }

            if (CurrentContext.GetCurUser().f_Level < this.RequiredPermission)
            {
                filterContext.Result =
                    new RedirectResult("~/Error/Index");

                return;
            }

 	        base.OnActionExecuting(filterContext);
        }
    }
}