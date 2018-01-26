using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    [Authorize]
    public class ProductsAdminController : Controller
    {
        private WebDienThoaiEntities db = new WebDienThoaiEntities();

        
        // GET: ProductsAdmin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllProducts()
        {
            var products = db.Products.Include(p => p.Category).
                Include(p => p.Supplier).
                OrderByDescending(p => p.DateModified);
            return Json(from prod in products
                        select new
                        {
                            ProductId = prod.ProductId,
                            ProductName = prod.ProductName,
                            ProductImage = prod.ProductImage,
                            Price = prod.Price,
                            Quantity = prod.Quantity
                            
                        }, JsonRequestBehavior.AllowGet);
        }

        // GET: ProductsAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: ProductsAdmin/Create
        public ActionResult Create()
        {
            ViewBag.CateId = new SelectList(db.Categories, "CateId", "CateName");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "SupplierName");
            return View();
        }

        // POST: ProductsAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,SupplierId,CateId,Price,Quantity,TechnicalInfo,InformationDetail,Status")] Product product, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        string path = Path.Combine(Server.MapPath("~/FileUploads"), fileName);
                        upload.SaveAs(path);
                        product.ProductImage = "/FileUploads/" + fileName;
                    }
                    else
                    {
                        product.ProductImage = "/FileUploads/NoProduct.png";
                    }

                    product.DateCreated = DateTime.Now;
                    product.DateModified = DateTime.Now;
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {

                throw;
            }


            ViewBag.CateId = new SelectList(db.Categories, "CateId", "CateName", product.CateId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: ProductsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CateId = new SelectList(db.Categories, "CateId", "CateName", product.CateId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // POST: ProductsAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,SupplierId,CateId,Price,Quantity,TechnicalInfo,InformationDetail,Status")] Product product, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentProduct = db.Products.Find(product.ProductId);

                    if (upload != null && upload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(upload.FileName);
                        string path = Path.Combine(Server.MapPath("~/FileUploads"), fileName);
                        upload.SaveAs(path);
                        currentProduct.ProductImage = "/FileUploads/" + fileName;
                    }


                    currentProduct.ProductName = product.ProductName;
                    currentProduct.SupplierId = product.SupplierId;
                    currentProduct.CateId = product.CateId;
                    currentProduct.Price = product.Price;
                    currentProduct.Quantity = product.Quantity;
                    currentProduct.TechnicalInfo = product.TechnicalInfo;
                    currentProduct.InformationDetail = product.InformationDetail;
                    currentProduct.Status = product.Status;
                    currentProduct.DateModified = DateTime.Now;

                    db.Entry(currentProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {

                throw;
            }


            ViewBag.CateId = new SelectList(db.Categories, "CateId", "CateName", product.CateId);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: ProductsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
