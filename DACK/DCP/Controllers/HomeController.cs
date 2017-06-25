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


        // GET: Home/Top5Highest
        public ActionResult Top5Highest()
        {
            using (var ctx = new QLDGEntities())
            {
                int records = 5;
                var list = ctx.Products
                    .Where(p => p.ProStatus == true)
                    .OrderByDescending(p => p.PriceCur)
                    .Take(records)
                    .ToList();

                return PartialView("Top5Highest", list);
            }
            
        }

        //GET: Home/Top5Hot

        public ActionResult Top5Hot()
        {
            using (var ctx = new QLDGEntities())
            {
                
                var list = ctx.Products
                    .ToList();
                
                return PartialView("Top5Hot", list);
                
            }
        }
    }
}