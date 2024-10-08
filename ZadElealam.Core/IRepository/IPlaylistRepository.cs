﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.Models;

namespace ZadElealam.Core.IRepository
{
    public interface IPlaylistRepository
    {
        Task<ApiResponse> AddPlaylistFromYouTubeAsync(string playlistUrl, int CategoryId);
        Task<ApiResponse> GetPlaylistsByCategoryIdAsync(int categoryId);
        Task<ApiResponse> GetVideosByPlaylistIdAsync(int playlistId);
        Task<ApiResponse> DeletePlaylistAsync(int playlistId);
        Task<ApiResponse> UpdateVideoProgressAsync(string studentId, int videoId, TimeSpan watchedDuration);
        Task<ApiResponse> GetErollmentCourses(string studentId);
        Task<ApiResponse> EnrollToCourse(string studentId, int playlistId);
        Task<ApiResponse> GetCourseById(int playlistId);
    }
}
