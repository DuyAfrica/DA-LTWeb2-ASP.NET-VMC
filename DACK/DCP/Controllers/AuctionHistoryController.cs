using DCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCP.Controllers
{
    public class AuctionHistoryController : Controller
    {
        // GET: AuctionHistory/List
        public ActionResult List(int ?id)
        {
            using (var ctx = new QLDGEntities())
            {

                var list = ctx.AuctionHistories
                    .Where(a => a.ProID == id)
                    .ToList();
                var list2 = ctx.Products
                    .Where(a => a.ProID == id)
                    .ToList();
                return PartialView("ListPartial", list);
            }
            
        }
    }
}