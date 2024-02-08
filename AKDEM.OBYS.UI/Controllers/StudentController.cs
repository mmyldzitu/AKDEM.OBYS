using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{

    
    public class StudentController : Controller
    {
        private readonly IAppSessionService _appSessionService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IAppScheduleService _appScheduleService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppWarningService _appWarningService;
        private readonly IAppStudentService _appStudentService;
        private readonly IAppBranchService _appBranchService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppUserService _appUserService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppGraduatedService _appGraduatedService;


        public StudentController(IAppSessionService appSessionService, IAppScheduleDetailService appScheduleDetailService, IAppScheduleService appScheduleService, IAppUserSessionService appUserSessionService, IAppWarningService appWarningService, IAppStudentService appStudentService, IAppUserSessionLessonService appUserSessionLessonService, IAppBranchService appBranchService, IAppUserService appUserService, IWebHostEnvironment webHostEnvironment, IAppGraduatedService appGraduatedService)
        {
            _appSessionService = appSessionService;
            _appScheduleDetailService = appScheduleDetailService;
            _appScheduleService = appScheduleService;
            _appUserSessionService = appUserSessionService;
            _appWarningService = appWarningService;
            _appStudentService = appStudentService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appBranchService = appBranchService;
            _appUserService = appUserService;
            _webHostEnvironment = webHostEnvironment;
            _appGraduatedService = appGraduatedService;
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
        public async Task<IActionResult> Index(int sessionId, int userId = 0)
        {
            int myUserId = 0;
            if (userId == 0)
            {
                myUserId = int.Parse((User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)).Value);

            }
            else
            {
                myUserId = userId;
            }
            if (sessionId != 0)
            {
                var control = await _appUserSessionService.IfThereIsAnyUserSession(myUserId, sessionId);
                if (control)
                {
                    int scheduleId = await _appScheduleService.GetScheduleIdByUserAndSessionId(sessionId, myUserId);
                    ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
                    ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(sessionId);
                    string scheduleName = await _appScheduleService.GetNameByScheduleId(scheduleId);

                    ViewBag.scheduleId = scheduleId;
                    string sessionName = await _appSessionService.ReturnSessionName(sessionId);
                    string name = $"{scheduleName} ({sessionName})";
                    string name2 = name.Replace("/", "_");
                    ViewBag.name = name;
                    ViewBag.name2 = name2;
                    ViewBag.userId = myUserId;
                    ViewBag.sessionId = sessionId;
                    ViewBag.sessionBranchId = await _appScheduleService.GetSessionBranchIdByAppScheduleId(scheduleId);
                    if (scheduleId == 0)
                    {
                        ViewBag.name = "Ders Programınız Henüz Oluşturulmadı";

                    }

                    string header = $"{name}";
                    string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                    headerforPdf = headerforPdf.ToUpper();
                    headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                    ViewBag.headerforPdf = headerforPdf;

                    ViewBag.status = 1;
                    return View();
                }
                else
                {
                    string header = $"Program";
                    string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                    headerforPdf = headerforPdf.ToUpper();
                    headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                    ViewBag.headerforPdf = headerforPdf;
                    ViewBag.status = 0;
                    return View();
                }




            }
            else
            {
                string header = $"Program";
                string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                headerforPdf = headerforPdf.ToUpper();
                headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                ViewBag.headerforPdf = headerforPdf;

                ViewBag.name = "";
                ViewBag.status = 0;
                return View();
            }
        }
        public async Task<IActionResult> SessionCriterias(int sessionId, int userId = 0)
        {
            int myUserId = 0;
            if (userId == 0)
            {
                myUserId = int.Parse((User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)).Value);

            }
            else
            {
                myUserId = userId;
            }
            if (sessionId != 0)
            {
                ViewBag.userId = myUserId;
                var control = await _appUserSessionService.IfThereIsAnyUserSession(myUserId, sessionId);
                if (control)
                {
                    var sessionCriterias = await _appSessionService.SessionCriterias(sessionId);
                    string sessionName = await _appSessionService.ReturnSessionName(sessionId);
                    ViewBag.sessionName = sessionName;
                    ViewBag.sessionName2 = sessionName.Replace("/", "_");
                    ViewBag.sessionId = sessionId;

                    string header = $"{sessionName}_Dönem_yönetmeliği";
                    string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                    headerforPdf = headerforPdf.ToUpper();
                    headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                    ViewBag.headerforPdf = headerforPdf;

                    return View(sessionCriterias);
                }
                else
                {
                    string header = $"Yönetmelik";
                    string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                    headerforPdf = headerforPdf.ToUpper();
                    headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                    ViewBag.headerforPdf = headerforPdf;
                    return View(new AppSessionListDto { Definition = "NotExists" });

                }

            }
            else
            {
                string header = $"Yönetmelik";
                string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                headerforPdf = headerforPdf.ToUpper();
                headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                ViewBag.headerforPdf = headerforPdf;
                return View(new AppSessionListDto { Definition = "NotExists" });
            }

        }
        public async Task<IActionResult> StudentDetailsStudent(int userId, int sessionId)
        {

            if (sessionId != 0)
            {
                var control = await _appUserSessionService.IfThereIsAnyUserSession(userId, sessionId);
                if (control)
                {
                    int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);


                    ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);

                    int classId = await _appUserSessionService.GetClassIdByUserSessionId(userSessionId);

                    //BURASI
                    var branchId = await _appUserSessionService.GetBranchIdByUserSessionId(userSessionId);
                    double slwc = await _appWarningService.SessionLessonWarningCountByUserSessionId(userSessionId);
                    double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId, slwc);
                    double awc = await _appWarningService.AbsenteismWarningCountByUserSessionId(userSessionId);
                    double twc = await _appWarningService.TotalWarningCountByUserId(userId, sessionId);
                    await _appWarningService.ChangeStudentStatusBecasuseOfWarning(userId, swc, awc, twc, userSessionId);

                    string sessionName = await _appSessionService.ReturnSessionName(sessionId);
                    ViewBag.sessionName = sessionName;
                    ViewBag.sessionName2 = sessionName.ToUpper().Replace("/", "_");
                    ViewBag.userSessionId = userSessionId;
                    ViewBag.sessionId = sessionId;
                    ViewBag.branchId = branchId;
                    var student = await _appStudentService.GetStudentById(userId);
                    var userSessionlessons = await _appUserSessionLessonService.GetAppUserSessionLessonsByUserSessionId(userSessionId);

                    StudentDetailSessionListModel studentDetailSessionListModel = new StudentDetailSessionListModel
                    {
                        UserId = userId,
                        ClassId = classId,
                        BranchId = branchId,
                        AppBranch = await _appBranchService.GetBrancWithId(branchId),
                        AppClass = await _appBranchService.GetClassById(classId),
                        Average = await _appUserSessionService.ReturnSessionAverage(userSessionId),
                        SessionId = sessionId,
                        Status = await _appUserSessionService.GetUserSessionStatus(userSessionId),
                        SessionWarningCount = await _appWarningService.ReturnSwc(userSessionId),
                        AbsenteismWarningCount = await _appWarningService.ReturnAwc(userSessionId),
                        LessonWarningCount = await _appWarningService.SessionLessonWarningCountByUserSessionId(userSessionId),
                        TotalAverage = await _appUserSessionService.ReturnTotalAverage(userId),
                        TotalWarningCount = await _appWarningService.ReturnTwc(userId)
                    };


                    StudentDetailsModel model = new StudentDetailsModel
                    {
                        AppStudent = student,
                        AppUserSessionLessons = userSessionlessons.Data,
                        BranchSessionDegree = await _appUserSessionService.ReturnSessionOrderOfBranch(student.Id, student.BranchId, sessionId),
                        BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, student.BranchId, sessionId),
                        TotalSessionDegree = await _appUserSessionService.ReturnSessionOrderOfClass(student.Id, student.BranchId, sessionId),
                        TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, student.BranchId, sessionId),
                        AppStudentSession = studentDetailSessionListModel
                    };
                    string header = $"{student.FirstName}_{student.SecondName}_{sessionName}";
                    string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                    headerforPdf = headerforPdf.ToUpper();
                    headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                    ViewBag.headerforPdf = headerforPdf;
                    return View(model);

                }
                else
                {
                    var student = await _appStudentService.GetStudentById(userId);

                    string header = $"{student.FirstName}_{student.SecondName}_dönem";
                    string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                    headerforPdf = headerforPdf.ToUpper();
                    headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                    ViewBag.headerforPdf = headerforPdf;
                    return View(new StudentDetailsModel { SessionName = "NotExists" });
                }

            }
            else
            {
                var student = await _appStudentService.GetStudentById(userId);

                string header = $"{student.FirstName}_{student.SecondName}_dönem";
                string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                headerforPdf = headerforPdf.ToUpper();
                headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                ViewBag.headerforPdf = headerforPdf;

                return View(new StudentDetailsModel { SessionName = "NotExists" });
            }


        }

        public async Task<IActionResult> StudentWarningDetails(int sessionId, int userId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.ToUpper().Replace("/", "_");
            ViewBag.sessionId = sessionId;
            ViewBag.userId = userId;
            string userName = await _appUserService.GetUserNameById(userId);
            ViewBag.userName = userName;
            int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
            ViewBag.userSessionId = userSessionId;
            var warningList = await _appWarningService.AppWarningByUserSessionId(userSessionId);

            var status = await _appStudentService.ReturnStatusOfStudent(userId);
            var departReason = await _appStudentService.ReturnDepartReasonOfStudent(userId);

            string header = $"{sessionName}_{userName}_İhtarları";
            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;
            return View(new StudentWarningsModel { AppWarnings = warningList, Status = status, DepartReason = departReason });



        }
        

        public async Task<IActionResult> StudentExSessions(int userId)
        {
            var sessions = await _appSessionService.StudentExSessionsAsync(userId);
            ViewBag.userId = userId;
            return View(sessions);
        }
        

        public async Task<IActionResult> StudentExSessionDetails(int userId, int sessionId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(sessionId);
            ViewBag.userId = userId;
            ViewBag.sessionId = sessionId;
            ViewBag.sessionName = await _appSessionService.ReturnSessionName(sessionId);
            return View();
        }
        public async Task<IActionResult> Certificate(int userId)
        {

            var list = await _appGraduatedService.CertificaofUser(userId);

            string header = $"{list.studentName}_Sertifika";
            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;

            return View(list);
        }
        public async Task<IActionResult> StudentTranscrypt(int userId)
        {
            var user = await _appStudentService.GetStudentById(userId);
            ViewBag.userId = userId;
            List<StudentDetailsModel> models = new();
            var userSessionIds = await _appUserSessionService.GetUserSessionIdsByUserIdAsync(userId);
            foreach (var userSessionId in userSessionIds)
            {

                var sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);
                double slwc = await _appWarningService.SessionLessonWarningCountByUserSessionId(userSessionId);
                double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId, slwc);
                double awc = await _appWarningService.AbsenteismWarningCountByUserSessionId(userSessionId);
                double twc = await _appWarningService.TotalWarningCountByUserId(userId, sessionId);
                await _appWarningService.ChangeStudentStatusBecasuseOfWarning(userId, swc, awc, twc, userSessionId);
                await _appUserSessionService.TotalAverageAllUsers(sessionId);
                //BURASI
                int classId = await _appUserSessionService.GetClassIdByUserSessionId(userSessionId);
                var branchId = await _appUserSessionService.GetBranchIdByUserSessionId(userSessionId);
                ViewBag.sessionId = sessionId;
                ViewBag.branchId = branchId;
                var student = await _appStudentService.GetStudentById(userId);
                ViewBag.firstName = student.FirstName;
                ViewBag.secondName = student.SecondName;
                ViewBag.ImagePath = student.ImagePath;

                var userSessionlessons = await _appUserSessionLessonService.GetAppUserSessionLessonsByUserSessionId(userSessionId);
                StudentDetailSessionListModel studentDetailSessionListModel = new StudentDetailSessionListModel
                {
                    Status = await _appUserSessionService.GetUserSessionStatus(userSessionId),

                    ClassId = classId,
                    BranchId = branchId,
                    AppBranch = await _appBranchService.GetBrancWithId(branchId),
                    AppClass = await _appBranchService.GetClassById(classId),
                    UserId = userId,
                    Average = await _appUserSessionService.ReturnSessionAverage(userSessionId),
                    SessionId = sessionId,
                    SessionWarningCount = await _appWarningService.ReturnSwc(userSessionId),
                    AbsenteismWarningCount = await _appWarningService.ReturnAwc(userSessionId),
                    LessonWarningCount = await _appWarningService.SessionLessonWarningCountByUserSessionId(userSessionId),
                    TotalAverage = await _appUserSessionService.ReturnTotalAverage(userId),
                    TotalWarningCount = await _appWarningService.ReturnTwc(userId)
                };


                StudentDetailsModel model = new StudentDetailsModel
                {
                    AppStudent = student,

                    SessionName = await _appSessionService.ReturnSessionName(sessionId),
                    AppUserSessionLessons = userSessionlessons.Data,
                    //BURASI
                    BranchSessionDegree = await _appUserSessionService.ReturnSessionOrderOfBranch(student.Id, student.BranchId, sessionId),
                    //BURASI
                    BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, student.BranchId, sessionId),
                    TotalSessionDegree = await _appUserSessionService.ReturnSessionOrderOfClass(student.Id, student.BranchId, sessionId),
                    TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, student.BranchId, sessionId),
                    AppStudentSession = studentDetailSessionListModel
                };
                models.Add(model);

            }
            TranscryptListModel transcryptListModel = new TranscryptListModel { AppStudent = user, StudentDetails = models };

            var student2 = await _appStudentService.GetStudentById(userId);

            string header = $"{student2.FirstName}_{student2.SecondName}_Transkript";
            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;


            return View(transcryptListModel);

        }
        public async Task<IActionResult> StudentAllWarningDetails(int userId)
        {

            ViewBag.userId = userId;
            string userName= await _appUserService.GetUserNameById(userId);
            ViewBag.userName = userName;

            var warningList = await _appWarningService.AppWarningByUserId(userId);
            
                var status = await _appStudentService.ReturnStatusOfStudent(userId);
                var departReason = await _appStudentService.ReturnDepartReasonOfStudent(userId);

            string header = $"{userName}_İhtarlar";
            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;
            return View(new StudentWarningsModel { AppWarnings = warningList, Status = status, DepartReason = departReason });
            
            


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
                    var razorPageUrl = Url.Action("Index", "Student", new { sessionId = sessionId, userId = userId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['Saat', 'Pazartesi','Salı','Çarşamba','Perşembe', 'Cuma', 'Cumartesi', 'Pazar'];

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



        public async Task<IActionResult> GenerateAndDownloadPdf2(string myFileName, int userId)
        {
            // PDF oluştur
            await GeneratePdfAsync2(myFileName, userId);

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

        private async Task GeneratePdfAsync2(string myFileName, int userId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("StudentAllWarningDetails", "Student", new { userId = userId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['İhtar Katkısı','İhtar Sebebi','İhtar Tarihi'];

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



        public async Task<IActionResult> GenerateAndDownloadPdf3(string myFileName, int userId, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsync3(myFileName, userId, sessionId);

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

        private async Task GeneratePdfAsync3(string myFileName, int userId, int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("StudentWarningDetails", "Student", new { userId = userId, sessionId = sessionId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['İhtar Katkısı','İhtar Sebebi','İhtar Tarihi'];

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
        public async Task<IActionResult> GenerateAndDownloadPdfCertificate(string myFileName, int userId)
        {
            // PDF oluştur
            await GeneratePdfAsyncCertificate(myFileName, userId);

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

        private async Task GeneratePdfAsyncCertificate(string myFileName, int userId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("Certificate", "Student", new { userId = userId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('buttons');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                

                                

                                // Diğer elementleri gizle (isteğe bağlı)
                                document.querySelectorAll('th, td').forEach(element => {
                                    const columnIndex = element.cellIndex;
                                    const columnHeader = document.querySelector('th:nth-child(' + (columnIndex + 1) + ')');

                                    if (columnHeader && !selectedColumns.includes(columnHeader.innerText.trim())) {
                                        element.style.display = 'none';
                                    }
                                });
                            }");

                    var divSelector = "div#mycont";
                    var divContent = await page.EvaluateFunctionAsync<string>(@"(selector) => {
                const element = document.querySelector(selector);
                return element ? element.innerHTML : null;
  
                                

            }", divSelector);

                    if (divContent != null)
                    {
                        // PDF boyut ve diğer seçenekleri belirle
                        var pdfOptions = new PdfOptions
                        {
                            Width = "1200px",
                            Height = "832px",
                            PrintBackground = true // Arka plan rengini ve resimleri dahil et
                                                   // Diğer isteğe bağlı seçenekleri ekleyebilirsiniz
                        };

                        // Seçilen div içeriğini PDF'ye çevir
                        var pdfBuffer = await page.PdfDataAsync(pdfOptions);

                        // PDF'yi kaydedin
                        var wwwrootPath = _webHostEnvironment.WebRootPath;
                        var pdfPath = Path.Combine(wwwrootPath, "pdf", $"{myFileName}.pdf");
                        System.IO.File.WriteAllBytes(pdfPath, pdfBuffer);
                    }
                    else
                    {
                        // Hata durumunu ele alabilirsiniz
                        Console.WriteLine("Belirtilen div bulunamadı.");
                    }

                }
            }
        }





        public async Task<IActionResult> GenerateAndDownloadStudentDetailsPdf(string myFileName, int userId, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsyncStudentDetails(myFileName, userId, sessionId);

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

        private async Task GeneratePdfAsyncStudentDetails(string myFileName, int userId, int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("StudentDetailsStudent", "Student", new { userId = userId, sessionId = sessionId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                         // PDF Oluştur butonunu gizle
                         const pdfButton = document.getElementById('pdfButton');
                     if (pdfButton) {
                                  pdfButton.style.display = 'none';
                            }
        const btnwarning = document.getElementById('btnwarnings');
                             if (btnwarning) {
                                          btnwarning.style.display = 'none';
                            }
                const turnclass = document.getElementById('turnclass');
                                             if (turnclass) {
                                                          turnclass.style.display = 'none';
                                            }

                    

                                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'Ders', 'Soyisim', 'Not', 'Devamsızlık'];

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


        public async Task<IActionResult> GenerateAndDownloadStudentTranscryptPdf(string myFileName, int userId)
        {
            // PDF oluştur
            await GeneratePdfAsyncStudentTranscrypt(myFileName, userId);

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
        private async Task GeneratePdfAsyncStudentTranscrypt(string myFileName, int userId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("StudentTranscrypt", "Student", new { userId = userId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                         // PDF Oluştur butonunu gizle
                         const pdfButton = document.getElementById('pdfButton');
                     if (pdfButton) {
                                  pdfButton.style.display = 'none';
                            }
        
                const turnList = document.getElementById('turnList');
                                             if (turnList) {
                                                          turnList.style.display = 'none';
                                            }

                    

                                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'Ders', 'Soyisim', 'Not', 'Devamsızlık'];

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
        public async Task<IActionResult> GenerateAndDownloadPdfStudentSessionCriterias(string myFileName, int sessionId, int userId)
        {
            // PDF oluştur
            await GeneratePdfAsyncStudentSessionCriterias(myFileName, sessionId, userId);

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

        private async Task GeneratePdfAsyncStudentSessionCriterias(string myFileName, int sessionId, int userId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("SessionCriterias", "Student", new { sessionId = sessionId, userId = userId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                         // PDF Oluştur butonunu gizle
                         const pdfButton = document.getElementById('pdfButton');
                     if (pdfButton) {
                                  pdfButton.style.display = 'none';
                            }
        

                    

                                     
                                

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
