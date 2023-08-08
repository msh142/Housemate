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
    public class CheckoutsController : Controller
    {
        private hmdbEntities db = new hmdbEntities();

        // GET: Checkouts
        public ActionResult Index()
        {
            int cus_id = int.Parse(Request.Cookies["CustomerID"].Value);
            System.Diagnostics.Debug.WriteLine(cus_id);
            CustomerInfo customer = db.CustomerInfoes.FirstOrDefault(c => c.customer_id.Equals(cus_id));
            System.Diagnostics.Debug.WriteLine(customer.customer_id);

            var cart = (from c in db.Carts
                        where c.customer_id == customer.customer_id
                        select c).SingleOrDefault();
            var address = (from c in db.ShippingAddresses
                        where c.customer_id == customer.customer_id
                        select c).SingleOrDefault();
            System.Diagnostics.Debug.WriteLine(cart.cart_id);
            List<CartRecord> cr = db.CartRecords.Where(c => c.cart_id == cart.cart_id).ToList();
            foreach (var item in cr)
            {
                System.Diagnostics.Debug.WriteLine(item.product_id);
            }
            ViewBag.customer = customer;
            ViewBag.cart = cart;
            ViewBag.cartR = cr;
            ViewBag.fee = Convert.ToDecimal(60);
            ViewBag.discount = Convert.ToDecimal(0);
            if (address != null)
            {
                ViewBag.address = address;
            }
            else
            {
                ViewBag.noaddress = "No address found!";
            }
            var checkouts = db.Checkouts.Include(c => c.Cart).Include(c => c.CustomerInfo);
            return View(checkouts.ToList());
        }

        // GET: Checkouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = db.Checkouts.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // GET: Checkouts/Create
        public ActionResult Create()
        {
            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id");
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username");
            return View();
        }

        // POST: Checkouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "checkout_id,cart_id,customer_id,checkout_date")] Checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.Checkouts.Add(checkout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id", checkout.cart_id);
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", checkout.customer_id);
            return View(checkout);
        }

        // GET: Checkouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = db.Checkouts.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id", checkout.cart_id);
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", checkout.customer_id);
            return View(checkout);
        }

        // POST: Checkouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "checkout_id,cart_id,customer_id,checkout_date")] Checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cart_id = new SelectList(db.Carts, "cart_id", "cart_id", checkout.cart_id);
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", checkout.customer_id);
            return View(checkout);
        }

        // GET: Checkouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = db.Checkouts.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // POST: Checkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Checkout checkout = db.Checkouts.Find(id);
            db.Checkouts.Remove(checkout);
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
