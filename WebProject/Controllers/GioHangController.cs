using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class GioHangController : Controller
    {
        WebDienThoaiEntities db = new WebDienThoaiEntities();
        // giỏ hàng mặc định, nếu session = null thì hiện không có hàng trong giỏ, nếu có thì trả lại list các sản phẩm
        public ActionResult Index()
        {
            ViewBag.Title = "Giỏ hàng";
            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart)Session["Cart"];
            return View(model);
        }

        // thêm vào giỏ hàng 1 sản phẩm có id = id của sản phẩm
        public ActionResult ThemVaoGioHang(int id)
        {
            var P = db.Products.Single(s => s.ProductId == id);
            if (P != null)
            {
                ShoppingCart objCart = (ShoppingCart)Session["Cart"];
                if (objCart == null)
                {
                    objCart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem()
                {
                    ProductName = P.ProductName,
                    ProductId = P.ProductId,
                    Price = P.Price,
                    Quantity = 1,
                    Total = P.Price
                };
                objCart.AddToCart(item);
                Session["Cart"] = objCart;
                return RedirectToAction("Index", "GioHang");
            }
            return RedirectToAction("Index", "GioHang");
        }

        // cập nhật giỏ hàng theo loại sản phẩm và số lượng
        public ActionResult UpdateQuantity(int proID, int quantity)
        {

            ShoppingCart objCart = (ShoppingCart)Session["Cart"];
            if (objCart != null)
            {
                objCart.UpdateQuantity(proID, quantity);
                Session["Cart"] = objCart;
            }
            return RedirectToAction("Index");
        }

        // xóa sản phẩm có id trong giỏ hàng đã có sẵn
        public ActionResult XoaSanPham(int id)
        {
            ShoppingCart objCart = (ShoppingCart)Session["Cart"];
            if (objCart != null)
            {
                objCart.RemoveFromCart(id);
                Session["Cart"] = objCart;
            }
            return RedirectToAction("Index");
        }

        //Thanh Toan
        public ActionResult ThanhToan()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart)Session["Cart"];

            int custId = int.Parse(Session["TaiKhoan"].ToString());
            var customer = db.Customers.Find(custId);
            ViewBag.customer = customer;
            return View(model);
        }

        [HttpPost]
        public ActionResult ThanhToan(string ShipAddress, string ShipPhone)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            int custId = int.Parse(Session["TaiKhoan"].ToString());
            var customer = db.Customers.Find(custId);
            Order order = new Order();
            order.CustId = custId;
            order.OrderDate = DateTime.Now;
            order.Status = 0;
            if (ShipAddress == null || ShipAddress == "")
            {
                order.ShipAddress = customer.Address;
            }
            else
            {
                order.ShipAddress = ShipAddress;
            }
            if (ShipPhone == null || ShipPhone == "")
            {
                order.ShipPhone = customer.PhoneNumber;
            }
            else
            {
                order.ShipPhone = ShipPhone;
            }

            //luu vao bang order
            db.Orders.Add(order);

            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart)Session["Cart"];
            foreach (var item in model.Cart.ListItem)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.OrderId;
                orderDetail.ProductId = item.ProductId;
                orderDetail.Quantity = item.Quantity;
                orderDetail.Price = item.Price;
                db.OrderDetails.Add(orderDetail);
            }
            db.SaveChanges();
            Session["Cart"] = null;
            TempData["msg"] = "Thành công";
            return RedirectToAction("Index","GioHang");
        }

        public ActionResult ThanhToanThanhCong() {

            return View();
        }
    }
}