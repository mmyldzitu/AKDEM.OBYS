using AKDEM.OBYS.Dto.AppWarningDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class StudentWarningsModel
    {
        public List<AppWarningListDto> AppWarnings { get; set; }
        public bool Status  { get; set; }
        public string DepartReason { get; set; }
    }
}
