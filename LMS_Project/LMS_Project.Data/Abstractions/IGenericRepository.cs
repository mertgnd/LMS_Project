using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LMS_Project.Data.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<bool> ExistsAsync(object id);
        Task<TEntity> AddAsync(TEntity entity);
        Task DeleteAsync(object id);
        IQueryable<TEntity> Find(IQueryable<TEntity> dbSet, Expression<Func<TEntity, bool>> filter = null);
        IQueryable<TEntity> FindWithPagination(IQueryable<TEntity> dbSet, int resultsPerPage, int pageNumber, out int totalPages, out int totalCount, out int resultCount, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(object id);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task UpdateManyAsync(IQueryable<TEntity> entities);
        Task<int> CountAsync();
        List<PropertyInfo> GetNavigationProperties();
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}