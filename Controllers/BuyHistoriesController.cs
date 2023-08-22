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
    public class BuyHistoriesController : Controller
    {
        private hmdbEntities db = new hmdbEntities();

        // GET: BuyHistories
        public ActionResult Index()
        {
            var buyHistories = db.BuyHistories.Include(b => b.CustomerInfo);
            return View(buyHistories.ToList());
        }

        // GET: BuyHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyHistory buyHistory = db.BuyHistories.Find(id);
            if (buyHistory == null)
            {
                return HttpNotFound();
            }
            return View(buyHistory);
        }

        // GET: BuyHistories/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username");
            return View();
        }

        // POST: BuyHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "history_id,customer_id,quantity,purchase_date")] BuyHistory buyHistory)
        {
            if (ModelState.IsValid)
            {
                db.BuyHistories.Add(buyHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", buyHistory.customer_id);
            return View(buyHistory);
        }

        // GET: BuyHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyHistory buyHistory = db.BuyHistories.Find(id);
            if (buyHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", buyHistory.customer_id);
            return View(buyHistory);
        }

        // POST: BuyHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "history_id,customer_id,quantity,purchase_date")] BuyHistory buyHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buyHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.CustomerInfoes, "customer_id", "username", buyHistory.customer_id);
            return View(buyHistory);
        }

        // GET: BuyHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyHistory buyHistory = db.BuyHistories.Find(id);
            if (buyHistory == null)
            {
                return HttpNotFound();
            }
            return View(buyHistory);
        }

        // POST: BuyHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BuyHistory buyHistory = db.BuyHistories.Find(id);
            db.BuyHistories.Remove(buyHistory);
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
