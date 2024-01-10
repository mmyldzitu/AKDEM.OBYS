using AKDEM.OBYS.Business.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.TagHelpers
{
    [HtmlTargetElement("getPassiveStudentInfo")]
    public class GetPassiveStudentInfo:TagHelper
    {
        private readonly IAppUserSessionService _appUserSessionService;
        public int UserId { get; set; }
        public GetPassiveStudentInfo(IAppUserSessionService appUserSessionService)
        {
            _appUserSessionService = appUserSessionService;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = "";
            var session = await _appUserSessionService.ReturnPassiveDateByUserId(UserId);
            html += session;
            output.Content.SetHtmlContent(html);
        }
    }
}
