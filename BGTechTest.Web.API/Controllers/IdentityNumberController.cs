using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BGTechTest.Web.API.Data.Dtos;
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

        [HttpPost()]
        [Route("addids")]
        public async Task<IActionResult> AddIdentityNumber(IdsDto idsDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idsDto.idNumbers))
                    return BadRequest("No identity numbers in request");
                //split identity numbers from request
                var idNumbers = idsDto.idNumbers.Trim()
                    .Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
                //Get the valid and invalid id information if they exist.
                IdInfo idInfo = _identityNumberService.ExtractIdInformation(idNumbers,_identityNumberValidator);
                // Save to data store: In case as a csv file
                if (idInfo.validIdInfos.Any())
                    await _dataRepository.Save(idInfo.validIdInfos, FileCsvType.ValidIdFile);
                if (idInfo.InvalidIdInfos.Any())
                    await _dataRepository.Save(idInfo.InvalidIdInfos, FileCsvType.InValidIdFile);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message,e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("uploadcsv")]
        public async Task<IActionResult> UploadIdsFromCsv([FromForm] CsvUploadDto csvIdDto)
        {
            try
            {
                var file = csvIdDto.CsvUploadFile;

                if (file.Length > 0)
                {
                    string fileContents = string.Empty;
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        fileContents = await reader.ReadToEndAsync();
                    }
                    if (string.IsNullOrWhiteSpace(fileContents))
                        return BadRequest("No identity numbers in upload File");
                    var idNumbers = fileContents.Trim().Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
                    var idInfos = _identityNumberService.ExtractIdInformation(idNumbers, _identityNumberValidator);
                    // save valid and invalid ids to csv
                    if (idInfos.validIdInfos.Any())
                        await _dataRepository.Save(idInfos.validIdInfos, FileCsvType.ValidIdFile);
                    if (idInfos.InvalidIdInfos.Any())
                        await _dataRepository.Save(idInfos.InvalidIdInfos, FileCsvType.InValidIdFile);
                    return NoContent();
                }

                return BadRequest("The file upload has id's contained");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return StatusCode(500);
            }
        }

        [HttpGet()]
        [Route("getIds")]
        public async Task<IActionResult> GetIds()
        {
            try
            {
                var validIdInfo = await _dataRepository.Read<ValidIDInfo>(FileCsvType.ValidIdFile);
                var invalidInfo = await _dataRepository.Read<InvalidIDInfo>(FileCsvType.InValidIdFile);
                var idInfo = new IdInfo();
                // select only distinct Ids
                idInfo.validIdInfos.AddRange(validIdInfo.GroupBy(x => x.IdentityNumber)
                    .Select(x => new ValidIDInfo(x.First().IdentityNumber,x.First().BirthDate,
                        x.First().Gender,x.First().Cizitenship)));
                idInfo.InvalidIdInfos.AddRange(invalidInfo.GroupBy(x => x.IdentityNumber)
                    .Select(x => new InvalidIDInfo(x.First().IdentityNumber,
                        x.First().ReasonsFailed)));
                return Ok(idInfo);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message,e);
                return StatusCode(500);
            }
        }

        
    }
}