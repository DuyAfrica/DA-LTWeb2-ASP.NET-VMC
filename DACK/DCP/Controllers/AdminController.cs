using DCP.Filters;
using DCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DCP.Controllers
{
    [CheckLogin(RequiredPermission = 2)]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (var ctx = new QLDGEntities())
            {
                var list = ctx.Categories.ToList();

                return View(list);
            }
        }


        //
        // GET: /Category/Add
        public ActionResult Add()
        {
            return View();
        }


        //
        // POST: /Category/Add

        [HttpPost]
        public ActionResult Add(Category model)
        {
            using (var ctx = new QLDGEntities())
            {
                ctx.Categories.Add(model);
                ctx.SaveChanges();
            }
            return View();
        }


        //
        // GET: /Category/Delete
        public ActionResult Delete(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.ID = id;
            return View();
        }

        //
        // POST: /Category/Delete

        [HttpPost]
        public ActionResult Delete(int idToDelete)
        {
            using (var ctx = new QLDGEntities())
            {
                Category model = ctx.Categories
                    .Where(c => c.CatID == idToDelete)
                    .FirstOrDefault();

                ctx.Categories.Remove(model);
                ctx.SaveChanges();
            }

            return RedirectToAction("Index", "Admin");
        }


        //
        // GET: /Category/Update

        public ActionResult Update(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.ID = id;
            return View();
        }

        //
        // POST: /Category/Update

        [HttpPost]

        public ActionResult Update(Category newmodel, int idToUpdate)
        {
            using (var ctx = new QLDGEntities())
            {
                Category model = ctx.Categories.Where(c => c.CatID == idToUpdate)
                    .FirstOrDefault();
                model.CatName = newmodel.CatName;
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}