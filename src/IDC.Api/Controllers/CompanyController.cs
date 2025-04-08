using IDC.Application.Dto.Company;
using IDC.Application.Services.Interfaces;
using IDC.Shared.SeedWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
