using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T:BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllAsync<TKey>(Expression<Func<T, TKey>> selector, OrderByType type = OrderByType.DESC);
        Task<List<T>> GetAllAsync<TKey>(Expression<Func<T, TKey>> selector, Expression<Func<T, bool>> filter, OrderByType type = OrderByType.DESC);
        Task<T> FindAsync(object id);
        Task<T> FindByFilterAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false);
        IQueryable<T> GetQuery();
        void Remove(T entity);
        Task CreateAsync(T entity);
        void Update(T newEntity, T unchanged);
    }
}
