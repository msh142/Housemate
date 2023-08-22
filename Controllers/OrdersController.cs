using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Housemate.Models;

namespace Housemate.Controllers
{
    public class OrdersController : Controller
    {
        private hmdbEntities db = new hmdbEntities();

        // GET: Orders
        public ActionResult Index()
        {
            
            if (Request.Cookies["AdminID"] != null)
            {
                return View(db.Orders.ToList());
            }
            if (Request.Cookies["CustomerID"] != null)
            {
                int cus_id = Convert.ToInt32(Request.Cookies["CustomerID"].Value);
                IEnumerable<Order> order = db.Orders.Where(c => c.customer_id == cus_id && c.order_status != "Cancelled").ToList();
                return View(order.ToList());
            }
            return View("AllOrders");
        }
        public ActionResult CustomerView()
        {
            int cus_id = int.Parse(Request.Cookies["CustomerID"].Value);
            var orders = db.Orders.Where(c => c.customer_id == cus_id);
            return View("Index", orders.ToList());
        }
        public ActionResult PendingOrder()
        {
            if (Request.Cookies["AdminID"] != null)
            {
                return View("Index", db.Orders.Where(c => c.order_status.Contains("Paid") || c.order_status.Contains("COD")).ToList());
            }
            if (Request.Cookies["CustomerID"] != null)
            {
                int cus_id = Convert.ToInt32(Request.Cookies["CustomerID"].Value);
                IEnumerable<Order> order = db.Orders.Where(c => c.customer_id == cus_id && (c.order_status.Contains("Paid") || c.order_status.Contains("COD"))).ToList();
                return View("Index", order);
            }
            return View("Index");
        }

        public ActionResult ConfirmedOrder()
        {
            if (Request.Cookies["AdminID"] != null)
            {
                return View("Index", db.Orders.Where(c => c.order_status.Contains("Confirmed")).ToList());
            }
            if (Request.Cookies["CustomerID"] != null)
            {
                int cus_id = Convert.ToInt32(Request.Cookies["CustomerID"].Value);
                IEnumerable<Order> order = db.Orders.Where(c => c.customer_id == cus_id && c.order_status.Contains("Confirmed")).ToList();
                return View("Index", order);
            }
            return View("Index");
        }

        public ActionResult RejectedOrder()
        {
            return View("Index", db.Orders.Where(c => c.order_status.Contains("Rejected")).ToList());
        }
        public ActionResult CancelledOrder()
        {
            return View("Index", db.Orders.Where(c => c.order_status.Contains("Cancelled")).ToList());
        }
        public ActionResult AllOrders()
        {
            var orders = db.Orders.ToList();
            return View("Index", orders.ToList());
        }

        public ActionResult ConfirmOrder(int? id)
        {
            Order order = db.Orders.Find(id);
            order.order_status = "Delivering";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return View("Index");
        }
        public ActionResult RejectOrder(int? id)
        {
            Order order = db.Orders.Find(id);
            order.order_status = "Rejected";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return View("Index");
        }
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            ViewBag.customerin = db.CustomerInfoes.Where(c => c.customer_id == order.customer_id).SingleOrDefault();
            List<CartRecord> carR = db.CartRecords.Where(c => c.cart_id == order.cart_id && c.status == "Processing" && c.order_id == order.order_id).ToList();
            ViewBag.carR = carR;
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        public ActionResult CancelOrder(int? id)
        {
            Order order = db.Orders.Find(id);
            order.order_status = "Cancelled";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult OrderResponse(string response)
        {
            return RedirectToAction("Index");
        }
        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username");
            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "order_id,customer_id,order_status,feedback,order_date,cart_id")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", order.customer_id);
            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id", order.cart_id);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", order.customer_id);
            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id", order.cart_id);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "order_id,customer_id,order_status,feedback,order_date,cart_id")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", order.customer_id);
            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id", order.cart_id);
            return View(order);
        }

        // GET: Orders/Delete/5
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

        // POST: Orders/Delete/5
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
