using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppLessonService:IGenericService<AppLessonCreateDto,AppLessonUpdateDto,AppLessonListDto,AppLesson>
    {
        Task<List<AppLessonListDto>> GetLessonsByTeacher();
        Task<string> GetLessonNameByLessonId(int lessonId);
        Task<List<AppLessonListDto>> TeacherActiveLessons(int sessionId, int userId);
    }
}
