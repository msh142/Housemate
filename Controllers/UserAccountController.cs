using Housemate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Housemate.Controllers
{
    public class UserAccountController : Controller
    {
        hmdbEntities db = new hmdbEntities();
        private static Random random = new Random();

        public ActionResult Index()
        {
            if(Request.Cookies["AdminID"] != null)
            {
                return View(db.CustomerInfoes.ToList());
            }
            else
            {
                ViewBag.ErrorMessage = "This page can only be accessed by Admin!";
                return View("Show Error");
            }
            
        }
        public ActionResult ShowError()
        {
            return View();
        }
        public ActionResult Search(string searchString)
        {
            var customers = from c in db.CustomerInfoes select c;
            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchInt))
                {
                    // The searchString can be converted to an int
                    customers = customers.Where(c => c.customer_id == searchInt);
                }
                else
                {
                    // The searchString is not a valid int, so filter by name or email
                    customers = customers.Where(c => c.email.Contains(searchString) || c.username.Contains(searchString));
                }
            };


            ViewBag.CurrentFilter = searchString;
            return View("Index", customers.ToList());
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
                    return RedirectToAction("Login", "UserAccount");

                }
                return View("RegisterUser", customer);
                //return RedirectToAction("Login", "UserAccount");
                // Redirect to a success page or perform any other action after successful registration
            }
            else
            {
                // Model is not valid, return to the registration form with validation errors
                return View("RegisterUser");
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                char randomChar = chars[index];
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }

        public static void SendEmail(string toEmail, string subject, string body)
        {
            string fromEmail = "mdsabbir120834@hotmail.com"; // Replace with your Gmail address
            string appPassword = "msh120834"; // Replace with your App Password

            SmtpClient smtpClient = new SmtpClient("smtp.live.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(fromEmail, appPassword);
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
            mailMessage.IsBodyHtml = true;

            try
            {
                smtpClient.Send(mailMessage);
                System.Diagnostics.Debug.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to send email: " + ex.Message);
            }
        }
    
        public ActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forget(string email)
        {
            CustomerInfo customer = db.CustomerInfoes.Where(c => c.email == email).SingleOrDefault();
            string newpass = GenerateRandomString(8);
            customer.password = newpass;
            customer.con_pass = newpass;
            db.SaveChanges();
            string body = "Dear Customer,\nThe request for changing your pussword has been initiated.\n Here is your temporary password to login: "+ newpass +"\n Please Change the password after your l";
            SendEmail(email.ToLower().ToString(), "Request for password reset", body);
            return RedirectToAction("Login");
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
