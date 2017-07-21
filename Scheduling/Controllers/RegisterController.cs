using Scheduling.Models;
using Scheduling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Controllers
{



    public class RegisterController : Controller

    {


        DBEntities db = new DBEntities();
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(RegisterVM vm)
        {

            if (ModelState.IsValid)
            {
                
                    db.Logins.Add(new Login
                    {
                        Username = vm.Username,
                        Password = vm.Password


                    });
                    db.SaveChanges();
                    ViewBag.success = "Register Successfully!";
                    //var success = "Register Successfully!";
                    return RedirectToAction("Index", "Login");
                
            }


            return View();
        }

    }
}