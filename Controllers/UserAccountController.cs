using Housemate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Housemate.Controllers
{
    public class UserAccountController : Controller
    {
        hmdbEntities db = new hmdbEntities();

        [HttpGet]
        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(CustomerInfo customer)
        {
            if (ModelState.IsValid)
            {
                // Model is valid, proceed with data processing
                CustomerInfo cobj = new CustomerInfo
                {
                    username = customer.username,
                    email = customer.email,
                    first_name = customer.first_name,
                    last_name = customer.last_name,
                    address = customer.address,
                    state = customer.state,
                    city = customer.city,
                    password = customer.password,
                    con_pass = customer.con_pass,
                    phone_number = customer.phone_number
                };

                db.CustomerInfoes.Add(cobj);
                db.SaveChanges();

                // Redirect to a success page or perform any other action after successful registration
                return RedirectToAction("Login", "UserAccount");
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
        public ActionResult EditProfile()
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
