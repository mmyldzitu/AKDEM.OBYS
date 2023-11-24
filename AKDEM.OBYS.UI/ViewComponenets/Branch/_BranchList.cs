using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppBranchDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ViewComponenets.Branch
{
    public class _BranchList:ViewComponent
    {
        private readonly IAppBranchService _appBranchService;

        public _BranchList(IAppBranchService appBranchService)
        {
            _appBranchService = appBranchService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            ViewBag.classId = id;

            if (id == 6)
            {
                var Alllist = await _appBranchService.GetList();
                return View(Alllist);

            }
            else
            {
                var list = await _appBranchService.GetClasses(id);

                return View(list);
            }
            
        }
    }
}
