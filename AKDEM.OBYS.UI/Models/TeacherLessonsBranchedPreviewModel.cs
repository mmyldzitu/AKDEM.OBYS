using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class TeacherLessonsBranchedPreviewModel
    {
        public int BranchId { get; set; }
        public int LessonId { get; set; }
        public string BranchDefinition { get; set; }
        public string LessonDefinition { get; set; }
    }
}
