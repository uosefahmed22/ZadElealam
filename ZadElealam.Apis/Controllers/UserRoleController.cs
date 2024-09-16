﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using ZadElealam.Core.Interfaces;
using ZadElealam.Core.Models.Auth;
using ZadElealam.Repository;

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
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _userRoleService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var role = await _userRoleService.CreateRole(roleName);
            return Ok(role);
        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _userRoleService.DeleteRole(roleName);
            return Ok(role);
        }
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var result = await _userRoleService.AddUserToRole(email, roleName);
            return Ok(result);
        }
        [HttpPost("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string email, string roleName)
        {
            var result = await _userRoleService.RemoveUserFromRole(email, roleName);
            return Ok(result);
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users =await _userRoleService.GetUsers();
            return Ok(users);
        }
        [HttpGet("GetRolesByUser")]
        public async Task<IActionResult> GetRolesByUser(string email)
        {
            var roles =await _userRoleService.GetRolesByUser(email);
            return Ok(roles);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPatch("AddProfileImage")]
        public async Task<IActionResult> AddProfileImage([FromForm] IFormFile image)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _userRoleService.AddProfileImage(image, null, email);
            return Ok(result);
        }
        [HttpGet("GetUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var result = await _userRoleService.GetUser(email);
            return Ok(result);
        }

    }
}
