using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.Interfaces;
using ZadElealam.Core.IServices;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Repository.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IImageService _imageService;

        public UserRoleService(AppDbContext dbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IImageService imageService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _imageService = imageService;
        }

        public async Task<ApiResponse> AddProfileImage(IFormFile? image, string? ImageUrl, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse(404, "المستخدم غير موجود");
            }

            if (image == null)
            {
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(user.ImageUrl);
                    user.ImageUrl = null;
                    await _userManager.UpdateAsync(user);
                }
                return new ApiResponse(200, "تم حذف الصورة بنجاح");
            }

            var fileResult = await _imageService.UploadImageAsync(image);
            if (fileResult.Item1 == 1)
            {
                user.ImageUrl = fileResult.Item2;
                await _userManager.UpdateAsync(user);
            }
            else
            {
                return new ApiResponse(400, fileResult.Item2);
            }

            return new ApiResponse(200, "تم اضافة الصورة بنجاح");
        }
        public async Task<ApiResponse> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse(404, "المستخدم غير موجود");
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                return new ApiResponse(404, "الصلاحية غير موجودة");
            }

            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return new ApiResponse(400, "المستخدم موجود بالفعل في الصلاحية");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return new ApiResponse(200, "تم اضافة المستخدم الى الصلاحية بنجاح");
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new ApiResponse(400, $"فشل في اضافة المستخدم الى الصلاحية: {errors}");
        }
        public async Task<ApiResponse> CreateRole(string roleName)
        {
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (!role)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return new ApiResponse(200, "الصلاحية تم انشائها بنجاح");
            }
            return new ApiResponse(400, "الصلاحية موجودة مسبقا");
        }
        public async Task<ApiResponse> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new ApiResponse(404, "الصلاحية غير موجودة");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return new ApiResponse(200, "تم حذف الصلاحية بنجاح");
            }
            return new ApiResponse(400, "فشل في حذف الصلاحية");
        }
        public async Task<IEnumerable<string>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Select(x => x.Name)
                .ToListAsync();
        }
        public async Task<IEnumerable<string>> GetRolesByUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<ApiResponse> GetUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse(404, "المستخدم غير موجود");
            }

            var userDto = new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                ImageUrl = user.ImageUrl
            };

            return new ApiResponse(200,"", userDto);
        }
        public async Task<IEnumerable<string>> GetUsers()
        {
            return await _userManager
                .Users
                .Select(x => x.FullName)
                .ToListAsync();
        }
        public async Task<ApiResponse> RemoveUserFromRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ApiResponse(404, "المستخدم غير موجود");
            }
            var role = await _roleManager.RoleExistsAsync(roleName);
            if (!role)
            {
                return new ApiResponse(404, "الصلاحية غير موجودة");
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return new ApiResponse(200, "تم حذف المستخدم من الصلاحية بنجاح");
            }
            return new ApiResponse(400, "فشل في حذف المستخدم من الصلاحية");
        }
    }
}
