using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
   public class AppLessonProfile:Profile
    {
        public AppLessonProfile()
        {
            CreateMap<AppLesson, AppLessonCreateDto>().ReverseMap();
            CreateMap<AppLesson, AppLessonUpdateDto>().ReverseMap();
            CreateMap<AppLesson, AppLessonListDto>().ReverseMap();

            CreateMap<AppLesson, AppLessonListDtoDeveloper>().ReverseMap();
        }
    }
}
