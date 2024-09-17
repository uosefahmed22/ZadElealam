using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.IServices
{
    public interface ICertificateService
    {
        Task<IActionResult> DownloadCertificate(string studentId, int examId);
    }
}
