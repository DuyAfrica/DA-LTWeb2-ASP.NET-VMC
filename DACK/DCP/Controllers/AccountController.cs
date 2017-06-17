using DCP.Helpers;
using DCP.Models;
using System;
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

        public ActionResult Register(RegisterVM model)
        {

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

            return View();
        }
    }
}