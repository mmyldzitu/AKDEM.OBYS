using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Managers
{
   public class AppLessonManager:GenericManager<AppLessonCreateDto,AppLessonUpdateDto,AppLessonListDto,AppLesson>,IAppLessonService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public AppLessonManager(IMapper mapper, IValidator<AppLessonCreateDto> createDtoValidator, IValidator<AppLessonUpdateDto> updateDtoValidator, IUow uow): base(mapper,createDtoValidator,updateDtoValidator,uow)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<AppLessonListDto>> GetLessonsByTeacher(bool status)
        {
            var query = _uow.GetRepositry<AppLesson>().GetQuery();
            var lessons = await query.Include(x => x.AppUser).Where(x=>x.Status==status).ToListAsync();
            var mappedList= _mapper.Map<List<AppLessonListDto>>(lessons);
            return mappedList;
        }
        public async Task ChangeLessonStatus(int id)
        {
            var query = _uow.GetRepositry<AppLesson>().GetQuery();
            var lesson = await query.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (lesson != null)
            {
                if (lesson.Status)
                {
                    lesson.Status = false;
                }
                else { lesson.Status = true; }
            }
            await _uow.SaveChangesAsync();
        }
        public async Task<string> ReturnJustLessonName(int lessonId)
        {
            var query = _uow.GetRepositry<AppLesson>().GetQuery();
            var lesson = await query.Where(x => x.Id == lessonId).SingleOrDefaultAsync();
            if (lesson != null)
            {
                return lesson.Definition;
            }
            return "";
        }
        public async Task<string> GetLessonNameByLessonId(int lessonId)
        {
            string definition = "";
            var query = _uow.GetRepositry<AppLesson>().GetQuery();
            var entity = await query.Include(x => x.AppUser).Where(x => x.Id == lessonId).SingleOrDefaultAsync();
            if (entity != null)
            {
                definition = $"{entity.Definition}({entity.AppUser.FirstName} {entity.AppUser.SecondName})";
            }
            
            return definition;
        }
        public async Task<List<AppLessonListDto>> TeacherActiveLessons(int sessionId, int userId)
        {
            var query = _uow.GetRepositry<AppLesson>().GetQuery();
            var entities = await query.Include(x => x.AppUser).Where(x => x.AppUser.Id == userId && x.AppUserSessionLessons.Any(x => x.AppUserSession.SessionId == sessionId)).ToListAsync();
            if (entities.Count != 0)
            {
                var mappedList = _mapper.Map<List<AppLessonListDto>>(entities);
                return mappedList;
            }
            return new List<AppLessonListDto>();
        }
        
        public async Task AppLessonCreate(AppLessonCreateDto dto)
        {
            var mappedDto = _mapper.Map<AppLesson>(dto);
            await _uow.GetRepositry<AppLesson>().CreateAsync(mappedDto);
            await _uow.SaveChangesAsync();
        }
        public async Task AppLessonUpdate(AppLessonUpdateDto dto)
        {
            var entity = await _uow.GetRepositry<AppLesson>().FindByFilterAsync(x=>x.Id==dto.Id);
            var mappedDto = _mapper.Map<AppLesson>(dto);
            _uow.GetRepositry<AppLesson>().Update(mappedDto, entity);
            await _uow.SaveChangesAsync();
        }


    }
}
