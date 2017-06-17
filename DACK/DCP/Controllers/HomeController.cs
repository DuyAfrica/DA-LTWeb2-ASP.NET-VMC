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
        public ActionResult Top5Hot()
        {
            using (var ctxx = new QLDGEntities())
            {
                return View();
            }
        }
    }
}