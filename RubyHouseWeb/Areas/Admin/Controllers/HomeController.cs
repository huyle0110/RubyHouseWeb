using RubyHouseWeb.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RubyHouseWeb.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            return RedirectToAction("Index");
        }

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}