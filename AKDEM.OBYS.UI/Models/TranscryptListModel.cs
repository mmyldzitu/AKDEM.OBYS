using AKDEM.OBYS.Dto.AppUserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class TranscryptListModel
    {
        public AppStudentListDto AppStudent { get; set; }
        public List<StudentDetailsModel> StudentDetails { get; set; }
    }
}
