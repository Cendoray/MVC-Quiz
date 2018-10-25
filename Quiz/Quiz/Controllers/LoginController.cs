using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Quiz.Models;

namespace Quiz.Controllers
{
    public class LoginController : Controller
    {
        // GET: Logim
        [HttpGet]
        public ActionResult UserLogin(string returnUrl)
        {
            UserLogin login = new UserLogin();
            ViewBag.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        public ActionResult UserLogin(string username, string password)
        {
            if (ModelState.IsValid)
            {
                using (var db = new QuizEntities())
                {
                    //check if valid user password pair. If it is, log them into the site and return them back

                    User useer = db.Users.Where(user => user.username == username && user.password == password).Select(user => user).SingleOrDefault();
                    // found a match
                    if (useer != null)
                    {
                        //log into site:
                        FormsAuthentication.RedirectFromLoginPage(username, false);
                        return RedirectToAction("Index", "Category");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid user name or password");
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Category");
        }




        [HttpGet]
        public ActionResult UserRegister()
        {
            UserLogin login = new UserLogin();
            return View(login);
        }

        [HttpPost]
        public ActionResult UserRegister(string username, string password, String lastname, String firstname)
        {
            if (username == "" || password == "" || lastname == "" || firstname == "")
            {
                ModelState.AddModelError("", "You are missing one or many fields");
            }
            if (ModelState.IsValid)
            {
                using (var db = new QuizEntities())
                {
                    User user = new User();
                    user.firstname = firstname;
                    user.lastname = lastname;
                    user.password = password;
                    user.username = username;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Category");
                }
            }
            return View();
        }

    }
}