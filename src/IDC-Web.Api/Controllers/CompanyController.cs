using IDC.Api.Extensions.Authorization;
using IDC.Application.Dto.Company;
using IDC.Application.Services.Interfaces;
using IDC.Shared.Constants;
using IDC.Shared.SeedWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet] 
        [ClaimRequirement(FunctionCode.COMPANY, CommandCode.VIEW)]
        public async Task<IActionResult> GetCompany()
        {
            var result = await _companyService.GetCompanys();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }
            try
            {
                await _companyService.CreateCompany(request);
                return Ok(new ApiSuccessResult<bool>(200, true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResult<bool>(500, $"Internal server error: {ex.Message}"));
            }
        }
    }
}
