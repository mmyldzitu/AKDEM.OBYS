using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    
    public class WarningController : Controller
    {
        private readonly IAppWarningService _appWarningService;
        private readonly IAppSessionService _appSessionService;
        private readonly IAppStudentService _appStudentService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppUserService _appUserService;

        public WarningController(IAppWarningService appWarningService, IAppSessionService appSessionService, IAppStudentService appStudentService, IAppUserSessionService appUserSessionService, IWebHostEnvironment webHostEnvironment, IAppUserService appUserService)
        {
            _appWarningService = appWarningService;
            _appSessionService = appSessionService;
            _appStudentService = appStudentService;
            _appUserSessionService = appUserSessionService;
            _webHostEnvironment = webHostEnvironment;
            _appUserService = appUserService;
        }
        char[] charactersToReplace = { '/', ' ', '\\', '?', ':', '.', ',' };
        char replacementChar = '_';
        static string ReplaceMultipleChars(string input, char[] charactersToReplace, char replacementChar)
        {
            foreach (char c in charactersToReplace)
            {
                input = input.Replace(c, replacementChar);
            }
            return input;
        }
        static string ConvertTurkishToEnglish(string input)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                switch (c)
                {
                    case 'Ç':
                        result.Append('C');
                        break;
                    case 'Ğ':
                        result.Append('G');
                        break;

                    case 'İ':
                        result.Append('I');
                        break;
                    case 'Ö':
                        result.Append('O');
                        break;
                    case 'Ş':
                        result.Append('S');
                        break;
                    case 'Ü':
                        result.Append('U');
                        break;
                    default:
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }
        public async Task <IActionResult> Index(int sessionId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            ViewBag.sessionId = sessionId;
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.Replace("/", "_");

            string header = $"{sessionName}_Dönemi_Öğrenci_İhtarları";
            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;


            var warnings = await _appWarningService.AppWarningBySessionId(sessionId);
            return View(warnings);
        }
        public async Task<IActionResult> PassiveStudents(int sessionId)
        {
            
            ViewBag.sessionId = sessionId;
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.Replace("/", "_");

            string header = $"{sessionName}_Dönemi_Başarısız_Öğrenciler";
            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;

            var users = await _appStudentService.GetPassiveStudents(sessionId);
            return View(users);
        }
        public async Task<IActionResult> StudentWarnings(int sessionId, int userId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.Replace("/", "_");
            ViewBag.sessionId = sessionId;
            ViewBag.userId = userId;
            string userName= await _appUserService.GetUserNameById(userId);
            ViewBag.userName = userName;
            int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
            ViewBag.userSessionId = userSessionId;
            string departReason = await _appStudentService.ReturnDepartReasonOfStudent(userId);
            var status = await _appStudentService.ReturnStatusOfStudent(userId);

            var warningList = await _appWarningService.AppWarningByUserSessionId(userSessionId);


            string header = $"{userName}_{sessionName}_Dönemi_İhtarları";
            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;

            return View(new StudentWarningsModel {AppWarnings=warningList,Status=status,DepartReason= departReason});

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveWarning(int sessionId, int userId, int id)
        {
            int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
            await _appWarningService.RemoveWarningById(id, userId, userSessionId);

            return RedirectToAction("StudentWarnings", new { sessionId = sessionId, userId = userId });
        }
        public async Task<IActionResult> GenerateAndDownloadPdf(string myFileName, int sessionId, int userId)
        {
            // PDF oluştur
            await GeneratePdfAsync(myFileName, sessionId, userId);

            // PDF dosyasının geçici konumunu belirtin
            var pdfPath = $"wwwroot/pdf/{myFileName}.pdf";

            // PDF dosyasının varlığını kontrol et
            if (System.IO.File.Exists(pdfPath))
            {
                // PDF dosyasını kullanıcıya indirme
                var fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                var fileName = $"{myFileName}.pdf";

                // PDF dosyasını indir
                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                // Hata durumunu ele alabilirsiniz
                return NotFound("PDF dosyası bulunamadı.");
            }
        }

        private async Task GeneratePdfAsync(string myFileName, int sessionId, int userId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("WarningDetails", "Otomation", new { sessionId = sessionId, userId = userId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
const studentButton = document.getElementById('turnToStudent');
                                if (studentButton) {
                                    studentButton.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['İhtar Katkısı', 'İhtar Sebebi','İhtar Tarihi'];

                                // Diğer elementleri gizle (isteğe bağlı)
                                document.querySelectorAll('th, td').forEach(element => {
                                    const columnIndex = element.cellIndex;
                                    const columnHeader = document.querySelector('th:nth-child(' + (columnIndex + 1) + ')');

                                    if (columnHeader && !selectedColumns.includes(columnHeader.innerText.trim())) {
                                        element.style.display = 'none';
                                    }
                                });
                            }");


                    // Razor sayfasının HTML içeriğini alın
                    var htmlContent = await page.GetContentAsync();

                    // HTML içeriğini PDF'ye çevir
                    var pdfBuffer = await page.PdfDataAsync();
                    var wwwrootPath = _webHostEnvironment.WebRootPath;
                    var pdfPath = Path.Combine(wwwrootPath, "pdf", $"{myFileName}.pdf");

                    // PDF'yi kaydedin
                    System.IO.File.WriteAllBytes(pdfPath, pdfBuffer);

                }
            }
        }



        public async Task<IActionResult> GenerateAndDownloadPdf2(string myFileName, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsync2(myFileName, sessionId);

            // PDF dosyasının geçici konumunu belirtin
            var pdfPath = $"wwwroot/pdf/{myFileName}.pdf";

            // PDF dosyasının varlığını kontrol et
            if (System.IO.File.Exists(pdfPath))
            {
                // PDF dosyasını kullanıcıya indirme
                var fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                var fileName = $"{myFileName}.pdf";

                // PDF dosyasını indir
                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                // Hata durumunu ele alabilirsiniz
                return NotFound("PDF dosyası bulunamadı.");
            }
        }

        private async Task GeneratePdfAsync2(string myFileName, int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("Index", "Warning", new { sessionId = sessionId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
const passiveStudents = document.getElementById('passiveStudents');
                                if (passiveStudents) {
                                    passiveStudents.style.display = 'none';
                                }
const turntoSession = document.getElementById('turntoSession');
                                if (turntoSession) {
                                    turntoSession.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['İhtar Katkısı', 'İhtar Sebebi','İhtar Tarihi'];

                                // Diğer elementleri gizle (isteğe bağlı)
                                document.querySelectorAll('th, td').forEach(element => {
                                    const columnIndex = element.cellIndex;
                                    const columnHeader = document.querySelector('th:nth-child(' + (columnIndex + 1) + ')');

                                    if (columnHeader && !selectedColumns.includes(columnHeader.innerText.trim())) {
                                        element.style.display = 'none';
                                    }
                                });
                            }");


                    // Razor sayfasının HTML içeriğini alın
                    var htmlContent = await page.GetContentAsync();

                    // HTML içeriğini PDF'ye çevir
                    var pdfBuffer = await page.PdfDataAsync();
                    var wwwrootPath = _webHostEnvironment.WebRootPath;
                    var pdfPath = Path.Combine(wwwrootPath, "pdf", $"{myFileName}.pdf");

                    // PDF'yi kaydedin
                    System.IO.File.WriteAllBytes(pdfPath, pdfBuffer);

                }
            }
        }


        public async Task<IActionResult> GenerateAndDownloadPdf3(string myFileName, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsync3(myFileName, sessionId);

            // PDF dosyasının geçici konumunu belirtin
            var pdfPath = $"wwwroot/pdf/{myFileName}.pdf";

            // PDF dosyasının varlığını kontrol et
            if (System.IO.File.Exists(pdfPath))
            {
                // PDF dosyasını kullanıcıya indirme
                var fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                var fileName = $"{myFileName}.pdf";

                // PDF dosyasını indir
                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                // Hata durumunu ele alabilirsiniz
                return NotFound("PDF dosyası bulunamadı.");
            }
        }

        private async Task GeneratePdfAsync3(string myFileName, int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("PassiveStudents", "Warning", new { sessionId = sessionId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
        const listButton = document.getElementById('turntolist');
                                if(listButton){
                                           listButton.style.display='none'     };
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'İsim','Soyisim','Sınıf','Genel Ortalama','Sebep'];

                                // Diğer elementleri gizle (isteğe bağlı)
                                document.querySelectorAll('th, td').forEach(element => {
                                    const columnIndex = element.cellIndex;
                                    const columnHeader = document.querySelector('th:nth-child(' + (columnIndex + 1) + ')');

                                    if (columnHeader && !selectedColumns.includes(columnHeader.innerText.trim())) {
                                        element.style.display = 'none';
                                    }
                                });
                            }");


                    // Razor sayfasının HTML içeriğini alın
                    var htmlContent = await page.GetContentAsync();

                    // HTML içeriğini PDF'ye çevir
                    var pdfBuffer = await page.PdfDataAsync();
                    var wwwrootPath = _webHostEnvironment.WebRootPath;
                    var pdfPath = Path.Combine(wwwrootPath, "pdf", $"{myFileName}.pdf");

                    // PDF'yi kaydedin
                    System.IO.File.WriteAllBytes(pdfPath, pdfBuffer);

                }
            }
        }

        public async Task<IActionResult> GenerateAndDownloadPdf4(string myFileName, int sessionId,int userId)
        {
            // PDF oluştur
            await GeneratePdfAsync4(myFileName, sessionId, userId);

            // PDF dosyasının geçici konumunu belirtin
            var pdfPath = $"wwwroot/pdf/{myFileName}.pdf";

            // PDF dosyasının varlığını kontrol et
            if (System.IO.File.Exists(pdfPath))
            {
                // PDF dosyasını kullanıcıya indirme
                var fileBytes = System.IO.File.ReadAllBytes(pdfPath);
                var fileName = $"{myFileName}.pdf";

                // PDF dosyasını indir
                return File(fileBytes, "application/pdf", fileName);
            }
            else
            {
                // Hata durumunu ele alabilirsiniz
                return NotFound("PDF dosyası bulunamadı.");
            }
        }

        private async Task GeneratePdfAsync4(string myFileName, int sessionId,int userId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("StudentWarnings", "Warning", new { sessionId = sessionId, userId=userId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                const listButton = document.getElementById('turntolist');
                                if(listButton){
                                           listButton.style.display='none'     };

                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'İhtar Katkısı','İhtar Sebebi','İhtar Tarihi'];

                                // Diğer elementleri gizle (isteğe bağlı)
                                document.querySelectorAll('th, td').forEach(element => {
                                    const columnIndex = element.cellIndex;
                                    const columnHeader = document.querySelector('th:nth-child(' + (columnIndex + 1) + ')');

                                    if (columnHeader && !selectedColumns.includes(columnHeader.innerText.trim())) {
                                        element.style.display = 'none';
                                    }
                                });
                            }");


                    // Razor sayfasının HTML içeriğini alın
                    var htmlContent = await page.GetContentAsync();

                    // HTML içeriğini PDF'ye çevir
                    var pdfBuffer = await page.PdfDataAsync();
                    var wwwrootPath = _webHostEnvironment.WebRootPath;
                    var pdfPath = Path.Combine(wwwrootPath, "pdf", $"{myFileName}.pdf");

                    // PDF'yi kaydedin
                    System.IO.File.WriteAllBytes(pdfPath, pdfBuffer);

                }
            }
        }
    }
}
