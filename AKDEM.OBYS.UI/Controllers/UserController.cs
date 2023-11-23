using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
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
        
        private readonly IMapper _mapper;
        private readonly IValidator<AppTeacherUpdateModel> _teacherUpdateModelValidator;
        

        public UserController(IAppUserService appUserService, IMapper mapper, IValidator<AppTeacherUpdateModel> teacherUpdateModelValidator)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _teacherUpdateModelValidator = teacherUpdateModelValidator;
            
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
            newdto.Password = model.Email;

            var response = await _appUserService.CreateWithRoleAsync(newdto, (int)RoleType.Teacher);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach(var error in response.ValidationErrors)
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
                    string pathforDb = Path.Combine("\\","images", "teacherImages", fileName + extName);
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
            
            var response = await _appUserService.UpdateAsync(newdto);
            if (response.ResponseType == ResponseType.ValidationError)
            {
                foreach(var error in response.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            return RedirectToAction("GetTeachers");
            
        }
        public async Task<IActionResult> RemoveTeacher(int id)
        {
            var response = await _appUserService.RemoveAsync(id);
            return this.ResponseRedirectAction(response,"GetTeachers");
        }

        
    }
}
