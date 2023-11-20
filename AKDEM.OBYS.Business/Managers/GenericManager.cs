using AKDEM.OBYS.Business.Extensions;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.Interfaces;
using AKDEM.OBYS.Entities;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Managers
{
    public class GenericManager<CreateDto, UpdateDto, ListDto,T> : IGenericService<CreateDto, UpdateDto, ListDto,T>
        where CreateDto : class, IDto, new()
        where UpdateDto : class, IUpdateDto, new()
        where ListDto : class, IDto, new()
        where T:BaseEntity
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDto> _createDtovalidator;
        private readonly IValidator<UpdateDto> _updateDtovalidator;
        
        private readonly IUow _uow;

        public GenericManager(IMapper mapper, IValidator<CreateDto> createDtovalidator, IValidator<UpdateDto> updateDtovalidator, IUow uow)
        {
            _mapper = mapper;
            _createDtovalidator = createDtovalidator;
            _updateDtovalidator = updateDtovalidator;
           
            _uow = uow;
        }

        public async Task<IResponse<CreateDto>> CreateAsync(CreateDto dto)
        {
            var result = _createDtovalidator.Validate(dto);
            if (result.IsValid)
            {
                var createdEntity = _mapper.Map<T>(dto);
               await _uow.GetRepositry<T>().CreateAsync(createdEntity);
                await _uow.SaveChangesAsync();
                return new Response<CreateDto>(ResponseType.Success, dto);
            }
            return new Response<CreateDto>(dto, result.ConvertToCustomValidationError());
        }

        public async Task<IResponse<List<ListDto>>> GetAllAsync()
        {
            var data=await _uow.GetRepositry<T>().GetAllAsync();
            var listDto = _mapper.Map<List<ListDto>>(data);
            return new Response<List<ListDto>>(ResponseType.Success, listDto);
            
        }

        public async Task<IResponse<IDto>> GetByIdAsync<IDto>(int id)
        {
            var data = await _uow.GetRepositry<T>().FindByFilterAsync(x => x.Id == id);
            if (data == null)
            {
                return new Response<IDto>(ResponseType.NotFound, "İlgili Veri Bulunamadı");
            }
            var dto = _mapper.Map<IDto>(data);
            return new Response<IDto>(ResponseType.Success, dto);
        }

        public async Task<IResponse> RemoveAsync(int id)
        {
            var data = await _uow.GetRepositry<T>().FindAsync(id);
            if (data == null)
            {
                return new Response(ResponseType.NotFound, "İlgili veri Bulunamadı");
            }
            _uow.GetRepositry<T>().Remove(data);
            await _uow.SaveChangesAsync();
            return new Response(ResponseType.Success);
        }

        public async  Task<IResponse<UpdateDto>> UpdateAsync(UpdateDto dto)
        {
            var result = _updateDtovalidator.Validate(dto);
            if (result.IsValid)
            {
                var unchangedData = await _uow.GetRepositry<T>().FindAsync(dto.Id);
                if (unchangedData == null)
                {
                    return new Response<UpdateDto>(ResponseType.NotFound, "İlgili Veri Bulunamadı");
                }
                var entity = _mapper.Map<T>(dto);
                _uow.GetRepositry<T>().Update(entity, unchangedData);
                await _uow.SaveChangesAsync();
                return new Response<UpdateDto>(ResponseType.Success, dto);


            }
            return new Response<UpdateDto>(dto, result.ConvertToCustomValidationError());
           

        }
    }
}
