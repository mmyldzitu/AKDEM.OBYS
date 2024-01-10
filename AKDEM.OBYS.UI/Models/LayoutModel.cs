using AKDEM.OBYS.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AKDEM.OBYS.UI.Models
{
    public class LayoutModel
    {
        private readonly IAppSessionService _appSessionService;

        public LayoutModel(IAppSessionService appSessionService)
        {
            _appSessionService = appSessionService;
        }

        
     
        public int ActiveSessionId { get; set; }
        public async Task OnGet()
        {
            
            ActiveSessionId = await _appSessionService.GetActiveSessionId();
        }
    }
}
