using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Dto.AppWarningDtos;
using AKDEM.OBYS.UI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class OtomationController : Controller
    {
        private readonly IAppBranchService _appBranchService;
        private readonly IAppStudentService _appStudentService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppWarningService _appWarningService;
        private readonly IAppLessonService _appLessonService;
        private readonly IAppSessionService _appSessionService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppUserService _appUserService;


        public OtomationController(IAppBranchService appBranchService, IAppStudentService appStudentService, IAppUserSessionService appUserSessionService, IAppUserSessionLessonService appUserSessionLessonService, IAppWarningService appWarningService, IAppLessonService appLessonService, IAppSessionService appSessionService, IWebHostEnvironment webHostEnvironment, IAppUserService appUserService)
        {
            _appBranchService = appBranchService;
            _appStudentService = appStudentService;
            _appUserSessionService = appUserSessionService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appWarningService = appWarningService;
            _appLessonService = appLessonService;
            _appSessionService = appSessionService;
            _webHostEnvironment = webHostEnvironment;
            _appUserService = appUserService;
        }

        public async Task<IActionResult> Index(int sessionId)
        {
            var list = new List<AppBranchListDto>();
            //BURASI
            var items = await _appBranchService.GetBranchListWithSessionId(sessionId);
            foreach (var item in items)
            {
                list.Add(new AppBranchListDto
                {
                    Id = item.Id,
                    Definition = item.Definition
                }); ;
            }
            ViewBag.branches = new SelectList(list, "Id", "Definition");
            ViewBag.sessionId = sessionId;
            return View(new AppOtomationBranchListModel());

        }
        [HttpPost]
        public async Task<IActionResult> BranchDetails(AppOtomationBranchListModel model)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(model.SessionId);
            ViewBag.sessionId = model.SessionId;
            ViewBag.branchId = model.BranchId;
            string branchName = await _appBranchService.BranchNameByByBranchId(model.BranchId);
            ViewBag.branchName = branchName;
            ViewBag.branchName2 = branchName.ToUpper().Replace("/", "_");
            string sessionName = await _appSessionService.ReturnSessionName(model.SessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.ToUpper().Replace("/", "_");
            List<AppOtomationStudentListModel> studentList = new();
            await _appUserSessionService.TotalAverageAllUsers(model.SessionId);

            //BURASI
            var students = await _appStudentService.GetStudentsWithBranchAndSessionAsync(model.BranchId,model.SessionId);


            foreach (var student in students)
            {
                int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(student.Id, model.SessionId);
                double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
                double twc = await _appWarningService.TotalWarningCountByUserId(student.Id, model.SessionId);
                await _appWarningService.ChangeStudentStatusBecasuseOfWarning(student.Id, swc, twc, userSessionId);

                studentList.Add(new AppOtomationStudentListModel
                {
                    FirstName = student.FirstName,
                    SecondName = student.SecondName,
                    StudentId = student.Id,
                    SessionId = model.SessionId,
                    //BURASI
                    Status = await _appUserSessionService.GetUserSessionStatus(userSessionId),

                    BranchSessionDegree = await _appUserSessionService.ReturnSessionOrderOfBranch(student.Id, model.BranchId, model.SessionId),
                    //BURASI

                    BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, model.BranchId,model.SessionId),
                    

                    TotalSessionDegree = await _appUserSessionService.ReturnSessionOrderOfClass(student.Id, model.BranchId, model.SessionId),
                    

                    TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, model.BranchId,model.SessionId),
                    

                    Average = await _appUserSessionService.ReturnSessionAverage(userSessionId),
                    

                    TotalAverage = await _appUserSessionService.ReturnTotalAverage(student.Id),
                    

                    StudentSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(student.Id, model.SessionId),
                    

                    WarningCount = await _appWarningService.ReturnTwc(student.Id),
                    

                    SessionWarningCount = await _appWarningService.ReturnSwc(userSessionId)
                }) ; ;


                double swc2 = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
                double twc2 = await _appWarningService.TotalWarningCountByUserIdGeneral(student.Id);
                await _appWarningService.ChangeStudentStatusBecasuseOfWarning(student.Id, swc2, twc2, userSessionId);
            }
            return View(studentList);
        }
        [HttpGet]
        public async Task<IActionResult> BranchDetails(int sessionId, int branchId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);
            ViewBag.sessionId = sessionId;
            ViewBag.branchId = branchId;
            string branchName = await _appBranchService.BranchNameByByBranchId(branchId);
            ViewBag.branchName = branchName;
            ViewBag.branchName2 = branchName.ToUpper().Replace("/", "_");
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.ToUpper().Replace("/", "_");


            List<AppOtomationStudentListModel> studentList = new();
            await _appUserSessionService.TotalAverageAllUsers(sessionId);


            //BURASI


            var students = await _appStudentService.GetStudentsWithBranchAndSessionAsync(branchId,sessionId);


            foreach (var student in students)
            {
                int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(student.Id, sessionId);

                double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
                double twc = await _appWarningService.TotalWarningCountByUserId(student.Id, sessionId);
                await _appWarningService.ChangeStudentStatusBecasuseOfWarning(student.Id, swc, twc, userSessionId);


                studentList.Add(new AppOtomationStudentListModel
                {
                    FirstName = student.FirstName,
                    SecondName = student.SecondName,
                    StudentId = student.Id,
                    SessionId = sessionId,
                    //BURASI
                    Status = await _appUserSessionService.GetUserSessionStatus(userSessionId),
                    BranchSessionDegree = await _appUserSessionService.ReturnSessionOrderOfBranch(student.Id, student.BranchId, sessionId),
                    //BURASI

                    BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, student.BranchId, sessionId),
                    TotalSessionDegree = await _appUserSessionService.ReturnSessionOrderOfClass(student.Id, student.BranchId, sessionId),
                    TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, student.BranchId, sessionId),
                    Average = await _appUserSessionService.ReturnSessionAverage(userSessionId),
                    TotalAverage = await _appUserSessionService.ReturnTotalAverage(student.Id),
                    StudentSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(student.Id, sessionId),
                    WarningCount = await _appWarningService.ReturnTwc(student.Id),
                    SessionWarningCount = await _appWarningService.ReturnSwc(userSessionId)
                }) ; ;

                double swc2 = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
                double twc2 = await _appWarningService.TotalWarningCountByUserIdGeneral(student.Id);
                await _appWarningService.ChangeStudentStatusBecasuseOfWarning(student.Id, swc2, twc2, userSessionId);
            }
            return View(studentList);
        }


        [HttpGet]
        public async Task<IActionResult> Notes(int userSessionId)
        {

            var userSessionLessons = await _appUserSessionLessonService.GetAppUserSessionLessonsByUserSessionId(userSessionId);
            return this.View(userSessionLessons.Data);
        }
        [HttpPost]
        public async Task<IActionResult> Notes(List<AppUserSessionLessonUpdateDto> dtos)
        {
            int userSessionId = dtos[0].UserSessionId;
            int userId = await _appUserSessionService.GetUserIdByUserSessionId(userSessionId);
            string userName = await _appUserSessionService.GetUserNameByUserSessionId(userSessionId);

            int sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);
            //BURASI

            int branchId = await _appUserSessionService.GetBranchIdByUserSessionId(userSessionId);


            await _appUserSessionLessonService.UpdateUserSessionLessonsAsync(dtos);
            foreach(var dto in dtos)
            {
                if (dto.Not != -1)
                {
                    await _appUserSessionService.FindAverageOfSessionWithUserAndSession(userId, sessionId);
                    await _appUserSessionService.TotalAverageByUserId(userId,sessionId);
                }
            }
            
            double average = await _appUserSessionService.ReturnSessionAverage(userSessionId);

            int notminuscount = 0;
            foreach (var dto in dtos)
            {
                string lesson = await _appLessonService.GetLessonNameByLessonId(dto.LessonId);

                if (dto.Not != -1 && dto.Not <50)
                {
                    var newdto = new AppWarningCreateDto
                    {
                        UserSessionId = userSessionId,
                        WarningCount = 0.5,
                        WarningReason = $"{userName} ismindeki öğrenci {lesson} isimli dersten 50'nin altında not aldığı için DERS BAŞARI İHTARI almıştır",
                        WarningTime = DateTime.Now
                    };
                    await _appWarningService.CreateWarningByDtoandString(newdto, lesson,userId, userSessionId);

                }
                else
                {
                    await _appWarningService.RemoveWarningByString(lesson,userId, userSessionId);

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
            

            
            return RedirectToAction("BranchDetails", "Otomation", new { SessionId = sessionId, BranchId = branchId });
        }
        public async Task<IActionResult> StudentDetails(int userSessionId)
        {
            

            var userId = await _appUserSessionService.GetUserIdByUserSessionId(userSessionId);

            var sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);

            int classId = await _appUserSessionService.GetClassIdByUserSessionId(userSessionId);

            //BURASI
            var branchId = await _appUserSessionService.GetBranchIdByUserSessionId(userSessionId);

            double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId);
            double twc = await _appWarningService.TotalWarningCountByUserId(userId, sessionId);
            await _appWarningService.ChangeStudentStatusBecasuseOfWarning(userId, swc, twc, userSessionId);

            string sessionName= await _appSessionService.ReturnSessionName(sessionId);
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


            StudentDetailsModel model = new StudentDetailsModel { AppStudent = student,
                AppUserSessionLessons = userSessionlessons.Data,
                BranchSessionDegree = await _appUserSessionService.ReturnSessionOrderOfBranch(student.Id, student.BranchId, sessionId),
                BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, student.BranchId, sessionId),
                TotalSessionDegree = await _appUserSessionService.ReturnSessionOrderOfClass(student.Id, student.BranchId, sessionId),
                TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, student.BranchId, sessionId),
                AppStudentSession = studentDetailSessionListModel };

            return View(model);
        }
        public async Task<IActionResult> WarningDetails(int sessionId, int userId)
        {
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(sessionId);

            ViewBag.sessionId = sessionId;
            ViewBag.userId = userId;
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.Replace("/", "_");
            ViewBag.userName = await _appUserService.GetUserNameById(userId);
            int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
            ViewBag.userSessionId = userSessionId;
            var warningList = await _appWarningService.AppWarningByUserSessionId(userSessionId);
            return View(warningList);

        }
        public async Task<IActionResult> RemoveWarning(int sessionId, int userId, int id)
        {
            int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
            await _appWarningService.RemoveWarningById(id,userId,userSessionId);

            return RedirectToAction("WarningDetails", new { sessionId = sessionId, userId = userId });
        }
        public async Task<IActionResult> RemoveWarning2(int sessionId, int userId, int id)
        {
            int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
            await _appWarningService.RemoveWarningById(id, userId, userSessionId);

            return RedirectToAction("Index","Warning", new { sessionId = sessionId });
        }
        [HttpPost]
        
        public async Task<IActionResult> ChangeStudentStatusBecasuseOfWarning(int userId, double sessionWarningCount, double totalWarningCount)
        {

            await _appStudentService.ChangeStudentStatusBecasuseOfWarning(userId, sessionWarningCount, totalWarningCount);


            return Ok(new { success = true });


        }

        public async Task<IActionResult> GenerateAndDownloadPdf(string myFileName,int sessionId, int branchId)
        {
            // PDF oluştur
            await GeneratePdfAsync(myFileName,sessionId,branchId);

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

        private async Task GeneratePdfAsync(string myFileName,int sessionId,int branchId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("BranchDetails", "Otomation", new { sessionId = sessionId, branchId = branchId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                         // PDF Oluştur butonunu gizle
                         const pdfButton = document.getElementById('pdfButton');
                     if (pdfButton) {
                                  pdfButton.style.display = 'none';
                            }
        const paragraph = document.getElementById('paragraph');
                             if (paragraph) {
                                          paragraph.style.display = 'none';
                            }

                    

                                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'İsim', 'Soyisim', 'Dönem Ortalaması', 'Genel Ortalama', 'Şube Sıralaması(Genel)', 'Şube Sıralaması(Dönem)', 'Sınıf Sıralaması(Genel)', 'Sınıf Sıralaması(Dönem)', 'Dönem İhtarları','Toplam İhtarlar'];

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
        public async Task<IActionResult> GenerateAndDownloadPdf2(string myFileName, int userSessionId)
        {
            // PDF oluştur
            await GeneratePdfAsync2(myFileName,userSessionId);

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

        private async Task GeneratePdfAsync2(string myFileName, int userSessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("StudentDetails", "Otomation", new { userSessionId=userSessionId }, Request.Scheme);
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

    }
}
