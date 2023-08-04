using Housemate.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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