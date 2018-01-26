using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebProject.Models;

namespace WebProject.Controllers
{
    public class CategoriesController : Controller
    {
        WebDienThoaiEntities db = new WebDienThoaiEntities();
        // GET: Categories
        public ActionResult Index(int? cateId)
        {
            if (cateId.HasValue)
            {
                return View(db.Products.Where(p => p.CateId == cateId).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
       
        public PartialViewResult MenuCategories()
        {
            return PartialView(db.Categories.ToList());
        }
    }
}