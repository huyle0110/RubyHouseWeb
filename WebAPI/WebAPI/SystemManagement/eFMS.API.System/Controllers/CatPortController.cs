using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eFMS.API.Common.Globals;
using eFMS.API.System.DL.Infrastructure.ErrorHandler;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Models.Criteria;
using eFMS.API.System.Infrastructure.Common;
using eFMS.API.System.Service.Models;
using ITL.NetCore.Common.Items;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SystemManagementAPI.Resources;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eFMS.API.System.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CatPortController : ControllerBase
    {
        private readonly IStringLocalizer<LanguageSub> stringLocalizer;
        private readonly ICatPortService catPortservice;
        private readonly IMapper _mapper;
        public CatPortController(IStringLocalizer<LanguageSub> localizer, ICatPortService service, IMapper mapper)
        {
            stringLocalizer = localizer;
            catPortservice = service;
            _mapper = mapper;
        }

       [HttpGet]
        public async Task<List<CatPortModel>> GetCatPortAsync()
        {
            var listPort =await catPortservice.GetAsync(w => w.PlaceTypeId == PlaceType.Port.ToString()
                                                         && w.Inactive == false);
            var result = _mapper.Map<List<CatPortModel>>(listPort);
            return result;
        }

        [HttpPost]
        public IActionResult CreatePort([FromBody] CatPortModel CatPort)
        {
            if (!ModelState.IsValid) return BadRequest();
            var messageExisted = CheckExisted(CatPort);
            if (messageExisted.Length > 0)
                return BadRequest(new ResultModel() {
                    Status = false,
                    Message = stringLocalizer[messageExisted]
                });

            CatPlaceModel catPlaceModel = _mapper.Map<CatPortModel, CatPlaceModel>(CatPort);
            var result = catPortservice.CreatePort(catPlaceModel);
            var message = HandleError.GetMessage(result, Crud.Insert);
            if (!result.Success)
            {
                return BadRequest(new { status = result.Success, mess = stringLocalizer[message] });
            }
            return Ok(new ResultModel() {
                Status = result.Success,
                Message = stringLocalizer[message]
            });
        }

        private string CheckExisted(CatPortModel model)
        {
                if (catPortservice.Get().Where(w => w.Inactive == false).Any(x => x.Code == model.Code))
                {
                    return LanguageSub.MSG_CODE_EXISTED;
                }
                //if (catPortservice.Any(x => x.NameEn == model.Name))
                //{
                //    return LanguageSub.MSG_NAME_EXISTED;
                //}
            return string.Empty;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = catPortservice.First(id);
            return Ok(_mapper.Map<CatPlaceModel,CatPortModel>(result));
        }

        [HttpPut]
        public IActionResult Put(CatPortModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = catPortservice.Update(model.Id.Value, _mapper.Map<CatPortModel, CatPlaceModel>(model));
            var message = HandleError.GetMessage(result, Crud.Update);
            if (!result.Success)
            {
                return BadRequest(new ResultModel()
                {
                    Status = result.Success,
                    Message = stringLocalizer[message].Value
                });
            }
            return Ok(new ResultModel()
            {
                Status = result.Success,
                Message = stringLocalizer[message].Value
            });
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var result = catPortservice.Delete(Id);
            var message = HandleError.GetMessage(result, Crud.Delete);
            if (!result.Success)
            {
                return BadRequest(new ResultModel() { Status = result.Success, Message = stringLocalizer[message] });
            }
            return Ok(new ResultModel()
            {
                Status = result.Success,
                Message = stringLocalizer[message].Value
            });
        }

        [HttpPost]
        [Route("Paging")]
        public IActionResult Paging(PortIndexCriteria criteria, [FromQuery] int page, [FromQuery] int size, [FromQuery] string orderByProperty, [FromQuery] bool isAscendingOrder)
        {
            var data = catPortservice.Paging(criteria, page, size, orderByProperty, isAscendingOrder, out int rowCount);
            var result = new { data, totalItems = rowCount, page, size };
            return Ok(result);
        }
    }
}
