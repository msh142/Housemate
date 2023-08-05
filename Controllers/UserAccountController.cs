using Housemate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Housemate.Controllers
{
    public class UserAccountController : Controller
    {
        hmdbEntities1 db = new hmdbEntities1();

        public ActionResult Index()
        {
            return View(db.CustomerInfoes.ToList());
        }


        public ActionResult Details(int? id)
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
            return View(customer);
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
            return View(customer);
        }

        public ActionResult Delete(int? id)
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
            return View(customer);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerInfo customer = db.CustomerInfoes.Find(id);
            db.CustomerInfoes.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(CustomerInfo customer, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string filename = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                    string extension = Path.GetExtension(ImageFile.FileName);
                    filename = filename + DateTime.Now.ToString("yymmssff") + extension;
                    customer.image_data = "~/img/customer/" + filename;
                    filename = Path.Combine(Server.MapPath("~/img/customer/"), filename);
                    ImageFile.SaveAs(filename);
                    db.CustomerInfoes.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index", "UserAccount");

                }
                return View("RegisterUser", customer);
                //return RedirectToAction("Login", "UserAccount");
                // Redirect to a success page or perform any other action after successful registration
            }
            else
            {
                // Model is not valid, return to the registration form with validation errors
                return View("RegisterUser", customer);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Verify(CustomerInfo customer)
        {
            CustomerInfo ci = db.CustomerInfoes.Where(a => a.email.Equals(customer.email) && a.password.Equals(customer.password)).SingleOrDefault();

            if (ci != null)
            {
                HttpCookie hc = new HttpCookie("CustomerID", ci.customer_id.ToString());
                Response.Cookies.Add(hc);
                //hc.Expires = DateTime.Now.AddSeconds(10);
                HttpCookie hc1 = new HttpCookie("CustomerEmail", ci.email.ToString());
                Response.Cookies.Add(hc1);
                HttpCookie hc2 = new HttpCookie("CustomerUsername", ci.username.ToString());
                Response.Cookies.Add(hc2);
                //hc1.Expires = DateTime.Now.AddSeconds(10);

                CustomerLogin cus_l = new CustomerLogin()
                {
                    customer_id = customer.customer_id,
                    email = customer.email,
                    password = customer.password,
                    date_time = DateTime.Now
                    
                };
                Console.WriteLine(customer.customer_id);

                db.CustomerLogins.Add(cus_l);
                //db.SaveChanges();


                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.InvalidMessage = "Invalid E-mail or Password";
            }
            return View("Login");
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpCookie rc = Request.Cookies["CustomerEmail"];
            HttpCookie rc1 = Request.Cookies["CustomerUsername"];

            rc.Expires = DateTime.Now.AddSeconds(-1);
            Response.Cookies.Add(rc);
            if (rc1 != null)
            {
                rc1.Expires = DateTime.Now.AddSeconds(-1);
                Response.Cookies.Add(rc1);
            }
            
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
