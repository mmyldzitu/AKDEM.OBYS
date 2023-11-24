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
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class BranchController : Controller
    {
        private readonly IAppBranchService _appBranchService;
        private readonly IValidator<AppBranchCreateModel> _branchCreateModelValidator;
        private readonly IMapper _mapper;

        public BranchController(IAppBranchService appBranchService, IValidator<AppBranchCreateModel> branchCreateModelValidator, IMapper mapper)
        {
            _appBranchService = appBranchService;
            _branchCreateModelValidator = branchCreateModelValidator;
            _mapper = mapper;
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
           
            return View(new AppBranchCreateModel { ClassId=id});
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(AppBranchCreateModel model)
        {
            var result = _branchCreateModelValidator.Validate(model);
            if (result.IsValid)
            {
                ViewBag.classId = model.ClassId;
                var dto = _mapper.Map<AppBranchCreateDto>(model);
                var createResponse = await _appBranchService.CreateAsync(dto);
                return this.ResponseRedirectAction(createResponse,"Index");
            }
            foreach( var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            
            return View(model);
        }
        
    }
}
