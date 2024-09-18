using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;
using ZadElealam.Core.Models.Auth;
using ZadElealam.Repository.Repository;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly UserManager<AppUser> _userManager;

        public ExamController(IExamRepository examRepository,
            UserManager<AppUser> userManager)
        {
            _examRepository = examRepository;
            _userManager = userManager;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("create-exam")]
        public async Task<IActionResult> CreateExam([FromBody] CreateExamDto examDto)
        {
            var questions = examDto.Questions.Select(q => new Question
            {
                Text = q.Text,
                Answers = q.Answers.Select(a => new Answer { Text = a.Text }).ToList(),
                CorrectAnswerId = q.CorrectAnswerId
            }).ToList();

            var result = await _examRepository.CreateExamAsync(examDto.PlaylistId, examDto.Title, questions);
            if (result.StatusCode == 200)
            {
                return Ok(new ApiResponse(200, result.Data));
            }

            return BadRequest(new ApiResponse(400, result.Message));
        }
        
        [HttpGet("get-exam-by-playlist")]
        public async Task<IActionResult> GetExamByPlaylistId(int playlistId)
        {
            var result = await _examRepository.GetExamsByPlaylistIdAsync(playlistId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("submit-exam")]
        public async Task<IActionResult> SubmitExam(int examId, Dictionary<int, int> StudentAnswers)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var student = await _userManager.FindByEmailAsync(email);
            
            var result = await _examRepository.SubmitExamAsync(student.Id, examId, StudentAnswers);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("get-all-student-exams")]
        public async Task<IActionResult> GetAllStudentExams()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var student = await _userManager.FindByEmailAsync(email);
            var result = await _examRepository.GetAllStudentExamsAsync(student.Id);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("delete-exam")]
        public async Task<IActionResult> DeleteExam(int examId)
        {
            var result = await _examRepository.DeleteExamAsync(examId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpGet("get-exam-by-id")]
        public async Task<IActionResult> GetExamById(int examId)
        {
            var result = await _examRepository.GetExamByIdAsync(examId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
