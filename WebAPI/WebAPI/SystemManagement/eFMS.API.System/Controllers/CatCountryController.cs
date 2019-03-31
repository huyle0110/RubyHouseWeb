using AutoMapper;
using eFMS.API.System.DL.IService;
using eFMS.API.System.DL.Models;
using eFMS.API.System.DL.Services;
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
    public class CatCountryController
    {
        private readonly IStringLocalizer<LanguageSub> stringLocalizer;
        private readonly ICatCountryService countryService;
        private readonly IMapper _mapper;
        public CatCountryController(IStringLocalizer<LanguageSub> localizer, ICatCountryService service, IMapper mapper)
        {
            stringLocalizer = localizer;
            countryService = service;
            _mapper = mapper;
        }

        [HttpGet]
        public List<CatCountryModel> GetAll()
        {
            return countryService.Get().ToList();
        }
    }
}
