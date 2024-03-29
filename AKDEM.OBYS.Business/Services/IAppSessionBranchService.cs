﻿using AKDEM.OBYS.Dto.AppSessionBranchDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppSessionBranchService : IGenericService<AppSessionBranchCreateDto, AppSessionBranchUpdateDto, AppSessionBranchListDto, AppSessionBranch>
    {
        Task CreateSessionBranchAsync(List<AppSessionBranchCreateDto> dtos);
        Task RemoveSessionBranchesBySessionId(int sessionId);
    }
}
