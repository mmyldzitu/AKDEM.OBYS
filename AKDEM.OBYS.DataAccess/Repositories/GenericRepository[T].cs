using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.DataAccess.Context;
using AKDEM.OBYS.DataAccess.Interfaces;
using AKDEM.OBYS.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.DataAccess.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T> where T:BaseEntity
    {
        private readonly AkdemContext _akdemContext;

        public GenericRepository(AkdemContext akdemContext)
        {
            _akdemContext = akdemContext;
        }

        //İlgili entitye dair herhangi bir filtre kullanmadan tüm verileri getirme
        public async Task<List<T>> GetAllAsync()
        {
            return await _akdemContext.Set<T>().AsNoTracking().ToListAsync();
        }
        //İlgili enetyde herhangi bir filtre kullanarak istediğimiz listeyi döndürmek için
        public async Task<List<T>> GetAllAsync(Expression<Func<T,bool>> filter)
        {
            return await _akdemContext.Set<T>().Where(filter).AsNoTracking().ToListAsync();
        }

        //İlgili entityde bir sıralama yaparak gerekli listeyi döndürme
        public async Task<List<T>> GetAllAsync<TKey>(Expression<Func<T,TKey>> selector, OrderByType type=OrderByType.DESC)
        {
            return type == OrderByType.ASC ? await _akdemContext.Set<T>().AsNoTracking().OrderBy(selector).ToListAsync() : await _akdemContext.Set<T>().AsNoTracking().OrderByDescending(selector).ToListAsync();
        }

        //Hem filtre ile hem sıralama ile ilgili entity gerekli listeyi döndürme
        public async Task<List<T>> GetAllAsync<TKey>(Expression<Func<T,TKey>> selector, Expression<Func<T,bool>> filter, OrderByType type= OrderByType.DESC)
        {
            return type == OrderByType.ASC ? await _akdemContext.Set<T>().Where(filter).AsNoTracking().OrderBy(selector).ToListAsync() :
                await _akdemContext.Set<T>().Where(filter).AsNoTracking().OrderByDescending(selector).ToListAsync();
        }

        //İlgili entity içerisinden verilecek id ye göre bir adet veri döndürmek için 
        public async Task<T> FindAsync(object id)
        {
            return await _akdemContext.Set<T>().FindAsync(id);
        }

        //İlgili entity içerisinden verilecek herhangi bir filtreye göre bir adet veri döndermek için
        public async Task<T> FindByFilterAsync(Expression<Func<T,bool>> filter, bool asNoTracking = false)
        {
            return !asNoTracking ? await _akdemContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(filter) : await
                _akdemContext.Set<T>().SingleOrDefaultAsync(filter);
        }
        public IQueryable<T> GetQuery()
        {
            return _akdemContext.Set<T>().AsQueryable();
        }

        public void Remove(T entity)
        {
            _akdemContext.Set<T>().Remove(entity);
        }

        public  async Task CreateAsync( T entity)
        {
            await _akdemContext.Set<T>().AddAsync(entity);
        }
        public void Update(T newEntity, T unchanged)
        {
            _akdemContext.Entry(unchanged).CurrentValues.SetValues(newEntity);
        }
    }
}
