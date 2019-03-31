using AutoMapper;
using eFMS.API.System.DL.IService;
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
    public class CatCommodityGroupController : ControllerBase
    {
        private readonly IStringLocalizer<LanguageSub> stringLocalizer;
        private readonly ICatCommodityGroupService commodityService;
        private readonly IMapper _mapper;
        public CatCommodityGroupController(IStringLocalizer<LanguageSub> localizer, ICatCommodityGroupService service, IMapper mapper)
        {
            stringLocalizer = localizer;
            commodityService = service;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = commodityService.Get();
            return Ok(result);
        }
    }
}
