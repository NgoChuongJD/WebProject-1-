using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class SuppliersController : Controller
    {
        WebDienThoaiEntities db = new WebDienThoaiEntities();
        // GET: Suppliers
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult MenuSupplier()
        {
            return PartialView(db.Suppliers.ToList());

        }

        

    }
}