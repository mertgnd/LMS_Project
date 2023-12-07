using LMS_Project.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LMS_Project.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        protected static List<PropertyInfo> NavigationProperties = null;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        public List<PropertyInfo> GetNavigationProperties()
        {
            // Maintain a cache as reflection is expensive and make it static as models can't change at runtime
            if (NavigationProperties != null)
            {
                return NavigationProperties;
            }

            var elementType = DbContext.Model.FindEntityType(typeof(TEntity));
            NavigationProperties = elementType.GetNavigations().Select(x => x.PropertyInfo).ToList();
            return NavigationProperties;
        }

        public async Task<bool> ExistsAsync(object id)
        {
            return (await DbSet.FindAsync(id)) != null;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                DbSet.Add(entity);
                await DbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                string message = e.Message;
                throw new Exception(message, e);
            }

        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateManyAsync(IQueryable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            await DbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            DbSet.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public IQueryable<TEntity> FindWithPagination(IQueryable<TEntity> dbSet, int resultsPerPage, int pageNumber, out int totalPages, out int totalCount, out int resultCount, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var query = dbSet;
            totalCount = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            resultCount = query.Count();
            totalPages = (int)Math.Ceiling(((double)resultCount / resultsPerPage));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip((pageNumber - 1) * resultsPerPage).Take(resultsPerPage);
            return query;
        }

        public IQueryable<TEntity> Find(IQueryable<TEntity> dbSet, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        protected async Task<TEntity> FirstOrDefaultWithIncludesAsync(IQueryable<TEntity> dbSet, Expression<Func<TEntity, bool>> filter = null)
        {
            var query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        protected async Task<List<TEntity>> GetDataFromHistoryTableAsync(string rawSql)
        {
            return await DbSet.FromSqlRaw(rawSql).AsNoTracking().ToListAsync();
        }

        public Task ExecuteInTransactionAsync(Func<Task> action)
        {
            var strategy = DbContext.Database.CreateExecutionStrategy();

            return strategy.ExecuteAsync(async () =>
            {
                using (var transaction = DbContext.Database.BeginTransaction())
                {
                    await action();

                    transaction.Commit();
                }
            });
        }
    }
}