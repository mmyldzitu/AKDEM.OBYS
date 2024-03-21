using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppSessionBranchDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeveloperController : Controller
    {
        private readonly IAppBranchService _appBranchService;
        private readonly IAppGraduatedService _appGraduatedService;
        private readonly IAppLessonService _appLessonService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IAppScheduleService _appScheduleService;
        private readonly IAppSessionBranchService _appSessionBranchService;
        private readonly IAppSessionService _appSessionService;
        private readonly IAppUserService _appUserService;
        private readonly IAppUserSessionLessonService   _appUserSessionLessonService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppWarningService _appWarningService;
        public DeveloperController(IAppBranchService appBranchService, IAppGraduatedService appGraduatedService, IAppLessonService appLessonService, IAppScheduleDetailService appScheduleDetailService, IAppScheduleService appScheduleService, IAppSessionBranchService appSessionBranchService, IAppSessionService appSessionService, IAppUserService appUserService, IAppUserSessionLessonService appUserSessionLessonService, IAppUserSessionService appUserSessionService, IAppWarningService appWarningService)
        {
            _appBranchService = appBranchService;
            _appGraduatedService = appGraduatedService;
            _appLessonService = appLessonService;
            _appScheduleDetailService = appScheduleDetailService;
            _appScheduleService = appScheduleService;
            _appSessionBranchService = appSessionBranchService;
            _appSessionService = appSessionService;
            _appUserService = appUserService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appUserSessionService = appUserSessionService;
            _appWarningService = appWarningService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAppBranches()
        {
            var values = await _appBranchService.GetBranchesDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppClasses()
        {
            return View();
        }
        public async Task<IActionResult> GetGraduatedStudents()
        {
            var values = await _appGraduatedService.GetGradutedStudentsDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppLessons()
        {
            var values = await _appLessonService.GetLessonsDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppScheduleDetails()
        {
            var values = await _appScheduleDetailService.GetAppScheduleDetailsDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppSchedules()
        {
            var values = await _appScheduleService.GetAppSchedulesDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppSessionBranches()
        {
            var values = await _appSessionBranchService.GetAppSessionBranchesDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppSessions()
        {
            var values = await _appSessionService.GetAppSessionsDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppUsers()
        {
            var values = await _appUserService.GetAppUsersDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppUserSessionLessons()
        {
            var values = await _appUserSessionLessonService.GetAppUserSessionLessonsDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppUserSessions()
        {
            var values = await _appUserSessionService.GetAppUserSessionsDeveloper();
            return View(values);
        }
        public async Task<IActionResult> GetAppUserWarnings()
        {
            var values = await _appWarningService.GetAppWarningsDeveloper();
            return View(values);
        }
        public IActionResult CreateAppSessionBranch()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppSessionBranch(AppSessionBranchCreateDto dto)
        {

            await _appSessionBranchService.CreateSessionBranchDeveloper(dto);
            return RedirectToAction("GetAppSessionBranches");
        }
        public async Task< IActionResult> UpdateAppSessionBranch(int id)
        {
            var sessionBranch = await _appSessionBranchService.GetAppSessionBranchDeveloper(id);
            return View( new AppSessionBranchUpdateDtoDeveloper { Id=sessionBranch.Id, BranchId=sessionBranch.BranchId, SessionId=sessionBranch.SessionId});
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAppSessionBranch(AppSessionBranchUpdateDtoDeveloper dto)
        {

            await _appSessionBranchService.UpdateSessionBranchDeveloper(dto);
            return RedirectToAction("GetAppSessionBranches");
        }

        public async Task<IActionResult> RemoveAppSessionBranch(int id)
        {
            await _appSessionBranchService.RemoveAppSessionBranchDeveloper(id);
            return RedirectToAction("GetAppSessionBranches");

        }
        public IActionResult CreateAppUserSessionLesson()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppUserSessionLesson(AppUserSessionLessonCreateDto dto)
        {

            await _appUserSessionLessonService.CreateUserSessionLessonDeveloper(dto);
            return RedirectToAction("GetAppUserSessionLessons");
        }
        public async Task<IActionResult> UpdateAppuserSessionLesson(int id)
        {
            var userSessionLesson = await _appUserSessionLessonService.GetAppUserSessionLessonDeveloper(id);
            return View(new AppUserSessionLessonUpdateDtoDeveloper { Id=userSessionLesson.Id, LessonId=userSessionLesson.LessonId, UserSessionId=userSessionLesson.UserSessionId, Not=userSessionLesson.Not, Devamsızlık=userSessionLesson.Devamsızlık });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAppUserSessionLesson(AppUserSessionLessonUpdateDtoDeveloper dto)
        {

            await _appUserSessionLessonService.UpdateUserSessionLessonDeveloper(dto);
            return RedirectToAction("GetAppUserSessionLessons");
        }

        public async Task<IActionResult> RemoveAppUserSessionLesson(int id)
        {
            await _appUserSessionLessonService.RemoveAppUserSessionLessonDeveloper(id);
            return RedirectToAction("GetAppUserSessionLessons");


        }

        public IActionResult CreateAppUserSession()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAppUserSession(AppUserSessionCreateDtoDeveloper dto)
        {

            await _appUserSessionService.CreateUserSessionDeveloper(dto);
            return RedirectToAction("GetAppUserSessions");
        }
        public async Task<IActionResult> UpdateAppUserSession(int id)
        {
            var userSession = await _appUserSessionService.GetAppUserSessionDeveloper(id);
            return View(new AppUserSessionUpdateDtoDeveloper { Id = userSession.Id, Average = userSession.Average, SessionWarningCount = userSession.SessionWarningCount, SessionLessonWarningCount = userSession.SessionLessonWarningCount, SessionAbsentWarningCount = userSession.SessionAbsentWarningCount, UserId=userSession.UserId, SessionId=userSession.SessionId, BranchId=userSession.BranchId, ClassId=userSession.ClassId, Status=userSession.Status });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAppUserSession(AppUserSessionUpdateDtoDeveloper dto)
        {

            await _appUserSessionService.UpdateUserSessionDeveloper(dto);
            return RedirectToAction("GetAppUserSessions");
        }

        public async Task<IActionResult> RemoveAppUserSession(int id)
        {
            await _appUserSessionService.RemoveAppUserSessionDeveloper(id);
            return RedirectToAction("GetAppUserSessions");


        }
        public async Task<IActionResult> UpdateAppUser(int id)
        {
            var user = await _appUserService.GetAppUserDeveloper(id);
            return View(new AppUserUpdateDtoDeveloper { Id = user.Id, FirstName = user.FirstName, SecondName = user.SecondName, Email = user.Email, Password = user.Password, PhoneNumber = user.PhoneNumber, ImagePath = user.ImagePath, BranchId = user.BranchId, ClassId = user.ClassId, Status = user.Status,DepartReason=user.DepartReason,SıraNo=user.SıraNo,TotalAverage=user.TotalAverage,TotalWarningCount=user.TotalWarningCount });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAppUser(AppUserUpdateDtoDeveloper dto)
        {

            await _appUserService.UpdateUserDeveloper(dto);
            return RedirectToAction("GetAppUsers");
        }

       
    }

}
