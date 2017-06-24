using DCP.Filters;
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


        // GET: Product/Detail
        [CheckLogin]
        public ActionResult Detail(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var ctx = new QLDGEntities())
            {

                // lấy thông tin cho view title

                int CatID = ctx.Products
                    .Where(p => p.ProID == id)
                    .FirstOrDefault().CatID;

                string curCat = ctx.Categories
                    .Where(c => c.CatID == CatID)
                    .FirstOrDefault().CatName;

                ViewBag.Cat = curCat;

                // lấy thông tin sản phẩm

                var model = ctx.Products
                    .Where(p => p.ProID == id)
                    .FirstOrDefault();

                return View(model);
            }
        }


        // POST: Product/Detail
        [HttpPost]
        
        public ActionResult Detail (BuyVM newmodel)
        {
            // LƯU ĐẤU GIÁ VÀO CSDL

            AuctionHistory bid = new AuctionHistory
            {
                ProID = newmodel.ProID,
                UserID = newmodel.UserID,
                PriceBid = newmodel.money,
                AuctionTime = newmodel.time,
                AuctionStatus = true
            };

            // THAY ĐỔI THÔNG SỐ SẢN PHẨM

            using (var ctx = new QLDGEntities())
            {
                ctx.AuctionHistories.Add(bid);
                ctx.SaveChanges();

                Product model = ctx.Products
                    .Where(p => p.ProID == newmodel.ProID)
                    .FirstOrDefault();
                model.PriceHighest = newmodel.money;
                model.PriceCur = model.PriceCur + model.Step;
                model.Buyer = newmodel.UserID;
                ctx.SaveChanges();
            }
            ViewBag.SuccessMsg = "Bạn đã đặt giá thành công";
            return View();
        }
    }
}