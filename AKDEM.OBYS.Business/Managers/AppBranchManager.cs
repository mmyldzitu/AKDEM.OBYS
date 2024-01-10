using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
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
    public class AppBranchManager : GenericManager<AppBranchCreateDto, AppBranchUpdateDto, AppBranchListDto, AppBranch>, IAppBranchService

    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public AppBranchManager(IMapper mapper, IValidator<AppBranchCreateDto> createDtoValidator, IValidator<AppBranchUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<AppBranchListDto>> GetList()
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var list = await query.Include(x => x.AppClass).Where(x=>x.Status==true).ToListAsync();
            if (list.Count != 0)
            {
                return _mapper.Map<List<AppBranchListDto>>(list);

            }
            return new List<AppBranchListDto>();
        }
        public async Task SessionEndingBranchStatus(int branchId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var entity = await query.Where(x => x.Id == branchId).SingleOrDefaultAsync();
            if (entity != null)
            {
                entity.Status = false;
            }
            await _uow.SaveChangesAsync();
        }
        public async Task<List<AppBranchListDto>> GetBranchListWithSessionId(int sessionId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var list = await query.Include(x => x.AppClass).Where(x => x.AppUserSessions.Any(x=>x.SessionId==sessionId)).ToListAsync();
            if (list.Count != 0)
            {
                return _mapper.Map<List<AppBranchListDto>>(list);

            }
            return new List<AppBranchListDto>();
        }
        public async Task<List<AppBranchListDto>> GetListWithClassId(int classId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var list = await query.Include(x => x.AppClass).Where(x=>x.ClassId==classId).ToListAsync();
            if (list.Count != 0)
            {
                return _mapper.Map<List<AppBranchListDto>>(list);

            }
            return new List<AppBranchListDto>();
        }
        public async Task<AppBranchListDto> GetBrancWithId(int branchId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var entity = await query.Include(x => x.AppClass).Where(x => x.Id == branchId).SingleOrDefaultAsync();
            if (entity !=null)
            {
                return _mapper.Map<AppBranchListDto>(entity);

            }
            return new AppBranchListDto();
        }

        public async Task<List<AppBranchListDto>> GetClasses(int id)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var list = await query.Include(x => x.AppClass).Where(x => x.ClassId == id && x.Status==true).ToListAsync();
            return _mapper.Map<List<AppBranchListDto>>(list);
        }
        public async Task<List<AppBranchListDto>> GetBranchListNotScheduleAsync(int sessionId)
        {
            var branches = await _uow.GetRepositry<AppBranch>().GetAllAsync(x => x.AppSessionBranches.Any(x => x.SessionId == sessionId && x.AppSchedules.Count==0) /*&& x.AppSessionBranches.Any(x => x.AppSchedules.Any(x=>x.Definition==null))*/);
            
            


            return _mapper.Map<List<AppBranchListDto>>(branches);

        }
        public async Task<int> GetBranchIdByBranchDefinitionAsync(string definition)
        {
            var entity = await _uow.GetRepositry<AppBranch>().FindByFilterAsync(x => x.Definition == definition && x.Status==true);
            if (entity != null)
            {
                return entity.Id;

            }
            return 0;
        }
        public async Task<int> GetClassIdByBranchId(int branchId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var entity = await query.Where(x => x.Id == branchId).FirstOrDefaultAsync();
            if (entity != null)
            {
                return entity.ClassId;
            }
            return 0;
        }
        public async Task<string> BranchNameByByBranchId(int branchId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var entity = await query.Where(x => x.Id == branchId).SingleOrDefaultAsync();
            if(entity != null)
            {
                return entity.Definition;
            }
            return "";
        }
        public async Task<AppClassListDto> GetClassById(int id)
        {
            var query = _uow.GetRepositry<AppClass>().GetQuery();
            var entity = await query.Where(x => x.Id == id).SingleOrDefaultAsync();
            if(entity != null)
            {
               var mappedList = _mapper.Map<AppClassListDto>(entity);
                return mappedList;
            }
            return new AppClassListDto();
        }
        public async Task<AppBranchListDto> FindBranchByNameAndStatus(string name)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var entity = await query.Where(x => x.Definition == name && x.Status==true).SingleOrDefaultAsync();
            if (entity != null)
            {
                var mappedList = _mapper.Map<AppBranchListDto>(entity);
                return mappedList;
            }
            return new AppBranchListDto();
        }
        public async Task<string> FindClassNameByClassId(int classId)
        {
            var query = _uow.GetRepositry<AppClass>().GetQuery();
            var entity = await query.Where(x => x.Id == classId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Definition;
            }
            return "";
        }
        public async Task CreateBranchWhichNotExist(AppBranchCreateDto dto)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var branch = await query.Where(x => x.Definition == dto.Definition && x.Status==true).SingleOrDefaultAsync();
            if (branch == null)
            {
                var myEntity = _mapper.Map<AppBranch>(dto);
                await _uow.GetRepositry<AppBranch>().CreateAsync(myEntity);
                await _uow.SaveChangesAsync();
            }
        }
        
        


    }
}