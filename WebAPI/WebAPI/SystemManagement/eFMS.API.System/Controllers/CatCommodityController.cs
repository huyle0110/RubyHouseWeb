using AutoMapper;
using eFMS.API.Common.Globals;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.Infrastructure.Common;
using ITL.NetCore.Common.Items;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemManagementAPI.Resources;

namespace eFMS.API.System.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CatCommodityController: ControllerBase
    {
        private readonly IStringLocalizer<LanguageSub> stringLocalizer;
        private readonly ICatCommodityService commodityService;
        private readonly IMapper _mapper;
        public CatCommodityController(IStringLocalizer<LanguageSub> localizer, ICatCommodityService service, IMapper mapper)
        {
            stringLocalizer = localizer;
            commodityService = service;
            _mapper = mapper;
        }

       [HttpGet]
        public List<CatCommodityModel> GetAll()
        {
            return commodityService.Get().Where(w => w.Inactive == false).ToList();
        }

        // POST: api/catcommudity
        [HttpPost]
        public IActionResult Create([FromBody] CatCommodityModel catCommodityModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            var checkExist = CheckCatCommodiyExists(catCommodityModel);
            if (checkExist.Length > 0)
                return BadRequest(new ResultModel()
                {
                    Status = false,
                    Message = stringLocalizer[checkExist]
                });
            var result = commodityService.Create(catCommodityModel);

            var message = HandleError.GetMessage(result, Crud.Insert);
            var objectResult = new ResultModel()
            {
                Status = result.Success,
                Message = stringLocalizer[message].Value
            };
            if (!result.Success)
            {
                return BadRequest(objectResult);
            }
            return Ok(objectResult);
        }
        private string CheckCatCommodiyExists(CatCommodityModel model)
        {
            if (commodityService.Get().Where(w => w.Inactive == false).Any(a => a.CommodityNameEn == model.CommodityNameEn))
            {
                return LanguageSub.MSG_NAME_EXISTED;
            }
            if (commodityService.Get().Where(w => w.Inactive == false).Any(a => a.CommodityNameVn == model.CommodityNameVn))
            {
                return LanguageSub.MSG_NAME_EXISTED;
            }
            return string.Empty;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = commodityService.Get(id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Update(CatCommodityModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var result = commodityService.Update(model.Id.Value, model);
            var message = HandleError.GetMessage(result, Crud.Update);
            var objectResult = new ResultModel()
            {
                Status = result.Success,
                Message = stringLocalizer[message].Value
            };
            if (!result.Success)
            {
                return BadRequest(objectResult);
            }
            return Ok(objectResult);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var result = commodityService.Delete(Id);
            var message = HandleError.GetMessage(result, Crud.Delete);
            var objectResult = new ResultModel()
            {
                Status = result.Success,
                Message = stringLocalizer[message].Value
            };
            if (!result.Success)
            {
                return BadRequest(objectResult);
            }
            return Ok(objectResult);
        }

        [HttpPost]
        [Route("Paging")]
        public IActionResult Paging(CatCommodityCriteria catCommodityCriteria, int page, int size, string orderByProperty, bool isAscendingOrder)
        {
            var data = commodityService.Paging(catCommodityCriteria, page, size, orderByProperty, isAscendingOrder, out int rowsCount);
            var result = new { data, totalItems = rowsCount, page, size };
            return Ok(result);
        }
    }
}
