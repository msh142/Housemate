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
    public class CartsController : Controller
    {
        private hmdbEntities db = new hmdbEntities();

        // GET: Carts
        public ActionResult Index()
        {
            int customerID = 0;
            if (Request.Cookies["CustomerID"].Value != null)
            {
                string customerIDValue = Request.Cookies["CustomerID"].Value;
                customerID = Convert.ToInt32(customerIDValue);
            }
            var carts = (from c in db.Carts
                         where (c.customer_id == customerID)
                         select c);

            return View(carts.ToList());
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int cartId, int quantity)
        {
            // Retrieve the cart item from the database
            var cartItem = db.Carts.Find(cartId);
            Product product = db.Products.Where(c => c.product_id.Equals(cartItem.product_id.Value)).SingleOrDefault();
            if (cartItem != null)
            {
                // Update the quantity column
                cartItem.cart_id = cartId;
                cartItem.quantity = quantity;
                cartItem.price = product.price.Value * quantity;
                System.Diagnostics.Debug.WriteLine("\n\nQuantity " + quantity + "\n\n");
                // Save changes to the database
                db.SaveChanges();
            }
            int customerID = 0;
            if (Request.Cookies["CustomerID"].Value != null)
            {
                string customerIDValue = Request.Cookies["CustomerID"].Value;
                customerID = Convert.ToInt32(customerIDValue);
            }
            var carts = (from c in db.Carts
                         where (c.customer_id == customerID)
                         select c);

            return View("Index", carts.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username");
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name");
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cart_id,customer_id,product_id,service_id,quantity,price")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", cart.customer_id);
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", cart.product_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_name", cart.service_id);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", cart.customer_id);
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", cart.product_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_name", cart.service_id);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cart_id,customer_id,product_id,service_id,quantity,price")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", cart.customer_id);
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", cart.product_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_name", cart.service_id);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
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
