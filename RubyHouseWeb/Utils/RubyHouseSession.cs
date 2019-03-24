using RubyHouseWeb.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RubyHouseWeb.Utils
{
    public class RubyHouseSession
    {
        public static UserSession GetUserInfo()
        {
            var data = HttpContext.Current.Session[typeof(UserSession).ToString()] as UserSession;
            if (data != null) return data;
            return null;
        }

        public static bool IsLogin()
        {
            return GetUserInfo() != null;
        }
    }
}