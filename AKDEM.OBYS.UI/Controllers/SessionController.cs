using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.SessionType;
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
    public class SessionController : Controller
    {
        private readonly IAppSessionService _appSessionService;

        public SessionController(IAppSessionService appSessionService)
        {
            _appSessionService = appSessionService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _appSessionService.GetOrderingAsync();
            
            return View(response.Data);
        }
        public async Task<IActionResult> ChangeStatus(int id)
        {
            await _appSessionService.SetStatusAsync(id);
            return RedirectToAction("Index", "Session");
        }
        public IActionResult CreateSession()
        {
            var items = Enum.GetValues(typeof(SessionType));
            var list = new List<SessionTypeListDto>();
            foreach (int item in items)
            {
                list.Add(new SessionTypeListDto
                {
                    Id = item,
                    Definition = Enum.GetName(typeof(SessionType), item)
                });

            }
            ViewBag.sessions = new SelectList(list, "Id", "Definition");
            return View(new AppSessionCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateSession(AppSessionCreateModel model)
        {
            model.Definition = $"{model.year1}-{model.year2}/{ Enum.GetName(typeof(SessionType), model.SessionType)}";
            
            AppSessionCreateDto dto = new AppSessionCreateDto();
            dto.Definition = model.Definition;
            dto.Status = model.Status;
            var response=await _appSessionService.CreateAsync(dto);
            return this.ResponseRedirectAction(response, "Index");
        }
       
    }
}
