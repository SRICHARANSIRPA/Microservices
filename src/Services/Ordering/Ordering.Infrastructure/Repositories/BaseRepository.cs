using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistance;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly OrderContext _dbContext;
        protected DbSet<T> _entity;

        public BaseRepository(OrderContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _entity = _dbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            _entity.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async  Task DeleteAsync(T entity)
        {
            _entity.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _entity.FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entity.Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> Query = _entity;
            if (disableTracking) Query = Query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString)) Query = Query.Include(includeString);

            if (predicate != null) Query = Query.Where(predicate);

            if (orderBy != null)
                return await orderBy(Query).ToListAsync();
            return await Query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> Query = _entity;
            if (disableTracking) Query = Query.AsNoTracking();

            if (includes != null) Query = includes.Aggregate(Query, (current, include) => current.Include(include));

            if (predicate != null) Query = Query.Where(predicate);

            if (orderBy != null)
                return await orderBy(Query).ToListAsync();
            return await Query.ToListAsync();
        }

    }
}
