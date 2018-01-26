using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    [Authorize]
    public class OrdersAdminController : Controller
    {
        private WebDienThoaiEntities db = new WebDienThoaiEntities();

        // GET: OrdersAdmin
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).OrderByDescending(o=>o.OrderDate);
            return View(orders.ToList());
        }

        // GET: OrdersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            List<OrderDetail> listOrderDetails = db.OrderDetails.Where(od => od.OrderId == id).Include(p=>p.Product).ToList();
            if (listOrderDetails == null)
            {
                return HttpNotFound();
            }

            ViewBag.OrderDetails = listOrderDetails;

            //chuyen trang thai don hang tu 0 sang 1
            if (order.Status == 0)
            {
                order.Status = 1;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return View(order);
        }


        [HttpPost]
        public ActionResult XuLyDonHang(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.Status = 2;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index","OrdersAdmin");
        }

        // GET: OrdersAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
