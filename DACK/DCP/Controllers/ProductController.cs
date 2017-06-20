using DCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCP.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product/ByCat
        public ActionResult ByCat(int? id, int page = 1)
        {

            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var ctx = new QLDGEntities())
            {

                // truyền tên danh mục đang xem vào viewtitle

                string curCatName = ctx.Categories
                    .Where(c => c.CatID == id)
                    .FirstOrDefault().CatName;

                ViewBag.CatName = curCatName;

                // chia page

                int n = ctx.Products
                    .Where(p => p.CatID == id)
                    .Count();

                int recordsPerpage = 6;

                int nPages = n / recordsPerpage;

                int m = n % recordsPerpage;
                if (m > 0)
                {
                    nPages++;
                }

                ViewBag.Pages = nPages;

                ViewBag.CurPage = page;

           // lấy sản phẩm vào view

                var list = ctx.Products
                    .Where(p => p.CatID == id)
                    .OrderBy(p => p.ProID)
                    .Skip((page - 1) * recordsPerpage)
                    .Take(recordsPerpage)
                    .ToList();


                return View(list);
            }
        }
    }
}