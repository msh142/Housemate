using Housemate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Housemate.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        hmdbEntities db = new hmdbEntities();
        [HttpGet]
        public ActionResult ViewUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(CustomerInfo customer)
        {
            CustomerInfo cobj = new CustomerInfo();
            cobj.username = customer.username;
            cobj.email = customer.email;
            cobj.first_name = customer.first_name;
            cobj.last_name = customer.last_name;
            cobj.address = customer.address;
            cobj.state = customer.state;
            cobj.city = customer.city;
            cobj.password = customer.password;
            cobj.phone_number = customer.phone_number;

            db.CustomerInfoes.Add(cobj);
            db.SaveChanges();
            return View("ViewUser");
        }

    }
}