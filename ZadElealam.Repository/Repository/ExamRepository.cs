using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;
using ZadElealam.Repository.Data;

namespace ZadElealam.Repository.Repository
{
    public class ExamRepository : IExamRepository
    {
        private readonly AppDbContext _context;

        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> GetAllStudentExamsAsync(string userId)
        {
            try
            {
                var studentExams = await _context.StudentExams
                    .Include(se => se.Exam)
                    .Where(se => se.StudentId == userId)
                    .Select(se => new
                    {
                        se.Score,
                        se.IsPassed,
                        se.CompletionDate,
                        Exam = new
                        {
                            se.Exam.Id,
                            se.Exam.Title,
                        }
                    })
                    .ToListAsync();

                return new ApiResponse(200, studentExams);
            }
            catch (Exception ex)
            {
                return new ApiResponse(400, ex.Message);
            }
        }
        public async Task<ApiResponse> SubmitExamAsync(string studentId, int examId, Dictionary<int, int> studentAnswers)
        {
            try
            {
                var exam = await _context.Exams
                    .Include(e => e.Questions)
                    .FirstOrDefaultAsync(e => e.Id == examId);

                if (exam == null)
                {
                    return new ApiResponse(404, "الامتحان غير موجود.");
                }

                var studentExamInDb = await _context.StudentExams
                    .FirstOrDefaultAsync(se => se.StudentId == studentId && se.ExamId == examId && se.IsPassed);

                if (studentExamInDb != null)
                {
                    return new ApiResponse(400, $"{studentExamInDb.Score}%. لقد قمت بحل هذا الامتحان مسبقًا بنسبة نجاح");
                }

                if (studentAnswers.Count != exam.Questions.Count)
                {
                    return new ApiResponse(400, "عدد الإجابات المقدمة لا يتطابق مع عدد الأسئلة في الامتحان.");
                }

                int correctAnswers = 0;
                foreach (var question in exam.Questions)
                {
                    if (studentAnswers.TryGetValue(question.Id, out int answerId)) // Error handling for missing question IDs
                    {
                        Console.WriteLine($"Question ID: {question.Id}, Student Answer: {answerId}, Correct Answer: {question.CorrectAnswerId}");
                        if (question.CorrectAnswerId == answerId)
                        {
                            correctAnswers++;
                        }
                    }
                    else
                    {
                        return new ApiResponse(400, $"إجابة السؤال رقم {question.Id} مفقودة.");
                    }
                }

                int totalQuestions = exam.Questions.Count;
                int score = (int)(((double)correctAnswers / totalQuestions) * 100);
                bool isPassed = correctAnswers >= (totalQuestions * 0.6);

                var studentName = await _context.Users.FindAsync(studentId);
                if (studentName == null)
                {
                    return new ApiResponse(404, "الطالب غير موجود.");
                }

                var studentExam = new StudentExam
                {
                    StudentId = studentId,
                    ExamId = examId,
                    IsPassed = isPassed,
                    Score = score,
                    StudentName = studentName.FullName,
                    CompletionDate = DateTime.Now
                };

                _context.StudentExams.Add(studentExam);
                await _context.SaveChangesAsync();

                string resultMessage = isPassed
                    ? $"مبروووووووووووووووووووك! لقد نجحت في الامتحان بنسبة {score}%. نسأل الله لك التوفيق دائمًا."
                    : $"للأسف، لم تنجح في الامتحان هذه المرة. نتيجتك {score}%. نتمنى لك النجاح في المرة القادمة.";

                return new ApiResponse(200, resultMessage);
            }
            catch (ArgumentNullException ex)
            {
                return new ApiResponse(400, $"خطأ في المدخلات: {ex.Message}");
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse(500, $"خطأ في تحديث قاعدة البيانات: {ex.Message}");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, $"حدث خطأ غير متوقع: {ex.Message}");
            }
        }
        public async Task<ApiResponse> CreateExamAsync(int playlistId, string title, List<Question> questions)
        {
            try
            {
                var playlist = await _context.YouTubePlaylists.FindAsync(playlistId);
                if (playlist == null)
                {
                    return new ApiResponse(404, "قائمة التشغيل غير موجودة.");
                }

                var exam = new Exam
                {
                    Title = title,
                    PlaylistId = playlistId,
                    Questions = questions
                };

                _context.Exams.Add(exam);
                await _context.SaveChangesAsync();

                return new ApiResponse(200, "تم إنشاء الامتحان بنجاح.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> GetExamsByPlaylistIdAsync(int playlistId)
        {
            try
            {
                var exams = await _context.Exams
                    .Where(e => e.PlaylistId == playlistId)
                    .ToListAsync();

                if (exams == null || exams.Count == 0)
                {
                    return new ApiResponse(404, "لم يتم العثور على امتحانات لهذه القائمة.");
                }

                var examList = exams.Select(e => new
                {
                    Id = e.Id,
                    Title = e.Title
                }).ToList();

                return new ApiResponse(200, examList);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteExamAsync(int examId)
        {
            try
            {
                var ExsistingExam = await _context.Exams.FindAsync(examId);
                if (ExsistingExam == null)
                {
                    return new ApiResponse(404, "الامتحان غير موجود.");
                }
                _context.Exams.Remove(ExsistingExam);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "تم حذف الامتحان بنجاح.");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> GetExamByIdAsync(int examId)
        {
            try
            {
                var ExsistingExam = await _context.Exams.FindAsync(examId);
                if (ExsistingExam == null)
                {
                    return new ApiResponse(404, "الامتحان غير موجود.");
                }
                var exam = await _context.Exams
                    .Include(e => e.Questions)
                    .ThenInclude(q => q.Answers)
                    .FirstOrDefaultAsync(e => e.Id == examId);
                return new ApiResponse(200, exam);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}