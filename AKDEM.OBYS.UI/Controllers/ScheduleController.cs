using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IAppScheduleService _appScheduleService;

        public ScheduleController(IAppScheduleService appScheduleService)
        {
            _appScheduleService = appScheduleService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var response = await _appScheduleService.GetSchedulesBySessionId(id);
            return this.View(response);
        }
    }
}
