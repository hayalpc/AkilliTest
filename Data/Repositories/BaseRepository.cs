using Data.Contexts;
using Data.Models.Interfaces;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public static class HpRepositoryExtension
    {
        public static IQueryable<Tentity> AsNoTracking<Tentity>(this IQueryable<Tentity> tentities) where Tentity : class
        {
            return tentities.AsNoTracking();
        }
    }

    public abstract class BaseRepository<Tentity> : IBaseRepository<Tentity>
        where Tentity : class, IBaseModel
    {
        protected readonly BaseDbContext context;

        public BaseRepository(BaseDbContext context)
        {
            this.context = context;
        }

        public DbSet<Tentity> GetContext()
        {
            return GetContext<Tentity>();
        }

        public DbSet<TOEntity> GetContext<TOEntity>() where TOEntity : class, IBaseModel
        {
            return context.Set<TOEntity>();
        }

        public IQueryable<Tentity> GetQuery(Expression<Func<Tentity, bool>> predicate = null)
        {
            return GetQuery<Tentity>(predicate);
        }

        public IQueryable<TOEntity> GetQuery<TOEntity>(Expression<Func<TOEntity, bool>> predicate = null) where TOEntity : class, IBaseModel
        {
            if (predicate == null)
                return context.Set<TOEntity>().Where(x => 1 == 1);
            return context.Set<TOEntity>().Where(predicate);
        }

        public Tentity Get(Expression<Func<Tentity, bool>> predicate = null)
        {
            return Get<Tentity>(predicate);
        }

        public TOEntity Get<TOEntity>(Expression<Func<TOEntity, bool>> predicate = null) where TOEntity : class, IBaseModel
        {
            if (predicate == null)
                return context.Set<TOEntity>().Where(x => 1 == 1).AsNoTracking().FirstOrDefault();
            return context.Set<TOEntity>().Where(predicate).AsNoTracking().FirstOrDefault();
        }

        public Tentity GetById(long Id)
        {
            return GetById(Id, false);
        }

        public Tentity GetById(long Id, bool AsNotracking)
        {
            return GetById<Tentity>(Id, AsNotracking);
        }

        public TOEntity GetById<TOEntity>(long Id) where TOEntity : class, IBaseModel
        {
            return GetById<TOEntity>(Id, false);
        }

        public TOEntity GetById<TOEntity>(long Id, bool AsNotracking) where TOEntity : class, IBaseModel
        {
            var entity = context.Set<TOEntity>().Find(Id);
            if (entity != null && AsNotracking)
                context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public void Insert(Tentity entity)
        {
            //context.Set<Tentity>().Add(entity);
            //context.Add(entity);
            Insert<Tentity>(entity);
        }

        public void Insert<TOEntity>(TOEntity entity) where TOEntity : class, IBaseModel
        {
            //context.Set<TOEntity>().Add(entity);
            context.Add(entity);
        }

        public void Update(Tentity entity)
        {
            //context.Entry(entity).State = EntityState.Modified;
            //context.Set<Tentity>().Update(entity);
            //context.Update(entity);
            Update<Tentity>(entity);
        }

        public void Update<TOEntity>(TOEntity entity) where TOEntity : class, IBaseModel
        {
            context.Entry(entity).State = EntityState.Modified;
            //context.Set<TOEntity>().Update(entity);
            context.Update(entity);
        }

        public void Update(Tentity entity, params string[] fields)
        {
            var attack = context.Attach(entity);
            foreach (var field in fields)
            {
                if (entity.GetType().GetProperty(field) != null)//geçersiz bir 
                {
                    attack.Property(field).IsModified = true;
                }
                else
                {
                    throw new Exception($"{field} is not a property of " + entity.GetType().Name);
                }
            }
        }

        public void Update<TOEntity>(TOEntity entity, params string[] fields) where TOEntity : class, IBaseModel
        {
            var attack = context.Attach(entity);
            foreach (var field in fields)
            {
                if (entity.GetType().GetProperty(field) != null)//geçersiz bir 
                {
                    attack.Property(field).IsModified = true;
                }
                else
                {
                    throw new Exception($"{field} is not a property of " + entity.GetType().Name);
                }
            }
        }

        public void Delete(Tentity entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
            //context.Set<Tentity>().Remove(entity);
            context.Remove(entity);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void InsertRange(IEnumerable<Tentity> entities)
        {
            //context.Set<Tentity>().AddRange(entities);
            //context.AddRange(entities);
            InsertRange<Tentity>(entities);
        }

        public void InsertRange<TOEntity>(IEnumerable<TOEntity> entities) where TOEntity : class, IBaseModel
        {
            context.Set<TOEntity>().AddRange(entities);
            //context.AddRange(entities);
        }

        public void UpdateRange(IEnumerable<Tentity> entities)
        {
            context.Entry(entities).State = EntityState.Modified;
            ////context.Set<Tentity>().UpdateRange(entities);
            //context.UpdateRange(entities);
        }

        public void DeleteRange(IEnumerable<Tentity> entities)
        {
            context.Entry(entities).State = EntityState.Deleted;
            //context.Set<Tentity>().RemoveRange(entities);
            context.RemoveRange(entities);
        }

    }
}
