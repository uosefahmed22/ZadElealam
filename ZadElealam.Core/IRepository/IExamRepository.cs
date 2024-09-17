using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Errors;
using ZadElealam.Core.Models;

namespace ZadElealam.Core.IRepository
{
    public interface IExamRepository
    {
        Task<ApiResponse> SubmitExamAsync(string studentId, int examId, Dictionary<int, int> studentAnswers);
        Task<ApiResponse> GetAllStudentExamsAsync(string userId);

        Task<ApiResponse> CreateExamAsync(int playlistId, string title, List<Question> questions);
        Task<ApiResponse> GetExamsByPlaylistIdAsync(int playlistId);
        Task<ApiResponse> GetExamByIdAsync(int examId);
        Task<ApiResponse> DeleteExamAsync(int examId);
    }
}
