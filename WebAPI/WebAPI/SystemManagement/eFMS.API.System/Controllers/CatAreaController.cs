using AutoMapper;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using SystemManagementAPI.Resources;

namespace eFMS.API.System.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CatAreaController
    {
        private readonly IStringLocalizer<LanguageSub> stringLocalizer;
        private readonly ICatAreaService areaService;
        private readonly IMapper _mapper;
        public CatAreaController(IStringLocalizer<LanguageSub> localizer, ICatAreaService service, IMapper mapper)
        {
            stringLocalizer = localizer;
            areaService = service;
            _mapper = mapper;
        }

        [HttpGet]
        public List<CatAreaModel> GetAll()
        {
            return areaService.Get().ToList();
        }
    }
}
