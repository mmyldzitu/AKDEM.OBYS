using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class LessonController : Controller
    {
        private readonly IAppLessonService _appLessonService;
        private readonly IAppUserService _appUserService;

        public LessonController(IAppLessonService appLessonService, IAppUserService appUserService)
        {
            _appLessonService = appLessonService;
            _appUserService = appUserService;
        }

        public async Task <IActionResult> Index()
        {
            var response = await _appLessonService.GetLessonsByTeacher();
            return this.View(response);
            
        }
        public async Task <IActionResult> CreateLesson()
        {
            var list = new List<AppLessonListModel>();
            
            var items = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher);
           
            foreach (var item in items.Data)
            {
                list.Add(new AppLessonListModel
                {
                    Id = item.Id,
                    TeacherName =$"{ item.FirstName} { item.SecondName}"
                   
                   
                    
                });
            }
            ViewBag.teachers = new SelectList(list, "Id","TeacherName" );

            return View(new AppLessonCreateDto());

        }
        [HttpPost]
        public async Task <IActionResult> CreateLesson(AppLessonCreateDto dto)
        {
            var response = await _appLessonService.CreateAsync(dto);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach (var error in response.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                var list = new List<AppLessonListModel>();

                var items = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher);
                foreach (var item in items.Data)
                {
                    list.Add(new AppLessonListModel
                    {
                        Id = item.Id,
                        TeacherName = $"{ item.FirstName} { item.SecondName}"



                    });
                }
                ViewBag.teachers = new SelectList(list, "Id", "TeacherName");
                return View(dto);


            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateLesson(int id)
        {
             
            var response = await _appLessonService.GetByIdAsync<AppLessonUpdateDto>(id);
            var list = new List<AppLessonListModel>();

            var items = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher);

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
        [HttpPost]
        public async Task<IActionResult> UpdateLesson(AppLessonUpdateDto dto)
        {
            var response =await _appLessonService.UpdateAsync(dto);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach(var error in response.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                var list = new List<AppLessonListModel>();

                var items = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher);

                foreach (var item in items.Data)
                {
                    list.Add(new AppLessonListModel
                    {
                        Id = item.Id,
                        TeacherName = $"{ item.FirstName} { item.SecondName}"



                    });
                }
                ViewBag.teachers = new SelectList(list, "Id", "TeacherName");

                return View(dto);
            }
            return RedirectToAction("Index");
        }
    }
}
