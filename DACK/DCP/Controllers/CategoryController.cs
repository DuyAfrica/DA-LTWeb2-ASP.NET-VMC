using DCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCP.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category/List
        public ActionResult List()
        {
            using (var ctx = new QLDGEntities())
            {
                var list = ctx.Categories.ToList();

                return PartialView("ListPartial", list);
            }
        }
    }
}