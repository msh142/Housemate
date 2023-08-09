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
    public class PaymentMethodsController : Controller
    {
        private hmdbEntities db = new hmdbEntities();

        // GET: PaymentMethods
        public ActionResult Index()
        {
            var paymentMethods = db.PaymentMethods.Include(p => p.CustomerInfo);
            return View(paymentMethods.ToList());
        }

        // GET: PaymentMethods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Create
        public ActionResult Create()
        {
            int CUS_ID = int.Parse(Request.Cookies["CustomerID"].Value);
            PaymentMethod pm = db.PaymentMethods.Where(c => c.customer_id == CUS_ID).FirstOrDefault();
            return View(pm);
        }

        // POST: PaymentMethods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "payment_method_id,customer_id,card_number,cardholder_name,expiration_date,cvv,is_default")] PaymentMethod paymentMethod, FormCollection form)
        {
            
            string payment = form["PaymentMethod"];
            int customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
            ShippingAddress address = db.ShippingAddresses.Where(c => c.customer_id == customer_id).FirstOrDefault();
            Cart cart = db.Carts.FirstOrDefault(c => c.customer_id == customer_id);
            int cart_id = cart.cart_id;
            Order order = new Order();
            if (Request.Cookies["CustomerID"] != null)
            {
                if (address != null)
                {

                    if (payment == "cerditCard")
                    {
                        System.Diagnostics.Debug.WriteLine("Credit Cart");
                        PaymentMethod pm = db.PaymentMethods.Where(c => c.customer_id == customer_id).FirstOrDefault();
                        if (pm == null)
                        {
                            pm.customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
                            pm.cardholder_name = paymentMethod.cardholder_name;
                            pm.card_number = paymentMethod.card_number;
                            pm.cvv = paymentMethod.cvv;
                            pm.expiration_date = paymentMethod.expiration_date;
                            db.PaymentMethods.Add(pm);
                            db.SaveChanges();
                            order.customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
                            order.cart_id = cart_id;
                            order.order_date = DateTime.Now;
                            order.order_status = "Paid";
                            order.feedback = " ";
                            System.Diagnostics.Debug.WriteLine(order.customer_id);
                            System.Diagnostics.Debug.WriteLine(order.cart_id);
                            System.Diagnostics.Debug.WriteLine(order.order_date);
                            System.Diagnostics.Debug.WriteLine(order.order_status);
                            System.Diagnostics.Debug.WriteLine(order.feedback);
                            db.Orders.Add(order);
                            db.SaveChanges();
                            System.Diagnostics.Debug.WriteLine("Order Placed");
                            return RedirectToAction("Success");
                        }
                        else
                        {
                            order.customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
                            order.cart_id = cart_id;
                            order.order_date = DateTime.Now;
                            order.order_status = "Paid";
                            order.feedback = " ";
                            System.Diagnostics.Debug.WriteLine(order.customer_id);
                            System.Diagnostics.Debug.WriteLine(order.cart_id);
                            System.Diagnostics.Debug.WriteLine(order.order_date);
                            System.Diagnostics.Debug.WriteLine(order.order_status);
                            System.Diagnostics.Debug.WriteLine(order.feedback);
                            db.Orders.Add(order);
                            db.SaveChanges();
                            System.Diagnostics.Debug.WriteLine("Order Placed");
                            return RedirectToAction("Success");
                        }

                    }
                    else if (payment == "cod")
                    {
                        System.Diagnostics.Debug.WriteLine("Cash on Delivery");
                        
                        order.customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
                        order.cart_id = cart_id;
                        order.order_date = DateTime.Now;
                        order.order_status = "COD";
                        order.feedback = " ";
                        System.Diagnostics.Debug.WriteLine(order.customer_id);
                        System.Diagnostics.Debug.WriteLine(order.cart_id);
                        System.Diagnostics.Debug.WriteLine(order.order_date);
                        System.Diagnostics.Debug.WriteLine(order.order_status);
                        System.Diagnostics.Debug.WriteLine(order.feedback);
                        db.Orders.Add(order);
                        db.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Order Placed");
                        return RedirectToAction("Success");
                    }
                    else
                    {
                        return View(paymentMethod);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Create", "ShippingAddresses");
            }
        }

        public ActionResult Success()
        {
            int customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
            Cart cart = db.Carts.FirstOrDefault(c => c.customer_id == customer_id);
            var carR = db.CartRecords.Where(c =>c.cart_id == cart.cart_id && c.status == "PaymentPending").ToList();
            foreach(var item in carR)
            {
                item.status = "Processing";
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View();
        }

        // GET: PaymentMethods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", paymentMethod.customer_id);
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "payment_method_id,customer_id,card_number,cardholder_name,expiration_date,cvv,is_default")] PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentMethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", paymentMethod.customer_id);
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            db.PaymentMethods.Remove(paymentMethod);
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
