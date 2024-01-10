using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppScheduleDetailDto;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Dto.AppWarningDtos;
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
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    

    public class TeacherController : Controller
    {
        private readonly IAppSessionService _appSessionService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IAppScheduleService _appScheduleService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppWarningService _appWarningService;
        private readonly IAppBranchService _appBranchService;
        private readonly IAppStudentService _appStudentService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppLessonService _appLessonService;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public TeacherController(IAppSessionService appSessionService, IAppScheduleDetailService appScheduleDetailService, IAppScheduleService appScheduleService, IAppUserSessionService appUserSessionService, IAppWarningService appWarningService, IAppBranchService appBranchService, IAppStudentService appStudentService, IAppUserSessionLessonService appUserSessionLessonService, IAppLessonService appLessonService, IWebHostEnvironment webHostEnvironment)
        {
            _appSessionService = appSessionService;
            _appScheduleDetailService = appScheduleDetailService;
            _appScheduleService = appScheduleService;
            _appUserSessionService = appUserSessionService;
            _appWarningService = appWarningService;
            _appBranchService = appBranchService;
            _appStudentService = appStudentService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appLessonService = appLessonService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int sessionId,int userId)
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
                ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
                ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(sessionId);
                var schedules = await _appScheduleDetailService.GetScheduleDetailsForTeacher(sessionId, myUserId);
                ViewBag.sessionId = sessionId;
                ViewBag.userId = myUserId;
                string sessionName = await _appSessionService.ReturnSessionName(sessionId);
                ViewBag.sessionName = sessionName;
                ViewBag.sessionName2 = sessionName.Replace("/", "_");
                var mylist= await _appScheduleService.GetSCheduleIdsForTeacher(myUserId, sessionId);
                return View(mylist);

            }
            else
            {
                ViewBag.sessionName = "Aktif Bir Dönem İçerisinde Bulunmamaktasınız";
                return View(new List<int>());
            }
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TeacherLessons(int sessionId,int userId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(sessionId);
            ViewBag.sessionId = sessionId;
            ViewBag.sessionName = await _appSessionService.ReturnSessionName(sessionId);
            List<TeacherLessonsBranchedPreviewModel> models = new List<TeacherLessonsBranchedPreviewModel>();
            var schedules = await _appScheduleDetailService.GetScheduleDetailsForTeacher(sessionId, userId);
            var distinctSchedules = schedules
    .GroupBy(x => new { x.AppSchedule.AppSessionBranch.BranchId })
    .Select(group => group.First())
    .ToList();
            foreach (var schedule in distinctSchedules) {
                var scheduleDetailList = await _appScheduleDetailService.GetScheduleDetailsByScheduleIdDistinct(schedule.AppSchedule.Id);
                var branchName = await _appScheduleService.GetBranchNameByScheduleId(schedule.AppSchedule.Id);
                foreach(var detail in scheduleDetailList)
                {
                    TeacherLessonsBranchedPreviewModel model = new TeacherLessonsBranchedPreviewModel
                    {
                        BranchId = detail.AppSchedule.AppSessionBranch.BranchId,
                        BranchDefinition = branchName,
                        LessonId=detail.AppLesson.Id,
                        LessonDefinition = detail.AppLesson.Definition
                };
                    models.Add(model);
                }
                
            }

            return View(models);
        }
      
        public async Task<IActionResult> BranchNotes(int branchId,int sessionId, int lessonId)
        {
            
                ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            ViewBag.lessonId = lessonId;
            ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(sessionId);
            ViewBag.sessionId = sessionId;
                ViewBag.branchId = branchId;
                string branchName = await _appBranchService.BranchNameByByBranchId(branchId);
                ViewBag.branchName = branchName;
                ViewBag.branchName2 = branchName.ToUpper().Replace("/", "_");
                string sessionName = await _appSessionService.ReturnSessionName(sessionId);
                ViewBag.sessionName = sessionName;
                ViewBag.sessionName2 = sessionName.ToUpper().Replace("/", "_");


                List<AppTeacherStudentListModel> studentList = new();
                await _appUserSessionService.TotalAverageAllUsers(sessionId);


            //BURASI


            var students = await _appStudentService.GetStudentsWithBranchAndSessionAndLessonAsync(branchId, sessionId, lessonId);

            ViewBag.lessonId = lessonId;
            ViewBag.sessionId = sessionId;
            ViewBag.branchId = branchId;
                foreach (var student in students)
                {
                    int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(student.Id, sessionId);

                    double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
                    double twc = await _appWarningService.TotalWarningCountByUserId(student.Id, sessionId);
                    await _appWarningService.ChangeStudentStatusBecasuseOfWarning(student.Id, swc, twc, userSessionId);


                    studentList.Add(new AppTeacherStudentListModel
                    {
                        FirstName = student.FirstName,
                        SecondName = student.SecondName,
                        Email=student.Email,
                        Phone=student.PhoneNumber,
                        LessonNote= await _appUserSessionLessonService.GetLessonNoteByUserSessionIdAndLessonId(userSessionId,lessonId),
                        Devamsızlık = await _appUserSessionLessonService.GetLessonDevamsByUserSessionIdAndLessonId(userSessionId,lessonId),
                        
                        StudentSessionId=userSessionId,
                        SıraNo=student.SıraNo,

                        StudentId = student.Id,
                        SessionId = sessionId,
                        //BURASI
                        Status = await _appUserSessionService.GetUserSessionStatus(userSessionId),
                        
                        //BURASI

                        
                        Average = await _appUserSessionService.ReturnSessionAverage(userSessionId),
                        
                    });; ;

                    double swc2 = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
                    double twc2 = await _appWarningService.TotalWarningCountByUserIdGeneral(student.Id);
                    await _appWarningService.ChangeStudentStatusBecasuseOfWarning(student.Id, swc2, twc2, userSessionId);
                }
                return View(studentList);
            
        }


        [HttpGet]
        [Authorize(Roles = "Teacher")]

        public async Task<IActionResult> TeacherNotes(int userSessionId, int lessonId)
        {

            var userSessionLessons = await _appUserSessionLessonService.GetAppUserSessionLessonsByUserSessionIdAndLessonId(userSessionId,lessonId);
            return this.View(userSessionLessons.Data);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]

        public async Task<IActionResult> TeacherNotes(List<AppUserSessionLessonUpdateDto> dtos)
        {
            int userSessionId = dtos[0].UserSessionId;
            int lessonId = dtos[0].LessonId;
            int userId = await _appUserSessionService.GetUserIdByUserSessionId(userSessionId);
            string userName = await _appUserSessionService.GetUserNameByUserSessionId(userSessionId);

            int sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);
            //BURASI

            int branchId = await _appUserSessionService.GetBranchIdByUserSessionId(userSessionId);


            await _appUserSessionLessonService.UpdateUserSessionLessonsAsync(dtos);
            foreach (var dto in dtos)
            {
                if (dto.Not != -1)
                {
                    await _appUserSessionService.FindAverageOfSessionWithUserAndSession(userId, sessionId);
                    await _appUserSessionService.TotalAverageByUserId(userId, sessionId);
                }
            }

            double average = await _appUserSessionService.ReturnSessionAverage(userSessionId);

            int notminuscount = 0;
            foreach (var dto in dtos)
            {
                string lesson = await _appLessonService.GetLessonNameByLessonId(dto.LessonId);

                if (dto.Not != -1 && dto.Not < 50)
                {
                    var newdto = new AppWarningCreateDto
                    {
                        UserSessionId = userSessionId,
                        WarningCount = 0.5,
                        WarningReason = $"{userName} ismindeki öğrenci {lesson} isimli dersten 50'nin altında not aldığı için DERS BAŞARI İHTARI almıştır",
                        WarningTime = DateTime.Now
                    };
                    await _appWarningService.CreateWarningByDtoandString(newdto, lesson, userId, userSessionId);

                }
                else
                {
                    await _appWarningService.RemoveWarningByString(lesson, userId, userSessionId);

                }
                if (dto.Not != -1)
                {
                    notminuscount += 1;
                }


            }
            if (dtos.Count == notminuscount)
            {
                if (average < 70)
                {
                    var newdto = new AppWarningCreateDto
                    {
                        UserSessionId = userSessionId,
                        WarningCount = 1,
                        WarningReason = $"{userName} isimli öğrenci; dönem ortalaması 70'in altında olması sebebiyle DÖNEM BAŞARISIZLIĞI İHTARI almıştır",
                        WarningTime = DateTime.Now
                    };
                    await _appWarningService.CreateWarningByDtoandString(newdto, "dönem ortalaması", userId, userSessionId);
                }
                else
                {
                    await _appWarningService.RemoveWarningByString("dönem ortalaması", userId, userSessionId);
                }
            }



            return RedirectToAction("BranchNotes", "Teacher", new { sessionId = sessionId, branchId = branchId ,lessonId= lessonId});
        }

        public async Task<IActionResult> StudentDetailsForTeacher(int userSessionId,int lessonId)
        {


            var userId = await _appUserSessionService.GetUserIdByUserSessionId(userSessionId);

            var sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            ViewBag.lessonId = lessonId;
            int classId = await _appUserSessionService.GetClassIdByUserSessionId(userSessionId);

            //BURASI
            var branchId = await _appUserSessionService.GetBranchIdByUserSessionId(userSessionId);

            double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
            double twc = await _appWarningService.TotalWarningCountByUserId(userId, sessionId);
            await _appWarningService.ChangeStudentStatusBecasuseOfWarning(userId, swc, twc, userSessionId);

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

            return View(model);
        }
        [Authorize(Roles = "Teacher")]

        public async Task<IActionResult> TeacherExSessions(int userId)
        {
            var sessions = await _appSessionService.TeacherExSessionsAsync(userId);
            ViewBag.userId = userId;
            return View(sessions);
        }
        [Authorize(Roles = "Teacher")]

        public async Task<IActionResult> TeacherExSessionDetails(int userId,int sessionId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(sessionId);
            ViewBag.userId = userId;
            ViewBag.sessionId = sessionId;
            ViewBag.sessionName = await _appSessionService.ReturnSessionName(sessionId);
            return View();
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
                    var razorPageUrl = Url.Action("Index", "Teacher", new { sessionId = sessionId, userId = userId }, Request.Scheme);
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

        public async Task<IActionResult> GenerateAndDownloadPdf2(string myFileName, int sessionId, int branchId,int lessonId)
        {
            // PDF oluştur
            await GeneratePdfAsync2(myFileName, sessionId, branchId,lessonId);

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

        private async Task GeneratePdfAsync2(string myFileName, int sessionId, int branchId,int lessonId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("BranchNotes", "Teacher", new { sessionId = sessionId, branchId = branchId,lessonId=lessonId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['İsim', 'Soyisim','Durum','Sıra No','Ders Notu', 'Devamsızlık', 'Dönem Ortalaması'];

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
