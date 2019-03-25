using RubyHouseServices.EF;
using RubyHouseServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RubyHouseWeb.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(Product model)
        {
            return Json(new { });
        }

        [HttpGet]
        public PartialViewResult Detail()
        {
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult Edit()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(Product model)
        {
            return Json(new { });
        }


    }
}