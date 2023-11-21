using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppTeacherUpdateModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        
        public string PhoneNumber { get; set; }
        public bool Status { get; set; } = true;
        public IFormFile ImagePath { get; set; }
    }
}
