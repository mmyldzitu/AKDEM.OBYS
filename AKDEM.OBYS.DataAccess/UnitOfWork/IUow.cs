using AKDEM.OBYS.DataAccess.Interfaces;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.DataAccess.UnitOfWork
{
    public interface IUow
    {
        IGenericRepository<T> GetRepositry<T>() where T : BaseEntity;
        Task SaveChangesAsync();
    }
}
