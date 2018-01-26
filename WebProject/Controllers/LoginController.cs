using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class LoginController : Controller
    {
        WebDienThoaiEntities db = new WebDienThoaiEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string Password, string returnUrl)
        {
            var user = db.tblUsers.SingleOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                ViewBag.error = "Tên đăng nhập bị sai!";
            }
            else
            {
                if (user.Password == Password)
                {
                    var ident = new ClaimsIdentity(new[] { 
                          // adding following 2 claim just for supporting default antiforgery provider
                          new Claim(ClaimTypes.NameIdentifier, userName),
                          new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                          "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                          new Claim(ClaimTypes.Name,userName),}, DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties { IsPersistent = true }, ident);
                    return RedirectToLocal(returnUrl); // auth succeed 
                }
                else
                {
                    ViewBag.error = "Sai mật khẩu! Nhập lại";
                }
            }
            return View();


        }
        public ActionResult SessionLogOff()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();


            return RedirectToAction("Index", "Login");

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "ProductsAdmin");
        }

    }
}