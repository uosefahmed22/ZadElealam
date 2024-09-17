using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Client.TranslationServices;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IServices;
using ZadElealam.Core.Models.Auth;
using ZadElealam.Repository.Data;

namespace ZadElealam.Repository.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CertificateService(AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> DownloadCertificate(string? studentId, int examId)
        {
            try
            {
                var studentExamData = await _context.StudentExams
                    .Where(se => se.StudentId == studentId && se.ExamId == examId && se.IsPassed)
                    .Select(se => new
                    {
                        se.CompletionDate,
                        ExamTitle = se.Exam.Playlist.Title
                    })
                    .FirstOrDefaultAsync();

                var student = await _userManager.FindByIdAsync(studentId);

                if (student == null)
                {
                    throw new Exception("Student not found.");
                }

                if (studentExamData == null)
                {
                    throw new Exception("Student exam not found.");
                }

                var pdfBytes = await GenerateCertificate(student.FullName, studentExamData.ExamTitle, studentExamData.CompletionDate);

                return new FileContentResult(pdfBytes, "application/pdf")
                {
                    FileDownloadName = "Certificate.pdf"
                };
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiResponse(400, ex.Message));
            }
        }

        private static async Task<string> TranslateText(string inputText, string targetLanguage)
        {
            if (string.IsNullOrEmpty(inputText))
            {
                throw new ArgumentException("Input text cannot be null or empty.", nameof(inputText));
            }

            // Prepare the translation API request (MyMemory API)
            var client = new HttpClient();
            var endpoint = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(inputText)}&langpair=ar|{targetLanguage}";

            var response = await client.GetAsync(endpoint).ConfigureAwait(false);
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Deserialize the response into a structured object
            var result = JsonConvert.DeserializeObject<MyMemoryTranslationResponse>(responseBody);

            if (result != null && result.ResponseData != null)
            {
                return result.ResponseData.TranslatedText; // Return translated text
            }

            throw new Exception("Translation failed. No result returned.");
        }
        private class MyMemoryTranslationResponse
        {
            public ResponseData ResponseData { get; set; }
        }
        private class ResponseData
        {
            public string TranslatedText { get; set; }
        }
        private static async Task<byte[]> GenerateCertificate(string studentName, string courseTitle, DateTime completionDate)
        {
            ValidateInput(studentName, nameof(studentName));
            ValidateInput(courseTitle, nameof(courseTitle));

            studentName = await TranslateIfArabic(studentName);
            courseTitle = await TranslateIfArabic(courseTitle);

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document document = new Document(pdfDoc, PageSize.A4);

                document.Add(new Paragraph("Certificate of Completion")
                    .SetFontSize(24)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                document.Add(new Paragraph("This is to certify that")
                    .SetFontSize(24)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                document.Add(new Paragraph(studentName)
                    .SetFontSize(20)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                document.Add(new Paragraph("has successfully completed the course")
                    .SetFontSize(24)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                document.Add(new Paragraph(courseTitle)
                    .SetFontSize(20)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                document.Add(new Paragraph($"on {completionDate:dd MMMM yyyy}")
                    .SetFontSize(18)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                document.Close();

                return ms.ToArray();
            }
        }
        private static void ValidateInput(string input, string paramName)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{paramName} cannot be null or empty.", paramName);
        }
        private static async Task<string> TranslateIfArabic(string text)
        {
            return IsArabic(text) ? await TranslateText(text, "en") : text;
        }
        private static bool IsArabic(string text)
        {
            // Regular expression to detect Arabic characters
            return Regex.IsMatch(text, @"\p{IsArabic}");
        }
    }
}
