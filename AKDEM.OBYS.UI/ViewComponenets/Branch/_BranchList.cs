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
        public IViewComponentResult Invoke(int id)
        {
            var response = _appBranchService
            return View(response);
        }
    }
}
