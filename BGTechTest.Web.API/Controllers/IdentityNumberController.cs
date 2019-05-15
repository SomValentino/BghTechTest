using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Models;
using BGTechTest.Web.API.Data.Repositories;
using BGTechTest.Web.API.Service;
using BGTechTest.Web.API.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BGTechTest.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityNumberController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IIdentityNumberValidator _identityNumberValidator;
        private readonly IIdentityNumberService _identityNumberService;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<IdentityNumberController> _logger;

        public IdentityNumberController(IDataRepository dataRepository,
            IIdentityNumberValidator identityNumberValidator,
            IIdentityNumberService identityNumberService,
            IHostingEnvironment environment,
            ILogger<IdentityNumberController> logger)
        {
            _dataRepository = dataRepository;
            _identityNumberValidator = identityNumberValidator;
            _identityNumberService = identityNumberService;
            _environment = environment;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddIdentityNumber(string identityNumbers)
        {
            try
            {
                //split identity numbers from request
                var idNumbers = identityNumbers.Trim()
                    .Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
                IdInfo idInfo = _identityNumberService.ExtractIdInformation(idNumbers,_identityNumberValidator);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message,e);
                return StatusCode(500);
            }
        }

        

        
    }
}