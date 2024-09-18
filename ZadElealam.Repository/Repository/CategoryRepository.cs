using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.IServices;
using ZadElealam.Core.Models;
using ZadElealam.Repository.Data;

namespace ZadElealam.Repository.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CategoryRepository(AppDbContext dbContext,
            IImageService imageService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _imageService = imageService;
            _mapper = mapper;
        }
        public async Task<ApiResponse> AddCategoery(CategoryDto categoery)
        {
            var ExistingCategory = _dbContext.Categories.FirstOrDefault(x => x.Name == categoery.Name);
            if (ExistingCategory != null)
            {
                return new ApiResponse(400, "Category Already Exist");
            }
            try
            {
                if (categoery.Image != null)
                {
                    var fileResult = await _imageService.UploadImageAsync(categoery.Image);
                    if (fileResult.Item1 == 1)
                    {
                        categoery.ImageUrl = fileResult.Item2;
                    }
                    else
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }
                }
                var category = _mapper.Map<Category>(categoery);
                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Category Added Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteCategoery(int id)
        {
            var ExistingCategory = _dbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (ExistingCategory == null)
            {
                return new ApiResponse(400, "Category Not Found");
            }
            try
            {
                if (!string.IsNullOrEmpty(ExistingCategory.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(ExistingCategory.ImageUrl);
                }
                _dbContext.Categories.Remove(ExistingCategory);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Category Deleted Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, ex.Message);
            }
        }
        public async Task<ApiResponse> GetAllCategoery()
        {
            try
            {
                var result = await _dbContext
                .Categories
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl
                }).ToListAsync();
                return new ApiResponse(200, result);
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, ex.Message);
            }
        }
        public async Task<ApiResponse> GetCategoeryById(int id)
        {
            try
            {
                var result = await _dbContext.Categories
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl
                }).FirstOrDefaultAsync();
                return new ApiResponse(200, result);
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateCategoery(int categoeryId, CategoryDto categoery)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoeryId);
            if (existingCategory == null)
            {
                return new ApiResponse(400, "Category Not Found");
            }

            try
            {
                if (categoery.Image != null)
                {
                    if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                    {
                        await _imageService.DeleteImageAsync(existingCategory.ImageUrl);
                    }

                    var fileResult = await _imageService.UploadImageAsync(categoery.Image);
                    if (fileResult.Item1 != 1)
                    {
                        return new ApiResponse(400, fileResult.Item2);
                    }

                    categoery.ImageUrl = fileResult.Item2;
                }
                _mapper.Map(categoery, existingCategory);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Category Updated Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, ex.Message);
            }
        }
    }
}