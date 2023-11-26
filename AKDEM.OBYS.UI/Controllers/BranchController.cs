using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using AKDEM.OBYS.UI.Extensions;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BranchController(IAppBranchService appBranchService, IValidator<AppBranchCreateModel> branchCreateModelValidator, IMapper mapper, IAppStudentService appStudentService)
        {
            _appBranchService = appBranchService;
            _branchCreateModelValidator = branchCreateModelValidator;
            _mapper = mapper;
            _appStudentService = appStudentService;
        }

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

        
        public IActionResult CreateBranch(int id)
        {

            return View(new AppBranchCreateModel { ClassId = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(AppBranchCreateModel model)
        {
            model.Definition = $"{model.Class}/{model.Branch}";
            var result = _branchCreateModelValidator.Validate(model);
            if (result.IsValid)
            {
                ViewBag.classId = model.ClassId;
                var dto = _mapper.Map<AppBranchCreateDto>(model);
                var createResponse = await _appBranchService.CreateAsync(dto);
                return this.ResponseRedirectAction(createResponse, "ReturnRelatedBranches", parameter:model.ClassId);
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
        [HttpGet]
        public async Task<IActionResult> GetBranches(int ClassId)
        {
            if (ClassId == 6)
            {
                var allbranches = await _appBranchService.GetList();
                return Json(allbranches);
            }

            var branches = await _appBranchService.GetClasses(ClassId);
            foreach(var item in branches)
            {
               
            }
            return Json(branches);
        }
        public async Task<IActionResult> BranchDetails(int id)
        {
            var response = await _appStudentService.GetStudentsWithBranchAsync(id);
            return View(response);
        }

    }

}

    

