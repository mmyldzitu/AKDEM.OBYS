using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class UserController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IAppBranchService _appBranchService;
        private readonly IAppStudentService _appStudentService;

        private readonly IMapper _mapper;
        private readonly IValidator<AppTeacherUpdateModel> _teacherUpdateModelValidator;
        private readonly IAppSessionService _appSessionService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IAppWarningService _appWarningService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppGraduatedService _appGraduatedService;


        public UserController(IAppUserService appUserService, IMapper mapper, IValidator<AppTeacherUpdateModel> teacherUpdateModelValidator, IAppBranchService appBranchService, IAppStudentService appStudentService, IAppSessionService appSessionService, IAppUserSessionService appUserSessionService, IAppUserSessionLessonService appUserSessionLessonService, IAppScheduleDetailService appScheduleDetailService, IAppWarningService appWarningService, IWebHostEnvironment webHostEnvironment, IAppGraduatedService appGraduatedService)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _teacherUpdateModelValidator = teacherUpdateModelValidator;
            _appBranchService = appBranchService;
            _appStudentService = appStudentService;
            _appSessionService = appSessionService;
            _appUserSessionService = appUserSessionService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appScheduleDetailService = appScheduleDetailService;
            _appWarningService = appWarningService;
            _webHostEnvironment = webHostEnvironment;
            _appGraduatedService = appGraduatedService;
        }

        public async Task<IActionResult> GetTeachers(bool status=true)
        {
            var response = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher,status);
            if (status)
            {
                ViewBag.header = "Aktif";
            }
            else
            {
                ViewBag.header = "Pasif";
            }
            ViewBag.status = status;
            return View(response.Data);
        }
        public IActionResult CreateTeacher()
        {
            return View(new AppTeacherCreateModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeacher(AppTeacherCreateModel model)
        {
            AppTeacherCreateDto newdto = new();

            if (model.ImagePath != null)
            {
                var fileName = Guid.NewGuid().ToString();
                var extName = Path.GetExtension(model.ImagePath.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "teacherImages", fileName + extName);
                string pathforDb = Path.Combine("\\", "images", "teacherImages", fileName + extName);
                var stream = new FileStream(path, FileMode.Create);
                await model.ImagePath.CopyToAsync(stream);
                newdto.ImagePath = pathforDb;


            }
            newdto.FirstName = model.FirstName;
            newdto.SecondName = model.SecondName;
            newdto.Status = model.Status;
            newdto.PhoneNumber = model.PhoneNumber;
            newdto.Email = model.Email;
            newdto.DepartReason = model.DepartReason;
            newdto.Password = model.Email;
            

            var response = await _appUserService.CreateTeacherWithRoleAsync(newdto, (int)RoleType.Teacher);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach (var error in response.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                }
                return View(model);
            }

            return RedirectToAction("GetTeachers");
        }

        public async Task<IActionResult> UpdateTeacher(int id)
        {
            AppTeacherUpdateModel model = new();
            var response = await _appUserService.GetByIdAsync<AppTeacherUpdateDto>(id);
            if (response.Data.ImagePath != null)

            {
                model.ImagePath2 = response.Data.ImagePath;
            }
            model.Id = response.Data.Id;
            model.FirstName = response.Data.FirstName;
            model.SecondName = response.Data.SecondName;
            model.PhoneNumber = response.Data.PhoneNumber;
            model.Status = response.Data.Status;
            model.Email = response.Data.Email;
            model.Password = response.Data.Password;
            model.DepartReason = response.Data.DepartReason;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTeacher(AppTeacherUpdateModel model)
        {
            AppTeacherUpdateDto newdto = new();
            if (model.ImagePath != null)
            {

                var fileName = Guid.NewGuid().ToString();
                var extName = Path.GetExtension(model.ImagePath.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "teacherImages", fileName + extName);
                string pathforDb = Path.Combine("\\", "images", "teacherImages", fileName + extName);
                var stream = new FileStream(path, FileMode.Create);
                await model.ImagePath.CopyToAsync(stream);
                newdto.ImagePath = pathforDb;

            }
            else
            {
                newdto.ImagePath = model.ImagePath2;
            }

            newdto.Id = model.Id;
            newdto.Password = model.Password;
            newdto.FirstName = model.FirstName;
            newdto.SecondName = model.SecondName;
            newdto.Status = model.Status;
            newdto.PhoneNumber = model.PhoneNumber;
            newdto.Email = model.Email;
            newdto.DepartReason = model.DepartReason;

            var response = await _appUserService.UpdateAsync(newdto);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach (var error in response.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            return RedirectToAction("GetTeachers");

        }
        public async Task<IActionResult> RemoveTeacher(int id,bool status)
        {
            await _appUserService.ChangeTeacherStatus(id);
            return RedirectToAction("GetTeachers", new { status = status });
            
        }


        public async Task<IActionResult> GetStudents(ClassType classType)
        {
            var response = await _appUserService.GetAllStudentAsync(RoleType.Student, classType);
            ViewBag.classType = classType;
            ViewBag.classId = (int)classType;
            string header = "";
            if (Enum.GetName(typeof(ClassType), classType) == "Tamamı")
            {
                header = "Tüm Öğrenciler";
            }
            else if((Enum.GetName(typeof(ClassType), classType) == "Mezun"))
            {
                header = "Mezun Öğrenciler";
            }
            else if ((Enum.GetName(typeof(ClassType), classType) == "Bir"))
            {
                header = "1. Sınıf Öğrencileri";
            }
            else if ((Enum.GetName(typeof(ClassType), classType) == "iki"))
            {
                header = "2. Sınıf Öğrencileri";
            }
            else if ((Enum.GetName(typeof(ClassType), classType) == "Üç"))
            {
                header = "3. Sınıf Öğrencileri";
            }
            else if ((Enum.GetName(typeof(ClassType), classType) == "Dört"))
            {
                header = "4. Sınıf Öğrencileri";
            }
            else if ((Enum.GetName(typeof(ClassType), classType) == "Pasif"))
            {
                header = "Programdan Ayrılan Öğrenciler";
            }
            else
            {
                header = "Hazırlık Öğrencileri";

            }
            ViewBag.header = header;
            ViewBag.header2 = header.Replace(" ", "_").Replace(".", "");
            return View(response);
        }
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            var response = await _appStudentService.GetStudentsByClassId(RoleType.Student,classId);

            return View(response);
        }

        public async Task<IActionResult> CreateStudent(int classId=1)
        {
            ViewBag.classId = classId;
            var list = new List<AppClassListDto>();
            var items = Enum.GetValues(typeof(ClassType));
            foreach (int item in items)
            {
                if (Enum.GetName(typeof(ClassType), item) != "Mezun" && Enum.GetName(typeof(ClassType), item) != "Tamamı" && Enum.GetName(typeof(ClassType), item) != "Pasif")
                {
                    list.Add(new AppClassListDto
                    {
                        ClassId = item,
                        Definition = Enum.GetName(typeof(ClassType), item)
                    });
                }
                    
            }
            ViewBag.classes = new SelectList(list, "ClassId", "Definition");


            var list2 = new List<AppBranchListDto>();
            var items2 = await _appBranchService.GetList();
            foreach (var item in items2)
            {
                list2.Add(new AppBranchListDto
                {
                    Id = item.Id,

                    Definition = item.Definition
                }); ;
            }
            ViewBag.branches = new SelectList(list2, "Id", "Definition");


            return View(new AppStudentCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudent(AppStudentCreateModel model, int classId=1)
        {
            AppStudentCreateDto dto = new();

            if (model.ImagePath != null)
            {
                var fileName = Guid.NewGuid().ToString();
                var extName = Path.GetExtension(model.ImagePath.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "studentImages", fileName + extName);
                string pathforDb = Path.Combine("\\", "images", "studentImages", fileName + extName);
                var stream = new FileStream(path, FileMode.Create);
                await model.ImagePath.CopyToAsync(stream);
                dto.ImagePath = pathforDb;
            }
            dto.SıraNo = model.SıraNo;
            dto.FirstName = model.FirstName;
            dto.SecondName = model.SecondName;
            dto.PhoneNumber = model.PhoneNumber;
            dto.Email = model.Email;
            dto.Password = model.Email;
            dto.BranchId = model.BranchId;
            dto.ClassId = model.ClassId;
            dto.Status = model.Status;
            dto.DepartReason = "";
            var response = await _appStudentService.CreateStudentWithRoleAsync(dto, (int)RoleType.Student);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach (var error in response.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                }
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


                var list2 = new List<AppBranchListDto>();
                var items2 = await _appBranchService.GetList();
                foreach (var item in items2)
                {
                    list2.Add(new AppBranchListDto
                    {
                        Id = item.Id,

                        Definition = item.Definition
                    }); ;
                }
                ViewBag.branches = new SelectList(list2, "Id", "Definition");
                return View(model);
            }

            int thisUserId = await _appUserService.GetUserIdByNameSecondNameandEmail(dto.FirstName, dto.SecondName, dto.Email);
            //BURASI
            await _appStudentService.CreateStudentOrChangeStatusProcessForUserSessionandUSerSessionLessons(thisUserId);


            return RedirectToAction("GetStudents" ,new { classType=classId });


        }
        [HttpGet]
        public async Task<IActionResult> GetBranches(int ClassId)
        {


            var branches = await _appBranchService.GetClasses(ClassId); // Şubeleri getiren bir metot veya servis çağırılmalı
            var branchList = branches.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),   // Şubenin Id'si
                Text = s.Definition           // Şubenin Adı
            }).ToList();
            return Json(branchList);
        }
        public async Task<IActionResult> UpdateStudent(int id)
        {
            AppStudentUpdateModel model = new();
            var response = await _appStudentService.GetByIdAsync<AppStudentUpdateDto>(id);
            if (response.Data.ImagePath != null)

            {
                model.ImagePath2 = response.Data.ImagePath;
            }
            model.Id = response.Data.Id;
            model.FirstName = response.Data.FirstName;
            model.SecondName = response.Data.SecondName;
            model.PhoneNumber = response.Data.PhoneNumber;
            model.Status = response.Data.Status;
            model.Email = response.Data.Email;
            model.Password = response.Data.Password;
            model.ClassId = response.Data.ClassId;
            model.BranchId = response.Data.BranchId;
            model.ExBranchId = model.BranchId;
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


            var list2 = new List<AppBranchListDto>();
            var items2 = await _appBranchService.GetListWithClassId(model.ClassId);
            foreach (var item in items2)
            {
                list2.Add(new AppBranchListDto
                {
                    Id = item.Id,

                    Definition = item.Definition
                }); ;
            }
            ViewBag.branches = new SelectList(list2, "Id", "Definition");

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStudent(AppStudentUpdateModel model)
        {
            //BURASI
            AppStudentUpdateDto newdto = new();
            if (model.ImagePath != null)
            {

                var fileName = Guid.NewGuid().ToString();
                var extName = Path.GetExtension(model.ImagePath.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "studentImages", fileName + extName);
                string pathforDb = Path.Combine("\\", "images", "studentImages", fileName + extName);
                var stream = new FileStream(path, FileMode.Create);
                await model.ImagePath.CopyToAsync(stream);
                newdto.ImagePath = pathforDb;

            }
            else
            {
                newdto.ImagePath = model.ImagePath2;
            }

            newdto.Id = model.Id;
            newdto.Password = model.Password;
            newdto.FirstName = model.FirstName;
            newdto.SecondName = model.SecondName;
            newdto.Status = model.Status;
            newdto.PhoneNumber = model.PhoneNumber;
            newdto.Email = model.Email;
            newdto.BranchId = model.BranchId;
            newdto.ClassId = model.ClassId;



            var response = await _appStudentService.UpdateAsync(newdto);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach (var error in response.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
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


                var list2 = new List<AppBranchListDto>();
                var items2 = await _appBranchService.GetList();
                foreach (var item in items2)
                {
                    list2.Add(new AppBranchListDto
                    {
                        Id = item.Id,

                        Definition = item.Definition
                    }); ;
                }
                ViewBag.branches = new SelectList(list2, "Id", "Definition");
                return View(model);


            }
            if (model.ExBranchId != model.BranchId)
            {
                int activeStatusId = await _appSessionService.GetActiveSessionId();
                await _appStudentService.ControlUserSessionWhenUpdating(model.Id, activeStatusId);
                await _appStudentService.CreateStudentOrChangeStatusProcessForUserSessionandUSerSessionLessons(model.Id);


            }
            return RedirectToAction("GetStudents", new { classType = model.ClassId });

        }
        public async Task<IActionResult> RemoveStudent(int id, int classId=1)
        {


            var userSessionIdList = await _appUserSessionService.GetUserSessionIdsByUserIdAsync(id);
            await _appUserSessionLessonService.RemoveUserSessionLessonsByUserSessionListAsync(userSessionIdList);
            await _appUserSessionService.RemoveUserSessionByUserId(id);
            await _appWarningService.RemoveWarningByUserId(id);
            await _appGraduatedService.RemoveCertificateByUserId(id);
            var response = await _appStudentService.RemoveAsync(id);

            return RedirectToAction("GetStudents", new { classType = classId });
            
        }

        public async Task<IActionResult> GenerateAndDownloadPdf(string myFileName, ClassType classType)
        {
            // PDF oluştur
            await GeneratePdfAsync(myFileName, classType);

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

        private async Task GeneratePdfAsync(string myFileName, ClassType classType)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("GetStudents", "User", new {classType=classType } , Request.Scheme);
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
                                const newStudentbutton = document.getElementById('newStudent');
                                                                if (newStudentbutton) {
                                                                    newStudentbutton.style.display = 'none';
                                                                }

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'İsim','Sıra No','Durum','Email', 'Telefon Numarası', 'Görsel', 'Sınıf', 'Şube','Mezuniyet Dönemi','Ayrılık Dönemi'];

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

        public async Task<IActionResult> GenerateAndDownloadPdfTeacher(string myFileName, bool status)
        {
            // PDF oluştur
            await GeneratePdfAsyncTeacher(myFileName, status);

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

        private async Task GeneratePdfAsyncTeacher(string myFileName, bool status)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("GetTeachers", "User", new { status = status }, Request.Scheme);
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
                                const newStudentbutton = document.getElementById('newStudent');
                                                                if (newStudentbutton) {
                                                                    newStudentbutton.style.display = 'none';
                                                                }

                                // Seçilecek kolon başlıkları
                                const selectedColumns = ['#', 'Ünvan', ' İsim','Soyisim','Email', 'Telefon Numarası', 'Görsel'];

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
