using BotDetect.Web.Mvc;
using DCP.Helpers;
using DCP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCP.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }



        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }


        // POST: Account/Register
        [HttpPost]


        [CaptchaValidation("CaptchaCode", "ExampleCaptcha", "Incorrect CAPTCHA code!")]


        public ActionResult Register(RegisterVM model)
        {

            if (!ModelState.IsValid) {
                // TODO: Captcha validation failed, show error message

                ViewBag.ErrorMsg = "Sai mã Captcha";
            }
            else {
                // TODO: Captcha validation passed, proceed with protected action

                User u = new User
                {
                    f_Username = model.Username,
                    f_Password = StringUtils.Md5(model.RawPW),
                    f_Name = model.Name,
                    f_DOB = DateTime.ParseExact(model.DOB, "d/m/yyyy", null),
                    f_Email = model.Email,
                    f_Address = model.Address,
                    f_Level = 0,
                    f_Rate = 100
                };

                using (var ctx = new QLDGEntities())
                {
                    ctx.Users.Add(u);
                    ctx.SaveChanges();
                }
                ViewBag.SuccessMsg = "Bạn đã đăng kí thành công";
            }
            
            return View();
        }
    }
}