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
            return View();
        }
        public ActionResult CustomerView()
        {
            int cus_id = int.Parse(Request.Cookies["CustomerID"].Value);
            var orders = db.Orders.Where(c => c.customer_id == cus_id);
            return View("Index", orders.ToList());
        }
        public ActionResult PendingOrder()
        {
            var orders = db.Orders.Where(c => c.order_status.Contains("Paid") || c.order_status.Contains("COD"));
            return View("Index", orders.ToList());
        }

        public ActionResult ConfirmedOrder()
        {
            var orders = db.Orders.Where(c => c.order_status.Contains("Confirmed"));
            return View("Index", orders.ToList());
        }
        public ActionResult RejectedOrder()
        {
            var orders = db.Orders.Where(c => c.order_status.Contains("Rejected"));
            return View("Index", orders.ToList());
        }
        public ActionResult AllOrders()
        {
            var orders = db.Orders.ToList();
            return View("Index", orders.ToList());
        }


        // GET: Orders/Details/5
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
            return View(order);
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
