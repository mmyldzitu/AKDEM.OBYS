using AKDEM.OBYS.Dto.AppWarningDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppWarningService : IGenericService<AppWarningCreateDto, AppWarningUpdateDto, AppWarningListDto, AppWarning>
    {
    }
}
