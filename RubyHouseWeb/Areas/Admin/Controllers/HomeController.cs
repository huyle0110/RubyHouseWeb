using RubyHouseServices.EF;
using RubyHouseServices.IServices;
using RubyHouseWeb.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace RubyHouseWeb.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountServices _accountServices;
        public HomeController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            
            return View();
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
            if (ModelState.IsValid)
            {
                // Login
                
                return RedirectToAction("Index");
            }
            //var reuslt = _accountServices.Login(new User() {
            //    Password = model.Password,
            //    UserName = model.UserName
            //});
            return View();
        }

        
    }
}