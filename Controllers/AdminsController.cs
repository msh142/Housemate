using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Housemate.Models;

namespace Housemate.Controllers
{
    public class AdminsController : Controller
    {
        private hmdbEntities1 db = new hmdbEntities1();

        // GET: Admins
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "admin_id,username,email,password,first_name,last_name,con_pass")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            Console.WriteLine("Admin ID:" + id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);

            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "admin_id,username,email,password,first_name,last_name,con_pass")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
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


        ///////////////////////////////////////////////////////////////////////////////////////////////
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Verify(Admin admin)
        {
            Admin ad = db.Admins.Where(a => a.username.Equals(admin.username) && a.password.Equals(admin.password)).SingleOrDefault();
            //Alert Message
            
            
            if (ad != null)
            {
                HttpCookie hc4 = new HttpCookie("login", "True");
                Response.Cookies.Add(hc4);
                hc4.Expires = DateTime.Now.AddSeconds(5);
                HttpCookie hc = new HttpCookie("AdminID", ad.admin_id.ToString());
                Response.Cookies.Add(hc);
                //hc.Expires = DateTime.Now.AddSeconds(10);
                HttpCookie hc1 = new HttpCookie("AdminEmail", ad.email.ToString());
                Response.Cookies.Add(hc1);
                HttpCookie hc2 = new HttpCookie("AdminUsername", ad.username.ToString());
                Response.Cookies.Add(hc2);
                //hc1.Expires = DateTime.Now.AddSeconds(10);
                

                return View("Dashboard");
            }
            else
            {
                ViewBag.InvalidMessage = "Invalid username or Password";
            }
            return View("Login");
        }


        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Logout()
        {
            HttpCookie rc = Request.Cookies["AdminEmail"];
            HttpCookie rc1 = Request.Cookies["AdminUsername"];

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

        ///Products
        public ActionResult AddProduct()
        {
            return RedirectToAction("Create", "Products");
        }

        public ActionResult ViewProducts()
        {
            return RedirectToAction("Index", "Products");
        }

        public ActionResult UpdateProduct()
        {
            return RedirectToAction("Index", "Products");
        }

        public ActionResult RemoveProduct()
        {
            return RedirectToAction("Index", "Products");
        }




    }
}