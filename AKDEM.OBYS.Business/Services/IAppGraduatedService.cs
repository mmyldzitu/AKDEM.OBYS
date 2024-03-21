using AKDEM.OBYS.Dto.AppGraduatedDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppGraduatedService:IGenericService<AppGraduatedCreateDto,AppGraduatedUpdateDto,AppGraduatedListDto,AppGraduated>
    {
        Task GraduateStudents(int userId, int sessionId, int order);
        Task<AppGraduatedListDto> CertificaofUser(int userId);
        Task RemoveCertificateByUserId(int userId);
        Task<List<AppGraduatedListDto>> GetGradutedStudentsDeveloper();
    }
}
