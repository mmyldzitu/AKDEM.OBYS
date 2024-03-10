using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using AKDEM.OBYS.Dto.AppSessionBranchDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    
    public class BranchController : Controller
    {
        private readonly IAppBranchService _appBranchService;
        private readonly IAppStudentService _appStudentService;

        private readonly IValidator<AppBranchCreateModel> _branchCreateModelValidator;
        private readonly IMapper _mapper;
        private readonly IAppSessionService _appSessionService;
        private readonly IAppSessionBranchService _appSessionBranchService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppWarningService _appWarningService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IValidator<MergeBranchModel> _mergeBranchModelValidator;
        private readonly IAppGraduatedService _appGraduatedService;


        public BranchController(IAppBranchService appBranchService, IValidator<AppBranchCreateModel> branchCreateModelValidator, IMapper mapper, IAppStudentService appStudentService, IAppSessionService appSessionService, IAppSessionBranchService appSessionBranchService, IAppUserSessionService appUserSessionService, IAppWarningService appWarningService, IAppUserSessionLessonService appUserSessionLessonService, IWebHostEnvironment webHostEnvironment, IValidator<MergeBranchModel> mergeBranchModelValidator, IAppGraduatedService appGraduatedService)
        {
            _appBranchService = appBranchService;
            _branchCreateModelValidator = branchCreateModelValidator;
            _mapper = mapper;
            _appStudentService = appStudentService;
            _appSessionService = appSessionService;
            _appSessionBranchService = appSessionBranchService;
            _appUserSessionService = appUserSessionService;
            _appWarningService = appWarningService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _webHostEnvironment = webHostEnvironment;
            _mergeBranchModelValidator = mergeBranchModelValidator;
            _appGraduatedService = appGraduatedService;
        }
        char[] charactersToReplace = { '/', ' ', '\\', '?', ':', '.', ',' };
        char replacementChar = '_';

        public IActionResult Index()
        {

            var list = new List<AppClassListDto>();
            var items = Enum.GetValues(typeof(ClassType));
            foreach (int item in items)
            {
                list.Add(new AppClassListDto
                {
                    ClassId = item,
                    Definition = Enum.GetName(typeof(ClassType), item)
                });
            }
            ViewBag.classes = new SelectList(list, "ClassId", "Definition");

            return View(new AppBranchListDto());

        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateBranch(int id)
        {

            return View(new AppBranchCreateModel { ClassId = id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBranch(AppBranchCreateModel model)
        {
            model.Definition = $"{model.Class}/{model.Branch}";
            var result =await  _branchCreateModelValidator.ValidateAsync(model);
            var sessions = await _appSessionService.GetAllAsync();
            List<AppSessionBranchCreateDto> sessionBranchCreateDtos = new();
            if (result.IsValid)
            {
                ViewBag.classId = model.ClassId;
                var dto = _mapper.Map<AppBranchCreateDto>(model);
                var createResponse = await _appBranchService.CreateAsync(dto);
                var thisBranchId = await _appBranchService.GetBranchIdByBranchDefinitionAsync(createResponse.Data.Definition);

                foreach (var session in sessions.Data)
                {
                    if (session.Status2 == true)
                    {
                        sessionBranchCreateDtos.Add(new AppSessionBranchCreateDto { BranchId = thisBranchId, SessionId = session.Id });
                    }
                    
                }
                await _appSessionBranchService.CreateSessionBranchAsync(sessionBranchCreateDtos);
                
                return this.ResponseRedirectAction(createResponse, "GetBranches", parameter: model.ClassId);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }


            return View(model);
        }

        public async Task<IActionResult> ReturnRelatedBranches(int ClassId)
        {
            ViewBag.id = ClassId;
            var branches = await _appBranchService.GetClasses(ClassId);

            return View(branches);
        }
        
        public async Task<IActionResult> GetBranches(ClassType classType)
        {
            var model = new AppClassListWithCountModel();
            var list = new List<AppBranchListWithCountModel>();
            ViewBag.classType = (int)classType;
            string header = "";

            if (Enum.GetName(typeof(ClassType),classType) == "Tamamı")
            {
                var branches = await _appBranchService.GetList();
                header = "Tüm Sınıflar";

                foreach (var item in branches)
                {
                    if (item.ClassId != (int)ClassType.Mezun)
                    {
                        list.Add(new AppBranchListWithCountModel
                        {
                            Definition = item.Definition,
                            ClassId = item.ClassId,
                            Id = item.Id,
                            AppClass = item.AppClass,
                            //BURASI
                            BranchStokes = (await _appStudentService.GetStudentsWithBranchAsync(item.Id)).Count()


                        });
                    }
                   




                }
                int stock = 0;
                foreach( var item in list)
                {
                    stock = stock + item.BranchStokes;
                }
                ViewBag.totalStock = stock;
                ViewBag.header = header;
                

                string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                headerforPdf = headerforPdf.ToUpper();

                headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                ViewBag.headerforPdf = headerforPdf;
                var Class = await _appBranchService.GetClassById((int)classType);
                model.BranchListWithCountModels = list;
                model.AppClass = Class;
                return View(model);
            }
            else
            {
                if (Enum.GetName(typeof(ClassType), classType) == "Hazırlık")
                {
                    header = "Hazırlık Sınıfları";
                }
                
                else if ((Enum.GetName(typeof(ClassType), classType) == "Bir"))
                {
                    header = "1. Sınıflar";
                }
                else if ((Enum.GetName(typeof(ClassType), classType) == "iki"))
                {
                    header = "2. Sınıflar";
                }
                else if ((Enum.GetName(typeof(ClassType), classType) == "Üç"))
                {
                    header = "3. Sınıflar";
                }
                else if ((Enum.GetName(typeof(ClassType), classType) == "Dört"))
                {
                    header = "4. Sınıflar";
                }
                
                ViewBag.header = header;
                

                string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                headerforPdf = headerforPdf.ToUpper();

                headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                ViewBag.headerforPdf = headerforPdf;



                var branches = await _appBranchService.GetClasses( (int)classType);
                foreach (var item in branches)
                {
                    list.Add(new AppBranchListWithCountModel
                    {
                        Definition = item.Definition,
                        ClassId = item.ClassId,
                        Id = item.Id,
                        AppClass = item.AppClass,
                        //BURASI
                        BranchStokes = (await _appStudentService.GetStudentsWithBranchAsync(item.Id)).Count()


                    });

                }
                int classStokes = 0;
                foreach(var item in list)
                {
                    classStokes = classStokes + item.BranchStokes;
                }
                var Class = await _appBranchService.GetClassById((int)classType);
                model.AppClass = Class;

                model.BranchListWithCountModels = list;
                model.ClassStokes = classStokes;
                return View(model);
            }

        }
        public async Task<IActionResult> BranchDetails(int id)
        {
            await _appUserSessionService.TotalAverageAllUsersGeneral();
            List<AppBranchStudentListModel> studentList = new();
            //BURASI
            var students = await _appStudentService.GetStudentsWithBranchAsync(id);
            
            string branchName= await _appBranchService.BranchNameByByBranchId(id);
            ViewBag.branchName = branchName;
            ViewBag.branchId = id;
            

            
            
            string headerforPdf = ReplaceMultipleChars(branchName, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();
           
            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;


            foreach (var student in students)
            {


                studentList.Add(new AppBranchStudentListModel
                {
                    FirstName = student.FirstName,

                    SecondName = student.SecondName,
                    StudentId = student.Id,
                    Status = student.Status,
                    ImageUrl = student.ImagePath,
                    Email = student.Email,
                    Phone = student.PhoneNumber,
                    SıraNo = student.SıraNo,
                    Sınıf = student.AppBranch.Definition,
                    //BURASI
                    BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranchGeneral(student.Id, student.BranchId),

                    TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClassGeneral(student.Id, student.BranchId),

                    TotalAverage = await _appUserSessionService.ReturnTotalAverage(student.Id),

                    WarningCount = await _appWarningService.ReturnTwcGeneral(student.Id),

                }) ; ;
            }
            return View(studentList);
        }
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
        public async Task<IActionResult> Certificate(int userId)
        {
            
            var list = await _appGraduatedService.CertificaofUser(userId);
            string userName = list.studentName;
            string header = $"{userName}/Sertifika";

            

            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();

            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;

            return View(list);
        }
        public async Task<IActionResult> UserDetails(int userId)
        {
            ViewBag.userId = userId;
            ViewBag.userClassType = await _appBranchService.ReturnClassTypeOfUser(userId);
            var sessions = await _appSessionService.GetSessionsByUserId(userId);
            return View(sessions);
        }
        public async Task<IActionResult> SessionDetailsForUser(int userId, int sessionId)
        {
            await _appUserSessionService.TotalAverageAllUsers(sessionId);
            int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
            double slwc = await _appWarningService.SessionLessonWarningCountByUserSessionId(userSessionId);
            double awc = await _appWarningService.AbsenteismWarningCountByUserSessionId(userSessionId);
            double swc = await _appWarningService.SessionWarningCountByUserSessionId(userSessionId, slwc);
            double twc = await _appWarningService.TotalWarningCountByUserId(userId, sessionId);
            await _appWarningService.ChangeStudentStatusBecasuseOfWarning(userId, swc,awc, twc, userSessionId);
            //BURASI
            var branchId = await _appUserSessionService.GetBranchIdByUserSessionId(userSessionId);
            string sessionName= await _appSessionService.ReturnSessionName(sessionId);
            ViewBag.sessionName = sessionName;
            ViewBag.sessionName2 = sessionName.ToUpper().Replace("/", "_");
            ViewBag.sessionId = sessionId;
            ViewBag.branchId = branchId;
            ViewBag.userId = userId;
            int classId = await _appUserSessionService.GetClassIdByUserSessionId(userSessionId);

            var student = await _appStudentService.GetStudentById(userId);
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
                TotalAverage = await _appUserSessionService.ReturnTotalAverage(userId),
                AbsenteismWarningCount = await _appWarningService.ReturnAwc(userSessionId),
                LessonWarningCount= await _appWarningService.SessionLessonWarningCountByUserSessionId(userSessionId),
                TotalWarningCount = await _appWarningService.ReturnTwc(userId)
            };


            StudentDetailsModel model = new StudentDetailsModel { AppStudent = student, AppUserSessionLessons = userSessionlessons,
                //BURASI
                BranchSessionDegree = await _appUserSessionService.ReturnSessionOrderOfBranch(student.Id, student.BranchId, sessionId),
                //BURASI
                BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, student.BranchId, sessionId),
                TotalSessionDegree = await _appUserSessionService.ReturnSessionOrderOfClass(student.Id, student.BranchId, sessionId),
                TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, student.BranchId, sessionId),
                AppStudentSession = studentDetailSessionListModel };
            string header = $"{student.FirstName} {student.SecondName}_{sessionName}";

            string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
            headerforPdf = headerforPdf.ToUpper();

            headerforPdf = ConvertTurkishToEnglish(headerforPdf);
            ViewBag.headerforPdf = headerforPdf;


            return View(model);
        }
        public async Task<IActionResult> Transcrypt(int userId)
        {
            var user = await _appStudentService.GetStudentById(userId);
            ViewBag.userId = userId;
            List<StudentDetailsModel> models = new();
            var userSessionIds = await _appUserSessionService.GetUserSessionIdsByUserIdAsync(userId);
            foreach ( var userSessionId in userSessionIds)
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
                string firstName = student.FirstName;
                string secondName  = student.SecondName;
                string ımagePath = student.ImagePath;
                ViewBag.firstName = firstName;
                ViewBag.secondName = secondName;
                ViewBag.ImagePath = ımagePath;
                
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
                    LessonWarningCount= await _appWarningService.SessionLessonWarningCountByUserSessionId(userSessionId),
                    TotalAverage = await _appUserSessionService.ReturnTotalAverage(userId),
                    TotalWarningCount = await _appWarningService.ReturnTwc(userId)
                };


                StudentDetailsModel model = new StudentDetailsModel
                {
                    AppStudent = student,
                
                    SessionName = await _appSessionService.ReturnSessionName(sessionId),
                AppUserSessionLessons = userSessionlessons,
                //BURASI
                    BranchSessionDegree = await _appUserSessionService.ReturnSessionOrderOfBranch(student.Id, student.BranchId, sessionId),
                    //BURASI
                    BranchDegree = await _appUserSessionService.ReturnTotalOrderOfBranch(student.Id, student.BranchId, sessionId),
                    TotalSessionDegree = await _appUserSessionService.ReturnSessionOrderOfClass(student.Id, student.BranchId, sessionId),
                    TotalDegree = await _appUserSessionService.ReturnTotalOrderOfClass(student.Id, student.BranchId, sessionId),
                    AppStudentSession = studentDetailSessionListModel
                };
                models.Add(model);
                string header = $"{student.FirstName} {student.SecondName}_Transkript";
                string headerforPdf = ReplaceMultipleChars(header, charactersToReplace, replacementChar);
                headerforPdf = headerforPdf.ToUpper();

                headerforPdf = ConvertTurkishToEnglish(headerforPdf);
                ViewBag.headerforPdf = headerforPdf;

            }
            TranscryptListModel transcryptListModel= new TranscryptListModel{ AppStudent=user, StudentDetails=models};


            

            

            return View(transcryptListModel);

        }



        public async Task<IActionResult> GenerateAndDownloadPdf(string myFileName,int branchId)
        {
            // PDF oluştur
            await GeneratePdfAsync(myFileName, branchId);

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

        private async Task GeneratePdfAsync(string myFileName,int branchId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("BranchDetails", "Branch", new {id=branchId }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#','İsim', 'Sıra No', 'Şube', 'Email', 'Telefon', 'Görsel', 'Şube Derecesi', 'Sınıf Derecesi', 'Genel Ortalama', 'Toplam Baş. İhtar Sayısı'];

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

        public async Task<IActionResult> GenerateAndDownloadPdf2(string myFileName, ClassType classType)
        {
            // PDF oluştur
            await GeneratePdfAsync2(myFileName, classType);

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
        private async Task GeneratePdfAsync2(string myFileName, ClassType classType)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("GetBranches", "Branch", new {classType= classType }, Request.Scheme);
                    await page.GoToAsync(razorPageUrl);
                    await page.EvaluateFunctionAsync(@"() => {
                                // PDF Oluştur butonunu gizle
                                const pdfButton = document.getElementById('pdfButton');
                                if (pdfButton) {
                                    pdfButton.style.display = 'none';
                                }
                                         const hbutton = document.getElementById('branchButtons');
                                                                if (hbutton) {
                                                                    hbutton.style.display = 'none';
                                                                }
                                const newClassbutton = document.getElementById('newClass');
                                                                if (newClassbutton) {
                                                                    newClassbutton.style.display = 'none';
                                                                }
const mergeButton = document.getElementById('mergeBranches');
                                                                if (mergeButton) {
                                                                    mergeButton.style.display = 'none';
                                                                }

                                // Seçilecek kolon başlıkları
                                const selectedColumns = [ 'Şube', 'Sınıf', 'Mevcut'];

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
        public async Task<IActionResult> GenerateAndDownloadPdf3(string myFileName,int userId, int sessionId)
        {
            // PDF oluştur
            await GeneratePdfAsync3(myFileName,userId,sessionId);

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
        private async Task GeneratePdfAsync3(string myFileName,int userId,int sessionId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("SessionDetailsForUser", "Branch", new {userId=userId, sessionId=sessionId }, Request.Scheme);
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
        public async Task<IActionResult> GenerateAndDownloadPdf4(string myFileName, int userId)
        {
            // PDF oluştur
            await GeneratePdfAsync4(myFileName, userId);

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
        private async Task GeneratePdfAsync4(string myFileName, int userId)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("Transcrypt", "Branch", new { userId = userId }, Request.Scheme);
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

        public async Task< IActionResult> MergeBranches(int classType)
        {
            var branches1 = await _appBranchService.GetClasses(classType);
            var branches2 = await _appBranchService.GetClasses(classType);
            ViewBag.classType = classType;

            var list = new List<AppBranchListDto>();
            var items = branches1;
            foreach (var item in items)
            {
                list.Add(new AppBranchListDto
                {
                    Id = item.Id,
                    Definition = item.Definition
                });
            }
            ViewBag.firstBranch = new SelectList(list, "Id", "Definition");
            var list2 = new List<AppBranchListDto>();
            var items2 = branches2;
            foreach (var item in items2)
            {
                list2.Add(new AppBranchListDto
                {
                    Id = item.Id,
                    Definition = item.Definition
                });
            }
            ViewBag.secondBranch = new SelectList(list2, "Id", "Definition");
            return View();
        }
        public async Task<IActionResult> RemoveBranch(int branchId, int classType)
        {
            await _appBranchService.SessionEndingBranchStatus(branchId);
            return RedirectToAction("GetBranches", new { classType = classType });
        }
        [HttpPost]
        public async Task<IActionResult> MergeBranches(MergeBranchModel model)
        {
            var result = _mergeBranchModelValidator.Validate(model);
            if (result.IsValid)
            {
                var firstBranchStudents = await _appStudentService.GetStudentsWithBranchAsync(model.FirstBranchId);
                foreach (var student in firstBranchStudents)
                {
                    await _appStudentService.ChangeBranchForStudent(student.Id, model.SecondBranchId);
                    int activeStatusId = await _appSessionService.GetActiveSessionId();
                    await _appStudentService.ControlUserSessionWhenUpdating(student.Id, activeStatusId);
                    await _appStudentService.CreateStudentOrChangeStatusProcessForUserSessionandUSerSessionLessons(student.Id);
                    await _appBranchService.SessionEndingBranchStatus(model.FirstBranchId);
                }
                return RedirectToAction("GetBranches", new { ClassType = model.ClassType });
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var branches1 = await _appBranchService.GetClasses(model.ClassType);
            var branches2 = await _appBranchService.GetClasses(model.ClassType);
            

            var list = new List<AppBranchListDto>();
            var items = branches1;
            foreach (var item in items)
            {
                list.Add(new AppBranchListDto
                {
                    Id = item.Id,
                    Definition = item.Definition
                });
            }
            ViewBag.classType = model.ClassType;

            ViewBag.firstBranch = new SelectList(list, "Id", "Definition");
            var list2 = new List<AppBranchListDto>();
            var items2 = branches2;
            foreach (var item in items2)
            {
                list2.Add(new AppBranchListDto
                {
                    Id = item.Id,
                    Definition = item.Definition
                });
            }
            ViewBag.secondBranch = new SelectList(list2, "Id", "Definition");
            return View(model);
            
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
                    var razorPageUrl = Url.Action("Certificate", "Branch", new { userId = userId }, Request.Scheme);
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
    }

}



