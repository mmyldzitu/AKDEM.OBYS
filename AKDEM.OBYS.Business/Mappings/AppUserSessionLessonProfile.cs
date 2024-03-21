using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
public    class AppUserSessionLessonProfile:Profile
    {
        public AppUserSessionLessonProfile()
        {
            CreateMap<AppUserSessionLessonListDto, AppUserSessionLesson>().ReverseMap();
            CreateMap<AppUserSessionLessonCreateDto, AppUserSessionLesson>().ReverseMap();
            CreateMap<AppUserSessionLessonUpdateDto, AppUserSessionLesson>().ReverseMap();

            CreateMap<AppUserSessionLessonListDtoDeveloper, AppUserSessionLesson>().ReverseMap();

        }
    }
}
