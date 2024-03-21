using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppGraduatedDtos;
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
    public class AppGraduatedManager:GenericManager<AppGraduatedCreateDto,AppGraduatedUpdateDto,AppGraduatedListDto,AppGraduated>, IAppGraduatedService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IAppUserService _appUserService;
        private readonly IAppSessionService _appSessionService;
        public AppGraduatedManager(IMapper mapper, IValidator<AppGraduatedCreateDto> createDtoValidator, IValidator<AppGraduatedUpdateDto> updateDtoValidator, IUow uow, IAppUserService appUserService, IAppSessionService appSessionService) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
            _appUserService = appUserService;
            _appSessionService = appSessionService;
        }

        public async Task<List<AppGraduatedListDto>> GetGradutedStudentsDeveloper()
        {
            var query = _uow.GetRepositry<AppGraduated>().GetQuery();
            var values = await query.ToListAsync();
            if(values.Count!=0)
            {
                var lists = _mapper.Map<List<AppGraduatedListDto>>(values);
                return lists;
            }
            return new List<AppGraduatedListDto>();
        }
        public async Task GraduateStudents(int userId, int sessionId, int order)
        {
            string zeros = "0000";
            if (order < 10)
            {
                zeros = "000";
            }
            else if(order>=10 && order < 100)
            {
                zeros = "00";
            }
            else if(order>=100 && order < 1000)
            {
                zeros = "0";
            }
            string userName = await _appUserService.GetUserNameById(userId);
            string sessionName = await _appSessionService.ReturnSessionName(sessionId);
            string president = await _appSessionService.ReturnPresidentName(sessionId);
            DateTime nowDate = DateTime.Now;
            string date = nowDate.ToString("dd.MM.yyyy");
            string year = nowDate.Year.ToString();
            string belgeNo = $"{year}-{zeros}{order}";

            AppGraduatedCreateDto dto = new AppGraduatedCreateDto {UserId=userId, studentName = userName, belgeNo = belgeNo, Date = date, President = president, year = year };
            var mappedDto = _mapper.Map<AppGraduated>(dto);
            await _uow.GetRepositry<AppGraduated>().CreateAsync(mappedDto);
            await _uow.SaveChangesAsync();
        }
        public async Task<AppGraduatedListDto> CertificaofUser(int userId)
        {
            var query = _uow.GetRepositry<AppGraduated>().GetQuery();
            var user = await query.Where(x => x.UserId == userId).SingleOrDefaultAsync();
            if (user != null)
            {
                var mappedList = _mapper.Map<AppGraduatedListDto>(user);
                return mappedList;
            }
            return new AppGraduatedListDto {UserId=userId, studentName="NotExists"};
        }
        public async Task RemoveCertificateByUserId(int userId)
        {
            var query = _uow.GetRepositry<AppGraduated>().GetQuery();
            var certifica = await query.Where(x => x.UserId == userId).SingleOrDefaultAsync();
            if (certifica != null)
            {
                _uow.GetRepositry<AppGraduated>().Remove(certifica);
            }
            await _uow.SaveChangesAsync();
        }
    }
}
