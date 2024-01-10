using AKDEM.OBYS.Dto.AppBranchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppBranchStudentListModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Sınıf { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public int SıraNo { get; set; }

        public double TotalAverage { get; set; }
        
        public int BranchDegree { get; set; } = 0;
        
        public int TotalDegree { get; set; } = 0;
        

        public double WarningCount { get; set; } = 0;
        public int StudentId { get; set; }
        
    }
}
