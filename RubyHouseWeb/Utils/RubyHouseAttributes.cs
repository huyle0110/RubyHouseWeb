using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RubyHouseWeb.Utils
{
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        private bool isCheckLogin { get; set; }
        public CheckLoginAttribute(bool val)
        {
            isCheckLogin = val;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (RubyHouseSession.GetUserInfo() == null)
            {
                filterContext.Result = new RedirectResult("~/login");
                return;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }

}