using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppStudentCreateModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int SıraNo { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; } = true;
        public IFormFile ImagePath { get; set; }
        
       
        
        public int BranchId { get; set; }
        public int ClassId { get; set; }
        public List<SelectListItem> Classes { get; set; }
        public List<SelectListItem> Branches { get; set; }

    }
}
