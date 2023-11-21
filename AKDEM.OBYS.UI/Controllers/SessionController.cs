using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.UI.Extensions;
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
        public async Task<IActionResult> ChangeStatus(int id)
        {
            await _appSessionService.SetStatusAsync(id);
            return RedirectToAction("Index", "Session");
        }
        public IActionResult CreateSession()
        {
            return View(new AppSessionCreateDto());
        }
        [HttpPost]
        public async Task<IActionResult> CreateSession(AppSessionCreateDto dto)
        {
            var response=await _appSessionService.CreateAsync(dto);
            return this.ResponseRedirectAction(response, "Index");
        }
       
    }
}
