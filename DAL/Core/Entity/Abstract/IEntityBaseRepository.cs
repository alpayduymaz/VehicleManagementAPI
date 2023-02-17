using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core.Entity.Abstract
{
    public interface IEntityBaseRepository<T> where T : class, new()
    {
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> GetAll();

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);

        int Count();

        T GetSingle(Expression<Func<T, bool>> predicate);

        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void DetachEntity(T entity);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        //IQueryable<T> FromSqlRaw(string query, params Expression<Func<T, object>>[] parameters); sonradan düzelt

        IDbContextTransaction BeginTransaction();

       // int ExecuteSqlRaw(string query, params Expression<Func<T, object>>[] parameters); //sonradan düzelt

        bool IsHaveForeign(T entity);

        void Add(T entity);

        void AddRange(IEnumerable<T> entityList);

        Task AddRangeAsync(IEnumerable<T> entityList);

        void Update(T entity);

        void Update(T entity, params Expression<Func<T, object>>[] updatedProperties);

        void Delete(T entity);

        void DeletePermanently(T entity);

        void DeleteWhere(Expression<Func<T, bool>> predicate);

        void DeletePermanentlyWhere(Expression<Func<T, bool>> predicate);

        void Commit();

        Task<int> CommitAsync();

        void BulkInsert(IList<T> entityList);

        Task BulkInsertAsync(IList<T> entityList);

        void BulkUpdate(IList<T> entityList);

        Task BulkUpdateAsync(IList<T> entityList);

        void BulkInsertOrUpdate(IList<T> entityList);

        Task BulkInsertOrUpdateAsync(IList<T> entityList);

        void BulkDelete(IList<T> entityList);

        Task BulkDeleteAsync(IList<T> entityList);
    }
}
