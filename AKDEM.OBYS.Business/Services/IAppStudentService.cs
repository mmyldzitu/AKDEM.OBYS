using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppStudentService : IGenericService<AppStudentCreateDto, AppStudentUpdateDto, AppStudentListDto, AppUser>
    {
        
        Task<List<AppStudentListDto>> GetAllStudentAsync(RoleType type);

        Task<IResponse<AppStudentCreateDto>> CreateStudentWithRoleAsync(AppStudentCreateDto dto, int roleId);
        Task<List<AppStudentListDto>> GetStudentsWithBranchAsync(int id);
        
        Task<AppStudentListDto> GetStudentById(int id);
        Task ChangeStudentStatusBecasuseOfWarning(int userId, double sessionWarningCount, double totalWarningCount);
        Task<List<AppStudentListDto>> GetPassiveStudents(int sessionId);
        Task CreateStudentOrChangeStatusProcessForUserSessionandUSerSessionLessons(int userId);
        Task<List<AppStudentListDto>> GetStudentsWithBranchAndSessionAsync(int branchId,int sessionId);
        Task<List<AppStudentListDto>> GetStudentsByClassId(RoleType type, int classId);
        Task ChangeStudentBranch(int studentId, int branchId,int classId);
        Task<List<AppStudentListDto>> GraduatedStudenstBySessionId(int sessionId);
        Task<List<AppStudentListDto>> GetStudentsWithBranchAndSessionAndLessonAsync(int branchId, int sessionId, int lessonId);
        Task<string> ReturnUserImg(int userId);
        Task ControlUserSessionWhenUpdating(int userId, int sessionId);
        Task ChangeBranchForStudent(int userId, int newBranchId);
        Task<bool> ReturnStatusOfStudent(int userId);
        Task<string> ReturnDepartReasonOfStudent(int userId);
        Task RemoveStudent(int userId);


    }
}
