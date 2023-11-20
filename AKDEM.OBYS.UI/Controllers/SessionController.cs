using AKDEM.OBYS.Business.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class SessionController : Controller
    {
        private readonly IAppSessionService _appSessionService;

        public SessionController(IAppSessionService appSessionService)
        {
            _appSessionService = appSessionService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _appSessionService.GetAllAsync();
            
            return View(response.Data);
        }
    }
}
