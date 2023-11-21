using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public async Task<IActionResult> GetTeachers()
        {
            var response = await _appUserService.GetAllTeacherAsync((int)RoleType.Teacher);

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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","images", "teacherImages", fileName + extName);
                string pathForDb = "\\images\\teacherImages\\" + fileName + extName;
                var stream = new FileStream(path, FileMode.Create);
                await model.ImagePath.CopyToAsync(stream);
                newdto.ImagePath = pathForDb;
                Console.WriteLine("selam");

            }
            newdto.FirstName = model.FirstName;
            newdto.SecondName = model.SecondName;
            newdto.Status = model.Status;
            newdto.PhoneNumber = model.PhoneNumber;
            newdto.Email = model.Email;
            newdto.Password = model.Email;

            var response = await _appUserService.CreateWithRoleAsync(newdto, (int)RoleType.Teacher);
            return this.ResponseRedirectAction(response, "GetTeachers");
        }

        public async Task<IActionResult> UpdateTeacher(int id)
        {
            var data = await _appUserService.GetByIdAsync<AppTeacherUpdateDto>(id);
            return this.ResponseView(data);
        }
        public async Task<IActionResult> RemoveTeacher(int id)
        {
            var response = await _appUserService.RemoveAsync(id);
            return this.ResponseRedirectAction(response,"GetTeachers");
        }
    }
}
