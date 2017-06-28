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
                    .Where(p => p.ProStatus == true)
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
            using (var ctx = new QLDGEntities())
            {
                var abc = ctx.AuctionHistories
                    .Where(a => a.ProID == newmodel.ProID).FirstOrDefault();
                
                if (abc != null)
                {
                    decimal highest = ctx.AuctionHistories
                    .Where(a => a.ProID == newmodel.ProID)
                    .OrderByDescending(a => a.PriceBid)
                    .FirstOrDefault().PriceBid;



                    if (newmodel.money < highest)
                    {
                        // LẤY THÔNG TIN TITLE

                        int CatID = ctx.Products
                        .Where(p => p.ProID == newmodel.ProID)
                        .FirstOrDefault().CatID;

                        string curCat = ctx.Categories
                            .Where(c => c.CatID == CatID)
                            .FirstOrDefault().CatName;

                        ViewBag.Cat = curCat;


                        // TỰ ĐỘNG ĐẤU GIÁ 
                        
                        // Đấu giá mới
                              // Lấy id người đang giữ giá cao nhất
                        int PreID = ctx.AuctionHistories
                            .Where(a => a.ProID == newmodel.ProID)
                            .OrderByDescending(a => a.PriceBid)
                            .FirstOrDefault().UserID;
                            
                               // Lấy giá đặt của người dùng giữ giá
                        decimal PrePriceBid = ctx.AuctionHistories
                            .Where(a => a.ProID == newmodel.ProID)
                            .OrderByDescending(a => a.PriceBid)
                            .FirstOrDefault().PriceBid;

                               // Tgian đặt giá của người giữ giá cao nhất
                        DateTime time = ctx.AuctionHistories
                            .Where(a => a.ProID == newmodel.ProID)
                            .OrderByDescending(a => a.PriceBid)
                            .FirstOrDefault().AuctionTime;

                                // lấy bước giá sản phẩm
                        decimal step = ctx.Products
                            .Where(a => a.ProID == newmodel.ProID)
                            .FirstOrDefault().Step;

                        // Lưu đấu giá
                            // người đấu giá mới thấp hơn cao nhất
                        AuctionHistory bid = new AuctionHistory
                        {
                            ProID = newmodel.ProID,
                            UserID = newmodel.UserID,
                            PriceBid = newmodel.money,
                            PriceCur = newmodel.money,
                            AuctionTime = newmodel.time,
                            AuctionStatus = true
                        };

                        ctx.AuctionHistories.Add(bid);


                            // tự động đấu giá cho người giữ giá
                        AuctionHistory bid2 = new AuctionHistory
                        {
                            ProID = newmodel.ProID,
                            UserID = PreID,
                            PriceCur = newmodel.money + step,
                            PriceBid = PrePriceBid,
                            AuctionTime = time,
                            AuctionStatus = true
                        };

                        
                        ctx.AuctionHistories.Add(bid2);
                        ctx.SaveChanges();

                        // THAY ĐỔI THÔNG TIN SẢN PHẨM

                        Product model = ctx.Products
                            .Where(p => p.ProID == newmodel.ProID)
                            .FirstOrDefault();

                        model.PriceCur = newmodel.money + model.Step;
                        ctx.SaveChanges();

                        // lấy sản phẩm cho view

                        var latestmodel = ctx.Products
                        .Where(p => p.ProID == newmodel.ProID)
                        .FirstOrDefault();

                        ViewBag.ErrorMsg = "Bạn đã đặt giá THÀNH CÔNG, NHƯNG giá bạn đặt chưa phải là giá cao nhất";

                        return View(latestmodel);
                    }


                    else if (newmodel.money == highest)
                    {
                        // Lấy id người đang giữ giá cao nhất
                        int PreID = ctx.AuctionHistories
                            .Where(a => a.ProID == newmodel.ProID)
                            .OrderByDescending(a => a.PriceBid)
                            .FirstOrDefault().UserID;

                        // Tgian đặt giá của người giữ giá cao nhất
                        DateTime time = ctx.AuctionHistories
                            .Where(a => a.ProID == newmodel.ProID)
                            .OrderByDescending(a => a.PriceBid)
                            .FirstOrDefault().AuctionTime;

                        // TỰ ĐỘNG ĐẤU GIÁ

                        AuctionHistory bid = new AuctionHistory
                        {
                            ProID = newmodel.ProID,
                            UserID = newmodel.UserID,
                            PriceBid = newmodel.money,
                            PriceCur = newmodel.money,
                            AuctionTime = newmodel.time,
                            AuctionStatus = true
                        };

                        ctx.AuctionHistories.Add(bid);

                        AuctionHistory bid2 = new AuctionHistory
                        {
                            ProID = newmodel.ProID,
                            UserID = PreID,
                            PriceBid = newmodel.money,
                            PriceCur = newmodel.money,
                            AuctionTime = time,
                            AuctionStatus = true
                        };
                        ctx.AuctionHistories.Add(bid2);
                        ctx.SaveChanges();


                        // THAY ĐỔI THÔNG TIN SẢN PHẨM

                        Product model = ctx.Products
                            .Where(p => p.ProID == newmodel.ProID)
                            .FirstOrDefault();

                        model.PriceCur = model.PriceHighest;
                        ctx.SaveChanges();

                        // Lấy thông tin title

                        int CatID = ctx.Products
                        .Where(p => p.ProID == newmodel.ProID)
                        .FirstOrDefault().CatID;

                        string curCat = ctx.Categories
                            .Where(c => c.CatID == CatID)
                            .FirstOrDefault().CatName;

                        ViewBag.Cat = curCat;

                        // Lấy sản phẩm cho view

                        var latestmodel = ctx.Products
                        .Where(p => p.ProID == newmodel.ProID)
                        .FirstOrDefault();

                        ViewBag.ErrorMsg = "Bạn đã đặt giá THÀNH CÔNG, NHƯNG giá bạn đặt chưa phải là giá cao nhất";

                        return View(latestmodel);
                    }


                    else
                    {
                        // lấy bước giá sản phẩm
                        decimal step = ctx.Products
                            .Where(a => a.ProID == newmodel.ProID)
                            .FirstOrDefault().Step;
                        // lấy giá hiện tại của sản phẩm
                        decimal cur = ctx.Products
                            .Where(a => a.ProID == newmodel.ProID)
                            .FirstOrDefault().PriceCur;

                        // LƯU ĐẤU GIÁ VÀO CSDL

                        AuctionHistory bid = new AuctionHistory
                        {
                            ProID = newmodel.ProID,
                            UserID = newmodel.UserID,
                            PriceBid = newmodel.money,
                            PriceCur = cur + step,
                            AuctionTime = newmodel.time,
                            AuctionStatus = true
                        };

                        ctx.AuctionHistories.Add(bid);
                        ctx.SaveChanges();

                        // THAY ĐỔI THÔNG TIN SẢN PHẨM

                        Product model = ctx.Products
                            .Where(p => p.ProID == newmodel.ProID)
                            .FirstOrDefault();
                        model.PriceHighest = newmodel.money;
                        model.PriceCur = model.PriceCur + model.Step;
                        model.Buyer = newmodel.UserID;

                        ctx.SaveChanges();

                        // Lấy thông tin title

                        int CatID = ctx.Products
                        .Where(p => p.ProID == newmodel.ProID)
                        .FirstOrDefault().CatID;

                        string curCat = ctx.Categories
                            .Where(c => c.CatID == CatID)
                            .FirstOrDefault().CatName;

                        ViewBag.Cat = curCat;

                        //Lấy sản phẩm cho view

                        var latestmodel = ctx.Products
                        .Where(p => p.ProID == newmodel.ProID)
                        .FirstOrDefault();

                        ViewBag.SuccessMsg = "Bạn đã đặt giá thành công";

                        return View(latestmodel);
                    }
                }
                else
                {
                    // lấy bước giá sản phẩm
                    decimal step = ctx.Products
                        .Where(a => a.ProID == newmodel.ProID)
                        .FirstOrDefault().Step;
                    // lấy giá hiện tại của sản phẩm
                    decimal cur = ctx.Products
                        .Where(a => a.ProID == newmodel.ProID)
                        .FirstOrDefault().PriceCur;


                    // LƯU ĐẤU GIÁ VÀO CSDL

                    AuctionHistory bid = new AuctionHistory
                    {
                        ProID = newmodel.ProID,
                        UserID = newmodel.UserID,
                        PriceBid = newmodel.money,
                        PriceCur = cur + step,
                        AuctionTime = newmodel.time,
                        AuctionStatus = true
                    };

                    ctx.AuctionHistories.Add(bid);
                    ctx.SaveChanges();


                    // THAY ĐỔI THÔNG TIN SẢN PHẨM

                    Product model = ctx.Products
                        .Where(p => p.ProID == newmodel.ProID)
                        .FirstOrDefault();
                    model.PriceHighest = newmodel.money;
                    model.PriceCur = model.PriceCur + model.Step;
                    model.Buyer = newmodel.UserID;

                    ctx.SaveChanges();

                    // Lấy thông tin title

                    int CatID = ctx.Products
                    .Where(p => p.ProID == newmodel.ProID)
                    .FirstOrDefault().CatID;

                    string curCat = ctx.Categories
                        .Where(c => c.CatID == CatID)
                        .FirstOrDefault().CatName;

                    ViewBag.Cat = curCat;
                    
                    //Lấy sản phẩm cho view

                    var latestmodel = ctx.Products
                    .Where(p => p.ProID == newmodel.ProID)
                    .FirstOrDefault();

                    ViewBag.SuccessMsg = "Bạn đã đặt giá thành công";
                    return View(latestmodel);
                }
            }  
        }
    }
}