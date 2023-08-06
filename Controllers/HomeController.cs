using Housemate.Models;
using System;
using System.Collections.Generic;
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
            //SqlConnection conn = new SqlConnection(@"Database=hmdb;Server=G0DZI11A\SQLEXPRESS01;user=sa;password=123456");
            //conn.Open();
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = conn;
            //cmd.CommandText = "";


            //conn.Close();

            return View(db.Products.ToList());
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
            Cart cart = db.Carts.Where(c => c.product_id == id).SingleOrDefault();
            if(cart == null || cart.customer_id != int.Parse(Request.Cookies["CustomerID"].Value))
            {
                Cart addcart = new Cart();
                addcart.customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
                addcart.product_id = id;
                addcart.quantity = 1;
                addcart.Product = product;
                addcart.price = product.price;
                addcart.CustomerInfo = customer;

                db.Carts.Add(addcart);
                db.SaveChanges();
            }
            else
            {
                cart.cart_id = cart.cart_id;
                cart.customer_id = int.Parse(Request.Cookies["CustomerID"].Value);
                cart.product_id = id;
                cart.quantity = cart.quantity + 1;
                cart.Product = product;
                cart.price = product.price * cart.quantity;
                cart.CustomerInfo = customer;

                db.SaveChanges();
            }

            
            return RedirectToAction("Index", "Carts");
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