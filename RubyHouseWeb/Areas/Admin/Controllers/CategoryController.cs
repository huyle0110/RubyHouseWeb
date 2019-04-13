using CommonLibrary.Response;
using EntityFrameWorkModule.IServices;
using EntityFrameWorkModule.Model;
using EntityFrameWorkModule.RequestModel;
using RubyHouseWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RubyHouseWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getDataList(SearchCategoryRequestModel model)
        {
            var result = _categoryServices.search(model);
            return Json(new
            {
                recordsTotal = result.Count(),
                recordsFiltered = result.Count(),
                data = result
            });
        }

        [HttpGet]
        public PartialViewResult Add()
        {
            //var categories = _categoryServices.SelectAll().ToList();
            AddCategoryVM vm = new AddCategoryVM()
            {
            };
            return PartialView(vm);
        }

        [HttpPost]
        public JsonResult Add(Category model)
        {
            model.CreateDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            var result = _categoryServices.Insert(model);
            _categoryServices.Save();
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
    }
}
