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
        public async Task<List<AppLessonListDto>> GetLessonsByTeacher()
        {
            var query = _uow.GetRepositry<AppLesson>().GetQuery();
            var lessons = await query.Include(x => x.AppUser).ToListAsync();
            var mappedList= _mapper.Map<List<AppLessonListDto>>(lessons);
            return mappedList;
        }
        
    }
}
