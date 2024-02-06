using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.UI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IAppScheduleService _appScheduleService;
        private readonly IAppLessonService _appLessonService;
        private readonly IAppBranchService _appBranchService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IAppSessionService _appSessionService;
        private readonly IAppWarningService _appWarningService;
        private readonly IValidator<AppScheduleCreateModel> _appScheduleCreateModelValidator;


        public ScheduleController(IAppScheduleService appScheduleService, IAppLessonService appLessonService, IAppBranchService appBranchService, IAppUserSessionService appUserSessionService, IAppUserSessionLessonService appUserSessionLessonService, IAppScheduleDetailService appScheduleDetailService, IAppSessionService appSessionService, IAppWarningService appWarningService, IValidator<AppScheduleCreateModel> appScheduleCreateModelValidator)
        {
            _appScheduleService = appScheduleService;
            _appLessonService = appLessonService;
            _appBranchService = appBranchService;
            _appUserSessionService = appUserSessionService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appScheduleDetailService = appScheduleDetailService;
            _appSessionService = appSessionService;
            _appWarningService = appWarningService;
            _appScheduleCreateModelValidator = appScheduleCreateModelValidator;
        }

        public async Task<IActionResult> Index(int id)
        {
            var result = await _appScheduleService.GetSchedulesBySession(id);
            ViewBag.sessionId = id;
            ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(id);
            return this.View(result);
        }
        public async Task<IActionResult> CreateSchedule(int id)
        {
            var list = new List<AppScheduleSelectModel>();
            var items = await _appBranchService.GetBranchListNotScheduleAsync(id);
            foreach (var item in items)
            {
                list.Add(new AppScheduleSelectModel
                {
                    Id = item.Id,
                    BranchDetail = item.Definition
                });
            }
            ViewBag.branches = new SelectList(list, "Id", "BranchDetail");
            ViewBag.sessionId = id;
            return View(new AppScheduleCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateSchedule(AppScheduleCreateModel model)
        {
            var modelResult = _appScheduleCreateModelValidator.Validate(model);
            var list = new List<AppScheduleSelectModel>();
            var items = await _appBranchService.GetBranchListNotScheduleAsync(model.SessionId);
            foreach (var item in items)
            {
                list.Add(new AppScheduleSelectModel
                {
                    Id = item.Id,
                    BranchDetail = item.Definition
                });
            }
            ViewBag.branches = new SelectList(list, "Id", "BranchDetail");
            ViewBag.sessionId = model.SessionId;
            if (modelResult.IsValid)
            {
                AppScheduleCreateDto dto = new();
                dto.Definition = model.Definition;
                dto.SessionBranchId = await _appScheduleService.GetSessionBranchIdBySessionIdandBranchId(model.SessionId, model.BranchId);
                var result = await _appScheduleService.CreateScheduleAsync(dto);
                

                if (result.ResponseType == ResponseType.Success)
                {
                    ViewBag.message = 1;
                    ViewBag.sessionBranchId = dto.SessionBranchId;
                    ViewBag.branchId = model.BranchId;

                    return View(new AppScheduleCreateModel());
                }
                else
                {
                    foreach (var item in result.ValidationErrors)
                    {
                        ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                    }
                    return View(result.Data);
                }

                
            }
            foreach (var error in modelResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(model);



        }
        public async Task<IActionResult> RemoveSchedule(int scheduleId)
        {
            List<int> userIds = new List<int>();
            var branchId = await _appScheduleService.GetBranchIdByScheduleId(scheduleId);
            var sessionId = await _appScheduleService.GetSessionIdByScheduleId(scheduleId);
            var userSessionList = await _appUserSessionService.GetUserSessionsByBranchId(sessionId, branchId);
            foreach ( var userSessionId in userSessionList)
            {
                userIds.Add(await _appUserSessionService.GetUserIdByUserSessionId(userSessionId));
            }
            await _appUserSessionLessonService.RemoveUserSessionLessonsByUserSessionListAsync(userSessionList);
            
            await _appScheduleDetailService.RemoveScheduleDetailsByScheduleId(scheduleId);
            await _appScheduleService.RemoveScheduleByScheduleId(scheduleId);
            foreach ( var userId in userIds)
            {
                int userSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, sessionId);
                await _appWarningService.RemoveWarningByUserIdandSessionId(userId, sessionId,userSessionId);
                await _appUserSessionService.SetZeroToSessionAverage(userId, sessionId);
                await _appUserSessionService.TotalAverageByUserId(userId,sessionId);
            }
            
            return RedirectToAction("Index", new {id=sessionId });
        }

    }
}
