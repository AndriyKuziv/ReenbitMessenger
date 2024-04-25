using Microsoft.AspNetCore.Identity;
using ReenbitMessenger.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using LinqKit;

namespace ReenbitMessenger.DataAccess.Repositories
{
    public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId> where TEntity : class
    {
        protected readonly MessengerDataContext _dbContext;
        public GenericRepository(MessengerDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return _dbContext.Set<TEntity>().AsQueryable().AsEnumerable();
        }

        public async Task<TEntity> GetAsync(TId id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _dbContext.Set<TEntity>().AddAsync(entity);

            return result.Entity;
        }

        public async Task<TEntity> DeleteAsync(TId id)
        {
            var entity = await _dbContext.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TId id, TEntity entity)
        {
            var existingEntity = await _dbContext.Set<TEntity>().FindAsync(id);

            if (existingEntity is null)
            {
                return null;
            }

            existingEntity = entity;

            return existingEntity;
        }

        public async Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate,
            string orderBy = "", bool ascending = true, int startAt = 0, int take = 20)
        {
            var orderByProp = typeof(TEntity).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, orderBy,
                StringComparison.OrdinalIgnoreCase));
            if (orderByProp is null)
            {
                orderByProp = typeof(TEntity).GetProperty("Id");
            }

            var query = _dbContext.Set<TEntity>().AsQueryable();

            var props = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var sortedList = ascending ?
                query.Where(predicate).OrderBy(item => orderByProp.GetValue(item)) :
                    query.Where(predicate).OrderByDescending(item => orderByProp.GetValue(item));

            if (take <= 0)
            {
                return sortedList.AsEnumerable();
            }

            return sortedList.Skip(startAt).Take(take);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(string searchValue,
            string orderBy = "", bool ascending = true, int startAt = 0, int take = 20)
        {
            var orderByProp = typeof(TEntity).GetProperties().FirstOrDefault(prop => string.Equals(prop.Name, orderBy,
                StringComparison.OrdinalIgnoreCase));
            if (orderByProp is null)
            {
                orderByProp = typeof(TEntity).GetProperty("Id");
            }

            var query = _dbContext.Set<TEntity>().AsQueryable();

            var props = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Expression<Func<TEntity, bool>> combinedExpression = null;
            foreach (var prop in props)
            {
                Expression<Func<TEntity, bool>> expr = item => Convert.ToString(prop.GetValue(item)).Contains(searchValue);

                if (combinedExpression is null)
                {
                    combinedExpression = expr;
                }
                else
                {
                    combinedExpression = CombineExpressions(combinedExpression, expr);
                }
            }

            var sortedList = ascending ?
                query.AsExpandable().Where(combinedExpression.Compile()).OrderBy(item => orderByProp.GetValue(item)) :
                    query.AsExpandable().Where(combinedExpression.Compile()).OrderByDescending(item => orderByProp.GetValue(item));

            if (take <= 0)
            {
                return sortedList.AsEnumerable();
            }

            return sortedList.Skip(startAt).Take(take);
        }

        protected Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> firstExpr,
            Expression<Func<T, bool>> secondExpr)
        {
            var invokedExpr = Expression.Invoke(secondExpr, firstExpr.Parameters);
            var resultExpr = Expression.OrElse(firstExpr.Body, invokedExpr);
            return Expression.Lambda<Func<T, bool>>(resultExpr, firstExpr.Parameters);
        }
    }
}
