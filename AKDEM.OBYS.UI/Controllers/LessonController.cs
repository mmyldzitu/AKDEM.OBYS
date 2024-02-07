using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LessonController : Controller
    {
        private readonly IAppLessonService _appLessonService;
        private readonly IAppUserService _appUserService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IValidator<AppLessonCreateDto> _appLessonCreateDtoValidator;
        private readonly IValidator<AppLessonUpdateDto> _appLessonUpdateDtoValidator;


        public LessonController(IAppLessonService appLessonService, IAppUserService appUserService, IConverter converter, IWebHostEnvironment webHostEnvironment, IValidator<AppLessonCreateDto> appLessonCreateDtoValidator, IValidator<AppLessonUpdateDto> appLessonUpdateDtoValidator)
        {
            _appLessonService = appLessonService;
            _appUserService = appUserService;
            _webHostEnvironment = webHostEnvironment;
            _appLessonCreateDtoValidator = appLessonCreateDtoValidator;
            _appLessonUpdateDtoValidator = appLessonUpdateDtoValidator;
        }

        public async Task<IActionResult> Index(bool status = true)
        {
            var response = await _appLessonService.GetLessonsByTeacher(status);
            ViewBag.status = status;
            if (status)
            {
                ViewBag.header = "Aktif";
            }
            else
            {
                ViewBag.header = "Pasif";
            }
            return this.View(response);



        }

        public async Task<IActionResult> GenerateAndDownloadPdf(string myFileName, bool status)
        {
            // PDF oluştur
            await GeneratePdfAsync(myFileName, status);

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

        private async Task GeneratePdfAsync(string myFileName, bool status)
        {
            await new BrowserFetcher().DownloadAsync();
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    // Razor sayfasının URL'sini belirtin
                    var razorPageUrl = Url.Action("Index", "Lesson", new { status = status }, Request.Scheme);
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

                    // Yeni Ders Ekle butonunu gizle
                    const newLessonButton = document.getElementById('newLessonButton');
                    if (newLessonButton) {
                        newLessonButton.style.display = 'none';
                    }

                    // Diğer elementleri gizle (isteğe bağlı)
                    document.querySelectorAll('th:not(:nth-child(1)), td:not(:nth-child(1)), th:not(:nth-child(2)), td:not(:nth-child(2))').forEach(element => {
                        if (element.cellIndex !== 0 && element.cellIndex !== 1) {
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



        public async Task<IActionResult> CreateLesson()
        {
            var list = new List<AppLessonListModel>();

            var items = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher, true);

            foreach (var item in items.Data)
            {
                list.Add(new AppLessonListModel
                {
                    Id = item.Id,
                    TeacherName = $"{ item.FirstName} { item.SecondName}"



                });
            }
            ViewBag.teachers = new SelectList(list, "Id", "TeacherName");

            return View(new AppLessonCreateDto());

        }
        [HttpPost]
        public async Task<IActionResult> CreateLesson(AppLessonCreateDto dto)
        {
            var result = await _appLessonCreateDtoValidator.ValidateAsync(dto);
            if (result.IsValid)
            {
                await _appLessonService.AppLessonCreate(dto);
                return RedirectToAction("Index");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(dto);
            }




        }
        public async Task<IActionResult> UpdateLesson(int id)
        {

            var response = await _appLessonService.GetByIdAsync<AppLessonUpdateDto>(id);
            var list = new List<AppLessonListModel>();

            var items = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher, true);

            foreach (var item in items.Data)
            {
                list.Add(new AppLessonListModel
                {
                    Id = item.Id,
                    TeacherName = $"{ item.FirstName} { item.SecondName}"



                });
            }
            ViewBag.teachers = new SelectList(list, "Id", "TeacherName");
            return this.View(response.Data);
        }
        public async Task<IActionResult> RemoveLesson(int id, bool status)
        {
            await _appLessonService.ChangeLessonStatus(id);
            return RedirectToAction("Index", new { status = status });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLesson(AppLessonUpdateDto dto)
        {
            var list = new List<AppLessonListModel>();

            var items = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher, true);

            foreach (var item in items.Data)
            {
                list.Add(new AppLessonListModel
                {
                    Id = item.Id,
                    TeacherName = $"{ item.FirstName} { item.SecondName}"



                });
            }
            ViewBag.teachers = new SelectList(list, "Id", "TeacherName");
            var result = await _appLessonUpdateDtoValidator.ValidateAsync(dto);
            if (result.IsValid)
            {
                await _appLessonService.AppLessonUpdate(dto);
                return RedirectToAction("Index");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(dto);
            }


        }









        // Bu action, StudentListDto'ları içeren bir listeyi PDF'ye çevirir ve tarayıcıya gönderir.




    }
}
