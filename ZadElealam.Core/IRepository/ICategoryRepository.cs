using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.Models;

namespace ZadElealam.Core.IRepository
{
    public interface ICategoryRepository
    {
        Task<ApiResponse> GetAllCategoery();
        Task<ApiResponse> GetCategoeryById(int id);
        Task<ApiResponse> AddCategoery(CategoryDto categoery);
        Task<ApiResponse> UpdateCategoery(int categoeryId, CategoryDto categoery);
        Task<ApiResponse> DeleteCategoery(int id);
    }
}
