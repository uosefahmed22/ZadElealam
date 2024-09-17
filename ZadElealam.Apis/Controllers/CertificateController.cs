using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IServices;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;
        private readonly UserManager<AppUser> _userManager;

        public CertificateController(ICertificateService certificateService,
            UserManager<AppUser> userManager)
        {
            _certificateService = certificateService;
            _userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("download")]
        public async Task<IActionResult> DownloadCertificate(int examId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }

            var student = await _userManager.FindByEmailAsync(email);
            var studentId = student.Id;
            return await _certificateService.DownloadCertificate(studentId, examId);
        }
    }
}
