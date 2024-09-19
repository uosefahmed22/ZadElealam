using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IServices.Auth;
using ZadElealam.Core.Models.Auth;
using ZadElealam.Repository.Data;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]

    public class UserRoleController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(AppDbContext dbcontext
            , UserManager<AppUser> userManager
            , RoleManager<IdentityRole> roleManager,
            IUserRoleService userRoleService
            )
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRoleService = userRoleService;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userRoleService.GetRolesAsync();
            return Ok(roles);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var role = await _userRoleService.CreateRole(roleName);
            return Ok(role);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _userRoleService.DeleteRole(roleName);
            return Ok(role);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var result = await _userRoleService.AddUserToRole(email, roleName);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var result = await _userRoleService.RemoveUserFromRole(email, roleName);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users =await _userRoleService.GetUsers();
            return Ok(users);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("GetRolesByUser")]
        public async Task<IActionResult> GetRolesByUser(string email)
        {
            var roles =await _userRoleService.GetRolesByUser(email);
            return Ok(roles);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPatch("AddProfileImage")]
        public async Task<IActionResult> AddProfileImage(IFormFile image)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _userRoleService.AddProfileImage(image, null, email);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _userRoleService.GetUser(email);
            return Ok(result);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User , Admin")]
        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var result = await _userRoleService.DeleteUser(email);
            return Ok(result);
        }

    }
}
