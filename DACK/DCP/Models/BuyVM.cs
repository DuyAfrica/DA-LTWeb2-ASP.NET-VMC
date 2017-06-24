using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCP.Models
{
    public class BuyVM
    {

        public int ProID { get; set; }
        public int UserID { get; set; }
        public decimal money { get; set; }
        public DateTime time { get; set; }
    }
}