using CommonLibrary.Response;
using EntityFrameWorkModule.IServices;
using EntityFrameWorkModule.Model;
using EntityFrameWorkModule.RequestModel;
using EntityFrameWorkModule.Services;
using RubyHouseWeb.Models;
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
        //private ProductServices _productServices = new ProductServices();

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getData(SearchProductRequestModel model)
        {
            var result = _productServices.search(model);
            return Json(new
            {
                recordsTotal = result.totalRecord,
                recordsFiltered = result.totalRecord,
                data = result.resultList
            });
        }

        [HttpGet]
        public PartialViewResult Add()
        {
            return PartialView(new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(Product model)
        {
            model.CreateDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            var result = _productServices.Insert(model);
            _productServices.Save();
            var responModel = new ResponseModel()
            {
                Code = result,
                Message = "Success"
            };
            return Json(responModel);
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