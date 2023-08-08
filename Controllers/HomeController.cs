using Housemate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Housemate.Controllers
{
    public class HomeController : Controller
    {
        hmdbEntities db = new hmdbEntities();
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }
        public ActionResult CategoryView(string category)
        {
            ViewBag.category = category;
            var prod = db.Products.Where(c => c.Category.Contains(category));
            return View(prod.ToList());
        }
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

        public ActionResult AddToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            CustomerInfo customer = db.CustomerInfoes.Find(int.Parse(Request.Cookies["CustomerID"].Value));
            if (product == null)
            {
                return HttpNotFound();
            }
            if(Request.Cookies["CustomerID"] != null)
            {
                Cart cart = db.Carts.FirstOrDefault(c => c.customer_id == customer.customer_id);
                HttpCookie hc  = new HttpCookie("CartID", cart.cart_id.ToString()); ;
                Response.Cookies.Add(hc);
                
                if (cart == null)
                {
                    Cart newCart = new Cart();
                    HttpCookie cookie = Request.Cookies["CartID"];
                    cookie.Value = newCart.cart_id.ToString();
                    Response.Cookies.Set(cookie);
                    System.Diagnostics.Debug.WriteLine(customer.customer_id);
                    newCart.customer_id = customer.customer_id;
                    newCart.price = 0;
                    db.Carts.Add(newCart);
                    db.SaveChanges();
                    CartRecord carR = db.CartRecords.FirstOrDefault(c => (c.cart_id==newCart.cart_id) && (c.product_id == product.product_id) && (c.status.Contains("Pending")));
                    if(carR == null)
                    {
                        CartRecord newCarR = new CartRecord();
                        newCarR.product_id = product.product_id;
                        newCarR.cart_id = newCart.cart_id;
                        newCarR.quantity = 1;
                        newCarR.status = "PendingPayment";
                        newCarR.price = product.price;
                        db.CartRecords.Add(newCarR);
                        db.SaveChanges();
                    }
                    else
                    {
                        carR.product_id = product.product_id;
                        carR.cart_id = cart.cart_id;
                        carR.quantity = carR.quantity + 1;
                        carR.price = product.price * carR.quantity;
                        db.SaveChanges();
                    }
                    
                    
                }
                else
                {
                    CartRecord carR = db.CartRecords.Where(c => (c.cart_id == cart.cart_id) && (c.product_id == product.product_id) && (c.status.Contains("Pending"))).SingleOrDefault();
                    HttpCookie cookie = Request.Cookies["CartID"];
                    cookie.Value = cart.cart_id.ToString();
                    Response.Cookies.Set(cookie);
                    if (carR == null)
                    {
                        CartRecord newCarR = new CartRecord();
                        newCarR.product_id = product.product_id;
                        newCarR.cart_id = cart.cart_id;
                        newCarR.quantity = 1;
                        newCarR.price = product.price;
                        newCarR.status = "PendingPayment";
                        db.CartRecords.Add(newCarR);
                        db.SaveChanges();
                    }
                    else
                    {
                        carR.product_id = product.product_id;
                        carR.cart_id = cart.cart_id;
                        carR.quantity = carR.quantity + 1;
                        carR.price = product.price * carR.quantity;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "UserAccount");
            }

            return RedirectToAction("Index", "Carts");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInfo customer = db.CustomerInfoes.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            //ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", cart.customer_id);
            return View(customer);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerInfo customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(customer);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}