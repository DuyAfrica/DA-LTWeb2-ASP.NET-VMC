using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCP.Helpers
{
    public class timeSpan
    {

        public static TimeSpan Time(System.DateTime date1, System.DateTime date2)
        {
            TimeSpan ts = date2.Subtract(date1);
            return ts;
        }
    }
}