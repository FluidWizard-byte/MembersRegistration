using Member.Models;
using Member.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Member.Controllers
{
    public class AccountController : Controller
    {
        Repository repository = new Repository();
        // GET: Account
        public ActionResult Index()
        {
            /*if (HttpContext.User.Identity.IsAuthenticated) 
            {
                return RedirectToAction("Index", "Members");
            }*/

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(Login model) //Login
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var result = await repository.login(model);
            if (result.resultCode == 200)
            {
                FormsAuthentication.SetAuthCookie(model.email, false);
                return RedirectToAction("Create", "Members", result.Data); //redirect to login form
            }
            else
            {
                ModelState.AddModelError("", result.message);
            }

            return View();
        }

        public ActionResult Register() //Register
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Register(Login model) //Register
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var result = await repository.Register(model);

            if (result.resultCode == 200)
            {
                TempData["SuccessMessage"] = "Registration successful";

                return RedirectToAction("Index"); //redirect to login form
            }
            else
            {
                ModelState.AddModelError("", result.message);
            }

            return View();
        }

        public ActionResult Logout() 
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index"); 
        }
    }
}