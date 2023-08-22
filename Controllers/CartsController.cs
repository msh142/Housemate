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
                Cart cart = db.Carts.FirstOrDefault(c => c.customer_id == customerID);
                if (cart == null)
                {
                    Cart newCart = new Cart();
                    newCart.customer_id = customerID;
                    newCart.price = 0;
                    db.Carts.Add(newCart);
                    db.SaveChanges();
                }

                Cart carts = (from c in db.Carts
                              where (c.customer_id == customerID)
                              select c).SingleOrDefault();
                var carR = from c in db.CartRecords
                           where (c.cart_id == carts.cart_id) && (c.status == "Pending")
                           select c;
                
                if (!carR.Any())
                {
                    carts.price = Convert.ToDecimal(0);
                }
                else
                {
                    carts.price = 0;
                    foreach (var item in carR)
                    {
                        carts.price = carts.price + item.price;
                    }
                }

                db.SaveChanges();

                if (carR != null)
                {
                    return View(carR.ToList());
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login","UserAccount");
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartRecord cart = db.CartRecords.Find(id);
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
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cart_id,customer_id,price")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", cart.customer_id);
            return View(cart);
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int cartId, int quantity)
        {
            // Retrieve the cart item from the database
            var cartItem = db.CartRecords.Find(cartId);
            Product product = db.Products.Where(c => c.product_id.Equals(cartItem.product_id.Value)).SingleOrDefault();
            if (cartItem != null)
            {
                // Update the quantity column
                cartItem.quantity = quantity;
                cartItem.price = product.price.Value * quantity;
                cartItem.status = "Pending";
                // Save changes to the database
                db.SaveChanges();
            }
            int customerID = 0;
            if (Request.Cookies["CustomerID"].Value != null)
            {
                string customerIDValue = Request.Cookies["CustomerID"].Value;
                customerID = Convert.ToInt32(customerIDValue);
            }
            Cart carts = (from c in db.Carts
                          where (c.customer_id == customerID)
                          select c).SingleOrDefault();
            var carR = (from c in db.CartRecords
                        where (c.cart_id == carts.cart_id)
                        select c);
            if (!carR.Any())
            {
                carts.price = Convert.ToDecimal(0);
            }
            else
            {
                carts.price = Convert.ToDecimal(0);
                foreach (var item in carR)
                {
                    carts.price = carts.price + item.price;
                }
            }

            return RedirectToAction("Index", carR.ToList());
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartRecord cart = db.CartRecords.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            //ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", cart.customer_id);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cart_id,customer_id,price")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", cart.customer_id);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int id)
        {
            Product prod = db.Products.Find(id);
            CartRecord cart = db.CartRecords.Where(c => c.product_id == prod.product_id).FirstOrDefault();
            db.CartRecords.Remove(cart);
            IEnumerable<CartRecord> cartR = db.CartRecords.Where(c => c.cart_id == cart.cart_id).ToList();
            if(cartR == null)
            {
                Cart c = db.Carts.Where(a => a.cart_id == cart.cart_id).FirstOrDefault();
                db.Carts.Remove(c);
            }
            
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
