using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppSessionBranchDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.SessionType;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
    public class SessionController : Controller
    {
        private readonly IAppSessionService _appSessionService;
        private readonly IAppStudentService _appStudentService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppBranchService _appBranchService;
        private readonly IAppSessionBranchService _appSessionBranchService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IAppScheduleService _appScheduleService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppWarningService _appWarningService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IValidator<AppSessionCreateModel> _appSessionCreateModelValidator;
        private readonly IValidator<AppSessionUpdateDto> _appSessionUpdateDtoValidator;
        private readonly IAppUserService _appUserService;
        private readonly IAppGraduatedService _appGraduatedService;


        public SessionController(IAppSessionService appSessionService, IAppStudentService appStudentService, IAppUserSessionService appUserSessionService, IAppBranchService appBranchService, IAppSessionBranchService appSessionBranchService, IAppScheduleDetailService appScheduleDetailService, IAppScheduleService appScheduleService, IAppUserSessionLessonService appUserSessionLessonService, IAppWarningService appWarningService, IWebHostEnvironment webHostEnvironment, IValidator<AppSessionCreateModel> appSessionCreateModelValidator, IValidator<AppSessionUpdateDto> appSessionUpdateDtoValidator, IAppUserService appUserService, IAppGraduatedService appGraduatedService)
        {
            _appSessionService = appSessionService;
            _appStudentService = appStudentService;
            _appUserSessionService = appUserSessionService;
            _appBranchService = appBranchService;
            _appSessionBranchService = appSessionBranchService;
            _appScheduleDetailService = appScheduleDetailService;
            _appScheduleService = appScheduleService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appWarningService = appWarningService;
            _webHostEnvironment = webHostEnvironment;
            _appSessionCreateModelValidator = appSessionCreateModelValidator;
            _appSessionUpdateDtoValidator = appSessionUpdateDtoValidator;
            _appUserService = appUserService;
            _appGraduatedService = appGraduatedService;
        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            
                var response = await _appSessionService.GetOrderingAsync();
            ViewBag.status2Count = await _appSessionService.Status2Count();

            if (response.ResponseType == Common.ResponseType.Success)
            {
                return View(response.Data);

            }
            return View(new List<AppSessionListDto>());








        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> ChangeStatus(int id)
        {
            await _appSessionService.SetStatusAsync(id);
            return RedirectToAction("Index", "Session");
        }
        [Authorize(Roles = "Admin")]

        public async Task< IActionResult> CreateSession()
        {
            var items = Enum.GetValues(typeof(SessionType));
            var list = new List<SessionTypeListDto>();
            foreach (int item in items)
            {
                list.Add(new SessionTypeListDto
                {
                    Id = item,
                    Definition = Enum.GetName(typeof(SessionType), item)
                });

            }
            ViewBag.sessions = new SelectList(list, "Id", "Definition");

            var items2 = await _appUserService.GetTeacherNameForPresident();
            var list2 = new List<TeacherListForPresident>();
            foreach (string item in items2)
            {
                list2.Add(new TeacherListForPresident
                {

                    Definition = item,
               Val = item
                });

            }
            ViewBag.teachers = new SelectList(list2, "Definition", "Val");
            return View(new AppSessionCreateModel());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateSession(AppSessionCreateModel model)
        {
            var items = Enum.GetValues(typeof(SessionType));
            var list = new List<SessionTypeListDto>();
            foreach (int item in items)
            {
                list.Add(new SessionTypeListDto
                {
                    Id = item,
                    Definition = Enum.GetName(typeof(SessionType), item)
                });

            }
            ViewBag.sessions = new SelectList(list, "Id", "Definition");

            var items2 = await _appUserService.GetTeacherNameForPresident(); 
            var list2 = new List<TeacherListForPresident>();
            foreach (string item in items2)
            {
                list2.Add(new TeacherListForPresident
                {

                    Definition=item,
                    Val=item
                }) ;

            }
            ViewBag.teachers = new SelectList(list2, "Definition", "Val");


            var result = await _appSessionCreateModelValidator.ValidateAsync(model);
            if (result.IsValid)
            {

                List<AppUserSessionCreateDto> appUserSessionCreateDtos = new();
                List<AppSessionBranchCreateDto> appSessionBranchCreateDtos = new();
                model.Definition = $"{model.year1}-{model.year2}/{ Enum.GetName(typeof(SessionType), model.SessionType)}";
                var students = await _appStudentService.GetAllStudentAsync(RoleType.Student);
                var branches = await _appBranchService.GetList();
                AppSessionCreateDto dto = new AppSessionCreateDto();
                dto.Definition = model.Definition;
                dto.Status = model.Status;
                dto.SessionPresident = model.SessionPresident;
                dto.MinAbsenteeism = model.MinAbsenteeism;
                dto.MinAverageNote = model.MinAverageNote;
                dto.MinLessonNote = model.MinLessonNote;
                dto.Status2 = model.Status2;
                var response = await _appSessionService.CreateAsync(dto);
                await _appSessionService.ChangeStatus2(dto.Definition);
                var thisSessionId = await _appSessionService.GetSeesionIdBySessionDefinition(response.Data.Definition);
                //BURASI
                foreach (var student in students)
                {
                    if (student.Status == true && student.ClassId != (int)ClassType.Mezun)
                    {
                        appUserSessionCreateDtos.Add(new AppUserSessionCreateDto { BranchId = student.BranchId, ClassId = student.ClassId, UserId = student.Id, SessionId = thisSessionId });

                    }
                }
                foreach (var branch in branches)
                {
                    if (!branch.Definition.Contains("Mezun"))
                    {
                        appSessionBranchCreateDtos.Add(new AppSessionBranchCreateDto { SessionId = thisSessionId, BranchId = branch.Id });
                    }

                }
                //BURASI
                await _appUserSessionService.CreateUserSessionAsync(appUserSessionCreateDtos);
                await _appSessionBranchService.CreateSessionBranchAsync(appSessionBranchCreateDtos);
                return this.ResponseRedirectAction(response, "Index");


            }
            foreach( var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);
            
            
        }

        public async Task<IActionResult> SessionDetails(int id)
        {
            ViewBag.id = id;
            string sessionName = await _appSessionService.ReturnSessionName(id);
            var sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(id);
            if (sessionName.Contains("Bahar") && sessionStatus2==false)
            {
                ViewBag.sessionValue = 1;
            }
            else
            {
                ViewBag.sessionValue = 0;
            }
            ViewBag.sessionName = sessionName;
            ViewBag.sessionStatus = await _appSessionService.GetStatusFromSessionId(id);
            ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(id);
            
            return View();
        }
        public async Task<IActionResult> RemoveSession(int sessionId)
        {
            await _appScheduleDetailService.RemoveScheduleDetailBySessionId(sessionId);
            await _appScheduleService.RemoveScheduleBySessionId(sessionId);
            await _appSessionBranchService.RemoveSessionBranchesBySessionId(sessionId);

            await _appUserSessionLessonService.RemoveUserSessionLessonBySessionId(sessionId);
            await _appWarningService.RemoveWarningBySessionId(sessionId);
            await _appUserSessionService.RemoveUserSessionBySessionId(sessionId);

            await _appUserSessionService.TotalAverageAllUsers(sessionId);

            await _appSessionService.RemoveAsync(sessionId);

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> ActiveSession(int sessionId)
        {
            await _appSessionService.ChangeStatus2BySessionId(sessionId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SessionEnding(int sessionId)
        {
            
            await _appBranchService.RemoveEmptyBranches();
            var branches = await _appBranchService.GetBranchListWithSessionId(sessionId);
                var sessionDefinition = await _appSessionService.ReturnSessionName(sessionId);
            if (sessionDefinition.Contains("Yaz"))
            {
                await _appSessionService.SetStatusAllexceptThis(sessionId);
            }
            else
            {
                await _appSessionService.SetStatusAllexceptThis( sessionId, sessionId:sessionId);

            }
            foreach (var branch in branches)
            {

                if (sessionDefinition.Contains("Yaz"))
                {
                    await _appBranchService.SessionEndingBranchStatus(branch.Id);

                    if (!branch.Definition.Contains("Mezun"))
                    {

                        int classId = await _appBranchService.GetClassIdByBranchId(branch.Id);
                        string branchDefinition = await _appBranchService.BranchNameByByBranchId(branch.Id);
                        string[] parcalar = branchDefinition.Split(new char[] { '/' });
                        string definition = "";
                        string branchType = parcalar[1];
                        string newClass = await _appBranchService.FindClassNameByClassId(classId + 1);
                        if (!branch.Definition.Contains("4"))
                        {
                            definition = $"{newClass}/{branchType}";

                        }
                        


                        AppBranchCreateDto appBranchCreateDto = new AppBranchCreateDto
                        {
                            ClassId = classId + 1,
                            Definition = definition,
                        };
                        await _appBranchService.CreateBranchWhichNotExist(appBranchCreateDto);
                        string newBranchDefinition = "";
                        if (!branch.Definition.Contains("4"))
                        {
                            newBranchDefinition = $"{newClass}/{branchType}";


                        }
                        else
                        {

                            newBranchDefinition = definition;
                        }

                        var newBranch = await _appBranchService.FindBranchByNameAndStatus(newBranchDefinition);
                        var students = await _appStudentService.GetStudentsWithBranchAsync(branch.Id);



                        foreach (var student in students)
                        {
                            if (student.Status != false)
                            {
                                await _appStudentService.ChangeStudentBranch(student.Id, newBranch.Id, newBranch.ClassId);

                            }
                        }
                    }




                }
                else if (sessionDefinition.Contains("Bahar"))
                 {
                   

                    if(branch.Definition.Contains("4"))
                    {
                        await _appBranchService.SessionEndingBranchStatus(branch.Id);

                        int classId = await _appBranchService.GetClassIdByBranchId(branch.Id);
                        string branchDefinition = await _appBranchService.BranchNameByByBranchId(branch.Id);
                        string[] parcalar = branchDefinition.Split(new char[] { '/' });
                        string definition = "";
                        string branchType = parcalar[1];
                        string newClass = await _appBranchService.FindClassNameByClassId(classId + 1);
                        await _appBranchService.SessionEndingBranchStatus(branch.Id);

                        definition = newClass + "/" + sessionDefinition;
                        AppBranchCreateDto appBranchCreateDto = new AppBranchCreateDto
                        {
                            ClassId = classId + 1,
                            Definition = definition,
                        };
                        await _appBranchService.CreateBranchWhichNotExist(appBranchCreateDto);
                        string newBranchDefinition = "";



                        newBranchDefinition = definition;


                        var newBranch = await _appBranchService.FindBranchByNameAndStatus(newBranchDefinition);
                        var students = await _appStudentService.GetStudentsWithBranchAsync(branch.Id);
                        for (int i = 0; i < students.Count; i++)
                        {
                            if (students[i].Status != false)
                            {
                                await _appStudentService.ChangeStudentBranch(students[i].Id, newBranch.Id, newBranch.ClassId);
                                await _appGraduatedService.GraduateStudents(students[i].Id, sessionId, i + 1);

                            }
                        }

                    }

                    
                }

            }
            return RedirectToAction("SessionDetails", new { id = sessionId });
        }
       
        public async Task<IActionResult> MissingNotes(int sessionId)
        {
            var missingNotes = await _appUserSessionLessonService.UserSessionLessonDetailsBySessionId(sessionId);
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.Replace("/", "_");
            ViewBag.sessionId = sessionId;
            return View(missingNotes);
        }
        public async Task<IActionResult> SessionCriteria(int sessionId)
        {
            var sessionCriterias = await _appSessionService.SessionCriterias(sessionId);
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.Replace("/", "_");
            ViewBag.sessionId = sessionId;
            return View(sessionCriterias);
        }
        public async Task<IActionResult> UpdateSessionCriteria(int sessionId)
        {
            var sessionCriterias = await _appSessionService.UpdateSessionCriterias(sessionId);
            ViewBag.sessionName= await _appSessionService.ReturnSessionName(sessionId);
            var items2 = await _appUserService.GetTeacherNameForPresident();
            var list2 = new List<TeacherListForPresident>();
            foreach (string item in items2)
            {
                list2.Add(new TeacherListForPresident
                {

                    Definition = item,
                    Val=item
                });

            }
            ViewBag.teachers = new SelectList(list2, "Definition", "Val");


            return View(sessionCriterias);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSessionCriteria(AppSessionUpdateDto dto)
        {
            var result = _appSessionUpdateDtoValidator.Validate(dto);
            var items2 = await _appUserService.GetTeacherNameForPresident();
            var list2 = new List<TeacherListForPresident>();
            foreach (string item in items2)
            {
                list2.Add(new TeacherListForPresident
                {

                    Definition = item,
                    Val=item
                });

            }
            ViewBag.teachers = new SelectList(list2, "Definition", "Val");
            if (result.IsValid)
            {
                await _appSessionService.UpdateSessionCriteriasPost(dto);
                return RedirectToAction("SessionCriteria", "Session", new { sessionId = dto.Id });
            }
            foreach( var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(dto);
            
        }
        public async Task<IActionResult> GraduatedStudents(int sessionId)
        {
            ViewBag.sessionId = sessionId;
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.Replace("/", "_");
            List<GraduatedStudentsModel> models = new List<GraduatedStudentsModel>();
            
            var students = await _appStudentService.GraduatedStudenstBySessionId(sessionId);
            foreach( var student in students)
            {
                GraduatedStudentsModel model = new GraduatedStudentsModel {
                    Name = $"{student.FirstName} {student.SecondName}",
                    Branch = await _appUserSessionService.LastBranchNameByUserAndSessionId(sessionId, student.Id),
                    UserId=student.Id,
                SıraNo = student.SıraNo,
                Average = student.TotalAverage,
                GradDate = await _appSessionService.ReturnSessionName(sessionId),
                BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, student.BranchId, sessionId),
                ClassDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, student.BranchId, sessionId),
            };
                models.Add(model);
            }
            return View(models);
        }
        public async Task<IActionResult> Certificate(int userId,int sessionId)
        {
            ViewBag.sessionId = sessionId;
            var list = await _appGraduatedService.CertificaofUser(userId);
            return View(list);
        }

        public async Task<IActionResult> GenerateAndDownloadPdf(string myFileName, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsync(myFileName, sessionId);

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

        private async Task GeneratePdfAsync(string myFileName, int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("GraduatedStudents", "Session", new { sessionId = sessionId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['İsim', 'Sıra No','Mezun Olduğu Sınıf','Mezuniyet Dönemi','Mezuniyet Ortalaması', 'Mezuniyet Derecesi(Genel)', 'Mezuniyet Derecesi(Şube)'];

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
                    var razorPageUrl = Url.Action("MissingNotes", "Session", new { sessionId = sessionId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'DERS','AÇIKLAMA'];

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

        public async Task<IActionResult> GenerateAndDownloadPdfCertificate(string myFileName, int userId, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsyncCertificate(myFileName,userId, sessionId);

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

        private async Task GeneratePdfAsyncCertificate(string myFileName,int userId, int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("Certificate", "Session", new {userId=userId, sessionId = sessionId }, Request.Scheme);
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
        public async Task<IActionResult> GenerateAndDownloadPdfStudentSessionCriteria(string myFileName, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsyncStudentSessionCriteria(myFileName, sessionId);

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

        private async Task GeneratePdfAsyncStudentSessionCriteria(string myFileName, int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("SessionCriteria", "Session", new { sessionId = sessionId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                         // PDF Oluştur butonunu gizle
                         const pdfButton = document.getElementById('pdfButton');
                     if (pdfButton) {
                                  pdfButton.style.display = 'none';
                            }
 const editButton = document.getElementById('editbutton');
                     if (editButton) {
                                  editButton.style.display = 'none';
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
