using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ScheduleAssistant.Data.EntityFramework;
using ScheduleAssistant.Data.Exceptions;
using ScheduleAssistant.Domain;
using Microsoft.EntityFrameworkCore;

namespace ScheduleAssistant.Data.Repository
{
    internal class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected AppDbContext context;

        public BaseRepository(AppDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => this.context.Set<TEntity>();

        // ToDo: Move to UnitOfWork
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        // ToDo: Move to UnitOfWork
        public async Task SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return this.GetAll();
        }

        public virtual List<TEntity> GetAllList()
        {
            return this.GetAll().ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(this.GetAllList());
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetAll().Where(predicate).ToList();
        }

        public virtual List<TProjection> GetAllList<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> projection)
        {
            return this.GetAll().Where(predicate).Select(projection).ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(this.GetAllList(predicate));
        }

        public virtual Task<List<TProjection>> GetAllListAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> projection)
        {
            return Task.FromResult(this.GetAllList(predicate, projection));
        }

        public virtual T Query<T>(Func<IQueryable<TEntity>, T> queryMethod)
        {
            return queryMethod(this.GetAll());
        }

        public virtual TEntity Get(int id)
        {
            var entity = this.FirstOrDefault(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            var entity = await this.FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetAll().Single(predicate);
        }

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(this.Single(predicate));
        }

        public virtual TEntity FirstOrDefault(int id)
        {
            return this.GetAll().FirstOrDefault(x => x.Id == id);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(int id)
        {
            return Task.FromResult(this.FirstOrDefault(id));
        }

        public virtual TEntity FirstOrDefault()
        {
            return this.GetAll().FirstOrDefault();
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetAll().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(this.FirstOrDefault(predicate));
        }

        public virtual TEntity Load(int id)
        {
            return this.Get(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            return this.Table.Add(entity).Entity;
        }

        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(this.Insert(entity));
        }

        public virtual int InsertAndGetId(TEntity entity)
        {
            return this.Insert(entity).Id;
        }

        public virtual Task<int> InsertAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(this.InsertAndGetId(entity));
        }

        public TEntity Update(TEntity entity)
        {
            this.AttachIfNot(entity);
            this.context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(this.Update(entity));
        }

        public virtual TEntity Update(int id, Action<TEntity> updateAction)
        {
            var entity = this.Get(id);
            updateAction(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(int id, Func<TEntity, Task> updateAction)
        {
            var entity = await this.GetAsync(id);
            await updateAction(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            this.AttachIfNot(entity);
            this.Table.Remove(entity);
        }

        public virtual Task DeleteAsync(TEntity entity)
        {
            this.Delete(entity);
            return Task.FromResult(0);
        }

        public virtual void Delete(int id)
        {
            var entity = this.Table.Local.FirstOrDefault(ent => ent.Id == id);
            if (entity == null)
            {
                entity = this.FirstOrDefault(id);
                if (entity == null)
                {
                    return;
                }
            }

            Delete(entity);
        }

        public virtual Task DeleteAsync(int id)
        {
            this.Delete(id);
            return Task.FromResult(0);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in this.GetAll().Where(predicate).ToList())
            {
                this.Delete(entity);
            }
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            this.Delete(predicate);
            return Task.FromResult(0);
        }

        public virtual int Count()
        {
            return this.GetAll().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(this.Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetAll().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(this.Count(predicate));
        }

        public virtual long LongCount()
        {
            return this.GetAll().LongCount();
        }

        public virtual Task<long> LongCountAsync()
        {
            return Task.FromResult(this.LongCount());
        }

        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return this.GetAll().Where(predicate).LongCount();
        }

        public virtual Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(this.LongCount(predicate));
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!this.context.Set<TEntity>().Local.Contains(entity))
            {
                this.context.Set<TEntity>().Attach(entity);
            }
        }
    }
}