using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCP.Helpers
{
    public class CurrentContext
    {

        public static bool IsLogged()
        {
            var flag = HttpContext.Current.Session["isLogin"];
            if (flag == null || (int)flag == 0)
            {
                return false;
            }

            return true;
        }

    }
}