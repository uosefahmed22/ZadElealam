using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Errors;

namespace ZadElealam.Core.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<string>> GetRolesAsync();
        Task<ApiResponse> CreateRole(string roleName);
        Task<ApiResponse> DeleteRole(string roleName);
        Task<ApiResponse> AddUserToRole(string email, string roleName);
        Task<ApiResponse> RemoveUserFromRole(string email, string roleName);
        Task<IEnumerable<string>> GetUsers();
        Task<IEnumerable<string>> GetRolesByUser(string email);
    }
}
