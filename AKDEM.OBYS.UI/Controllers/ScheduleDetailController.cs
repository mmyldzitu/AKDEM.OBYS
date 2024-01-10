using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppScheduleDetailDto;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    public class ScheduleDetailController : Controller
    {
        private readonly IAppLessonService _appLessonService;
        private readonly IAppScheduleService _appScheduleService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IMapper _mapper;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppSessionService _appSessionService;
        private readonly IAppWarningService _appWarningService;

        public ScheduleDetailController(IAppLessonService appLessonService, IAppScheduleService appScheduleService, IAppScheduleDetailService appScheduleDetailService, IMapper mapper, IAppUserSessionService appUserSessionService, IAppUserSessionLessonService appUserSessionLessonService, IAppSessionService appSessionService, IAppWarningService appWarningService)
        {
            _appLessonService = appLessonService;
            _appScheduleService = appScheduleService;
            _appScheduleDetailService = appScheduleDetailService;
            _mapper = mapper;
            _appUserSessionService = appUserSessionService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appSessionService = appSessionService;
            _appWarningService = appWarningService;
        }
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.name = await _appScheduleService.GetNameByScheduleId(id);
            ViewBag.scheduleId = id;
            ViewBag.sessionBranchId = await _appScheduleService.GetSessionBranchIdByAppScheduleId(id);
            int sessionId = await _appScheduleService.GetSessionIdByScheduleId(id);
            ViewBag.sessionStatus2 = await _appSessionService.GetStatus2FromSessionId(sessionId);
            return View();
        }
         
            public async Task<IActionResult> CreateScheduleDetail(int sessionBranchId)
             {
            int sessionId = await _appScheduleService.GetSessionIdBySessionBranchId(sessionBranchId);

            int scheduleId = await _appScheduleService.GetAppScheduleIdBySessionBranchIdAsync(sessionBranchId);

                var list = new List<AppScheduleDetailLessonModel>();
            var items = await _appLessonService.GetLessonsByTeacher();
            foreach (var item in items)
            {
                list.Add(new AppScheduleDetailLessonModel
                {
                    Id=item.Id,
                    LessonDetail=$"{item.Definition}--{item.AppUser.FirstName} {item.AppUser.SecondName}"

                });
               
            }
            ViewBag.lessons = new SelectList(list, "Id", "LessonDetail");

            var list2 = new List<AppScheduleDetailWeekDaysModel>();
            var items2 = Enum.GetValues(typeof(WeekDaysType));
            foreach(var item in items2)
            {
                list2.Add(new AppScheduleDetailWeekDaysModel {
                Day= Enum.GetName(typeof(WeekDaysType), item),
                DayVal =Enum.GetName(typeof(WeekDaysType),item)
                
                });
            }
            ViewBag.days = new SelectList(list2, "Day", "DayVal");
            ViewBag.scheduleId = scheduleId;
            ViewBag.sessionId = sessionId;
            //ViewBag.branchId = branchId;
            return View(new AppScheduleDetailCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateScheduleDetail(AppScheduleDetailCreateModel model)
        {
            //int sessionId = await _appScheduleService.GetSessionIdByScheduleId(model.ApScheduleId);
            //------------------SELECTBOXLAR-----------------
            var list = new List<AppScheduleDetailLessonModel>();
            var items = await _appLessonService.GetLessonsByTeacher();
            foreach (var item in items)
            {
                list.Add(new AppScheduleDetailLessonModel
                {
                    Id = item.Id,
                    LessonDetail = $"{item.Definition}--{item.AppUser.FirstName} {item.AppUser.SecondName}"

                });

            }
            ViewBag.lessons = new SelectList(list, "Id", "LessonDetail");

            var list2 = new List<AppScheduleDetailWeekDaysModel>();
            var items2 = Enum.GetValues(typeof(WeekDaysType));
            foreach (var item in items2)
            {
                list2.Add(new AppScheduleDetailWeekDaysModel
                {
                    Day = Enum.GetName(typeof(WeekDaysType), item),
                    DayVal = Enum.GetName(typeof(WeekDaysType), item)

                });
            }
            ViewBag.days = new SelectList(list2, "Day", "DayVal");

            //----------SELECTBOXLAR------------------





            AppScheduleDetailCreateDto dto = new();
            dto.ApScheduleId = model.ApScheduleId;
            dto.Day = model.Day;
            dto.Hours = $"{model.FirstHour}-{model.LastHour}";
            dto.LessonId = model.LessonId;
            var result = await _appScheduleDetailService.CreateAsync(dto);
            
            if (result.ResponseType == ResponseType.Success)
            {
                ViewBag.scheduleId = model.ApScheduleId;
                //ViewBag.sessionId = sessionId;
                ViewBag.confirmMessage = 1;
                ViewBag.alertMessage = "Ders Bilgisi Cizelgeye Basariyla Eklenmistir. Yeni Dersler Eklemeye Devam Edebilirsiniz";
                return View(new AppScheduleDetailCreateModel());
            }
            else
            {
                foreach(var error in result.ValidationErrors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(result.Data);

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetScheduleDetailandHours(int scheduleId)
        {
            var hourList = await _appScheduleDetailService.GetHoursByScheduleIdAsync(scheduleId);
            var scheduleDetailList = await _appScheduleDetailService.GetScheduleDetailsByScheduleId(scheduleId);
            var branchName = await _appScheduleService.GetBranchNameByScheduleId(scheduleId);
            List<Models.AppScheduleDetailsTeacherModel> models = new();
            foreach( var item in scheduleDetailList)
            {
                Models.AppScheduleDetailsTeacherModel schedulemodel = new Models.AppScheduleDetailsTeacherModel
                {
                    AppLesson = item.AppLesson,
                    AppSchedule = item.AppSchedule,
                    Day = item.Day,
                    ıd = item.Id,
                    Hours=item.Hours,
                    LessonId=item.LessonId,
                    BranchDefinition=branchName,
                };
                models.Add(schedulemodel);
            }
            static List<string> GetHoursOrdered(List<string> hourList)
            {
                
                List<Tuple<DateTime, DateTime>> hoursTupList = new();
                foreach (var item in hourList)
                {
                    string[] hourParts = item.Split("-");
                    DateTime firstTime = DateTime.ParseExact(hourParts[0].Trim(), "HH:mm", null);
                    DateTime lastTime = DateTime.ParseExact(hourParts[1].Trim(), "HH:mm", null);
                    hoursTupList.Add(new Tuple<DateTime, DateTime>(firstTime, lastTime));
                }
                    hoursTupList.Sort((x, y) => x.Item1.CompareTo(y.Item1));
                    List<string> hoursOrdered = new();
                    foreach(var tup in hoursTupList)
                    {
                        string orderedHourPart = $"{tup.Item1:HH:mm}-{tup.Item2:HH:mm}";
                        hoursOrdered.Add(orderedHourPart);
                    }
                    return hoursOrdered;

                

            }
            List<string> hoursForSchedule = GetHoursOrdered(hourList);

            var hoursForScheduleDistinct = hoursForSchedule.Distinct().ToList();
            AppScheduleDetailsForJsonModel model = new(){ ScheduleHours = hoursForScheduleDistinct, AppScheduleDetails = models };

            return Json(model);
        }
        public async Task<IActionResult> RemoveScheduleDetail(int id)
        {
            
            int scheduleId = await _appScheduleDetailService.GetScheduleByIdScheduleDetailIdAsync(id);
            int sessionBranchId = await _appScheduleService.GetSessionBranchIdByAppScheduleId(scheduleId);
            int branchId = await _appScheduleService.GetBranchIdBySessionBranchId(sessionBranchId);
            int sessionId = await _appScheduleService.GetSessionIdBySessionBranchId(sessionBranchId);
            var appUserSessionIdList = await _appUserSessionService.GetUserSessionsByBranchId(sessionId, branchId);

            String lessonName = await _appScheduleDetailService.GetLessonNameByScheduleDetailIdAsync(id);
            foreach ( var userSession in appUserSessionIdList)
            {
                await _appUserSessionLessonService.RemoveUserSessionLessonByLessonNameAsync(userSession, lessonName);
            }
            await _appScheduleDetailService.RemoveScheduleDetailByLessonNameScheduleId(scheduleId, lessonName);
            foreach(var userSession in appUserSessionIdList)
            {
                int bigwarningcount = await _appWarningService.FindWarningCountByString("70", userSession);
                int userId = await _appUserSessionService.GetUserIdByUserSessionId(userSession);
                await _appWarningService.RemoveWarningByString(lessonName, userId, userSession);
                await _appUserSessionService.FindAverageOfSessionWithUserAndSession(userId, sessionId);
                await _appUserSessionService.TotalAverageByUserId(userId,sessionId);
                double sessionavr = await _appUserSessionService.ReturnSessionAverage(userSession);
                if (bigwarningcount==1 && sessionavr>=70)
                {
                    await _appWarningService.RemoveWarningByString("70", userId, userSession);
                }
            }
            
            return RedirectToAction("CreateScheduleDetail", new { sessionBranchId = sessionBranchId });
        }

        public async Task<IActionResult> SaveChanges(int scheduleId)
        {
            List<AppUserSessionLessonCreateDto> userSessionLessonCreateDtos = new();
            var lessonIdList = await _appScheduleService.GetLessonIdFromScheduleDetailsByScheduleId(scheduleId);
            //var sessionBranchId = await _appScheduleService.GetSessionBranchIdByAppScheduleId(scheduleId);
            var branchId = await _appScheduleService.GetBranchIdByScheduleId(scheduleId);
            var sessionId = await _appScheduleService.GetSessionIdByScheduleId(scheduleId);
            
            var appUserSessionIdList = await _appUserSessionService.GetUserSessionsByBranchId(sessionId,branchId);
            foreach (var userSessionId in appUserSessionIdList)
            {
                foreach (var lessonId in lessonIdList)
                {
                    userSessionLessonCreateDtos.Add(new AppUserSessionLessonCreateDto
                    {
                        LessonId = lessonId,
                        UserSessionId = userSessionId



                    });

                }
            }
            await _appUserSessionLessonService.CreateUserSessionLessonAsync(userSessionLessonCreateDtos);

            return RedirectToAction("CreateSchedule", "Schedule", new { id = sessionId });
        }





    }
}
