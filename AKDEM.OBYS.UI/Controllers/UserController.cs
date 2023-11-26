using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public UserController(IAppUserService appUserService, IMapper mapper, IValidator<AppTeacherUpdateModel> teacherUpdateModelValidator, IAppBranchService appBranchService, IAppStudentService appStudentService)
        {
            _appUserService = appUserService;
            _mapper = mapper;
            _teacherUpdateModelValidator = teacherUpdateModelValidator;
            _appBranchService = appBranchService;
            _appStudentService = appStudentService;
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

            var response = await _appUserService.CreateTeacherWithRoleAsync(newdto, (int)RoleType.Teacher);
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


        public async Task<IActionResult> GetStudents()
        {
            var response = await _appUserService.GetAllStudentAsync(RoleType.Student);

            return View(response);
        }
        public async Task <IActionResult> CreateStudent()
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


            var list2 = new List<AppBranchListDto>();
            var items2 = await _appBranchService.GetList();
            foreach (var item in items2)
            {
                list2.Add(new AppBranchListDto
                {
                     Id=item.Id,

                    Definition = item.Definition
                });;
            }
            ViewBag.branches = new SelectList(list2, "Id", "Definition");
            

            return View(new AppStudentCreateModel ());
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudent(AppStudentCreateModel model)
        {
            AppStudentUpdateDto dto = new();
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
            dto.FirstName = model.FirstName;
            dto.SecondName = model.SecondName;
            dto.PhoneNumber = model.PhoneNumber;
            dto.Email = model.Email;
            dto.Password = model.Email;
            dto.BranchId = model.BranchId;
            dto.ClassId = model.ClassId;
            dto.Status = model.Status;
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
            return RedirectToAction("GetStudents");


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
        [HttpPost]
        public async Task<IActionResult> UpdateStudent(AppStudentUpdateModel model)
        {
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
            return RedirectToAction("GetStudents");

        }
        public async Task<IActionResult> RemoveStudent(int id)
        {
            var response = await _appStudentService.RemoveAsync(id);
            return this.ResponseRedirectAction(response, "GetStudents");
        }



    }
}
