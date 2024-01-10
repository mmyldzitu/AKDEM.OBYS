using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class GraduatedStudentsModel
    {
        public string Name { get; set; }
        public int SıraNo { get; set; }
        public string Branch { get; set; }
        public string GradDate { get; set; }
        public double Average { get; set; }
        public int BranchDegree { get; set; }
        public int ClassDegree { get; set; }
    }
}
