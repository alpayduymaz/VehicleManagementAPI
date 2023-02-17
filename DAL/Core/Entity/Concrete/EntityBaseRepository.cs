using DAL.Core.Entity.Abstract;
using Entity.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Core.Entity.Concrete
{
    public class EntityBaseRepository<T> : HttpContextAccessor, IEntityBaseRepository<T> where T : class, new()
    {
        private readonly DbContext _context;

        public EntityBaseRepository(DbContext context)
        {
            _context = context;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query;
        }

        public virtual int Count()
        {
            return _context.Set<T>().Count();
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.AsQueryable();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(predicate).FirstOrDefault();
        }

        public void DetachEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.Where(predicate);
        }
        /*
        public virtual IQueryable<T> FromSqlRaw(string query, params Expression<Func<T, object>>[] parameters)
        {
            return _context.Set<T>().FromSql(query, parameters);
        }

        public int ExecuteSqlRaw(string query, params Expression<Func<T, object>>[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(query, parameters);
        }
        */
        public bool IsHaveForeign(T entity)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    Delete(entity);
                    Commit();
                    transaction.Rollback();
                    return false;
                }
                catch { return true; }
            }
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entityList)
        {
            _context.Set<T>().AddRange(entityList);
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entityList)
        {
            return _context.Set<T>().AddRangeAsync(entityList);
        }

        public virtual void Update(T entity)
        {
            _context.Update<T>(entity);
        }

        public virtual void Update(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            foreach (var property in updatedProperties)
            {
                _context.Attach<T>(entity).Property(property).IsModified = true;
            }
        }

        public virtual void Delete(T entity)
        {
            if (entity is BaseEntity baseLog)
            {
                int.TryParse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value, out int id);
                int? userId = (id == 0 ? (int?)null : id);
                baseLog.DataStatus = DataStatus.Deleted;
                baseLog.LastUpdatedAt = DateTime.Now;
                baseLog.LastUpdatedUserId = userId;

                _context.Update<T>(entity);
            }
            else
            {
                _context.Remove<T>(entity);
            }
            
        }

        public virtual void DeletePermanently(T entity)
        {
            _context.Remove<T>(entity);
        }

        public virtual void DeletePermanentlyWhere(Expression<Func<T, bool>> predicate)
        {
            foreach (var entity in _context.Set<T>().Where(predicate))
            {
                _context.Remove<T>(entity);
            }
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            foreach (var entity in _context.Set<T>().Where(predicate))
            {
                if (entity is BaseEntity baseLog)
                {
                    int.TryParse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value, out int id);
                    int? userId = (id == 0 ? (int?)null : id);
                    baseLog.DataStatus = DataStatus.Deleted;
                    baseLog.LastUpdatedAt = DateTime.Now;
                    baseLog.LastUpdatedUserId = userId;

                    _context.Update<T>(entity);
                }
                else
                {
                    _context.Remove<T>(entity);
                }
            }
        }

        public virtual void Commit()
        {
            try
            {
                OnBeforeSaving();
                _context.SaveChanges();
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public virtual Task<int> CommitAsync()
        {
            try
            {

                OnBeforeSaving();

                return _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private void OnBeforeSaving()
        {
            if (HttpContext != null)
            {
                DateTime now = DateTime.Now;
                int.TryParse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value, out int id);
                int? userId = (id == 0 ? (int?)null : id);
                foreach (var entry in _context.ChangeTracker.Entries().Where(a => a.Entity is BaseEntity))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                        case EntityState.Detached:
                            ((BaseEntity)entry.Entity).CreatedAt = now;
                            ((BaseEntity)entry.Entity).CreatedUserId = userId;
                            if (((BaseEntity)entry.Entity).DataStatus == 0)
                                ((BaseEntity)entry.Entity).DataStatus = DataStatus.Activated;

                            break;

                        case EntityState.Modified:
                        case EntityState.Deleted:
                            ((BaseEntity)entry.Entity).LastUpdatedAt = now;
                            ((BaseEntity)entry.Entity).LastUpdatedUserId = userId;
                            break;
                    }
                }
            }
        }

        public void BulkInsert(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T>().AddRange(entityList);
                    Commit();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task BulkInsertAsync(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.Set<T>().AddRangeAsync(entityList);
                    await CommitAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public void BulkUpdate(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T>().UpdateRange(entityList);
                    Commit();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task BulkUpdateAsync(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T>().UpdateRange(entityList);
                    await CommitAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public void BulkInsertOrUpdate(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in entityList)
                    {
                        var entry = _context.Entry(item);
                        switch (entry.State)
                        {
                            case EntityState.Detached:
                                if (entry.IsKeySet)
                                    _context.Update(item);
                                else
                                    _context.Add(item);
                                break;

                            case EntityState.Modified:
                                _context.Update(item);
                                break;

                            case EntityState.Added:
                                _context.Add(item);
                                break;

                            case EntityState.Unchanged:
                                //item already in db no need to do anything
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    Commit();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task BulkInsertOrUpdateAsync(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in entityList)
                    {
                        var entry = _context.Entry(item);
                        switch (entry.State)
                        {
                            case EntityState.Detached:
                                if (entry.IsKeySet)
                                    _context.Update(item);
                                else
                                    _context.Add(item);
                                break;

                            case EntityState.Modified:
                                _context.Update(item);
                                break;

                            case EntityState.Added:
                                _context.Add(item);
                                break;

                            case EntityState.Unchanged:
                                //item already in db no need to do anything
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    await CommitAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public void BulkDelete(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T>().RemoveRange(entityList);
                    Commit();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task BulkDeleteAsync(IList<T> entityList)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T>().RemoveRange(entityList);
                    await CommitAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }


    }
}
