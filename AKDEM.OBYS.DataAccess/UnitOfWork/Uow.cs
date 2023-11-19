using AKDEM.OBYS.DataAccess.Context;
using AKDEM.OBYS.DataAccess.Interfaces;
using AKDEM.OBYS.DataAccess.Repositories;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.DataAccess.UnitOfWork
{
    public class Uow:IUow
    {
        private readonly AkdemContext _akdemContext;

        public Uow(AkdemContext akdemContext)
        {
            _akdemContext = akdemContext;
        }


        public IGenericRepository<T> GetRepositry<T>() where T:BaseEntity
        {
            return new GenericRepository<T>(_akdemContext);
        }    
        public async Task SaveChangesAsync()
        {
            await _akdemContext.SaveChangesAsync();
        }
    }
}
