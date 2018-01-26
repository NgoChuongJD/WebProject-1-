using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{

    public class HomeController : Controller
    {
        WebDienThoaiEntities db = new WebDienThoaiEntities();
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Login()
        {
            if (Session["TaiKhoan"] != null)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password) {
            var customer = db.Customers.Where(c => c.Email == Email).SingleOrDefault();
            if (customer != null)
            {
                if (customer.Password == Password) {
                    //dang nhap thanh cong
                    Session["TaiKhoan"] = customer.CustId;
                    return RedirectToAction("ThanhToan","GioHang");
                }
            }
            return View();
        }


    }
}