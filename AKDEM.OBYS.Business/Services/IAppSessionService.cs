using AKDEM.OBYS.Common;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
   public interface IAppSessionService:IGenericService<AppSessionCreateDto,AppSessionUpdateDto,AppSessionListDto,AppSession>
    {
        Task SetStatusAsync(int sessionId);
        Task<IResponse<List<AppSessionListDto>>> GetOrderingAsync();
    }
}
