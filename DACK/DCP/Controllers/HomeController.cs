using DCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCP.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        // GET: Home/Top5Hot
        public ActionResult Top5Highest()
        {
            using (var ctx = new QLDGEntities())
            {
                int records = 5;
                var list = ctx.Products
                    .OrderByDescending(p => p.PriceCur)
                    .Take(records)
                    .ToList();

                return PartialView("Top5Highest", list);
            }
            
        }
    }
}