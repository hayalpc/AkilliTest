using Data.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IBaseRepository<Tentity> 
        where Tentity :class, IBaseModel 
    {
        DbSet<Tentity> GetContext();
        DbSet<TEntity2> GetContext<TEntity2>() where TEntity2 : class, IBaseModel;
        IQueryable<Tentity> GetQuery(Expression<Func<Tentity, bool>> predicate = null);
        IQueryable<TOEntity> GetQuery<TOEntity>(Expression<Func<TOEntity, bool>> predicate = null) where TOEntity : class, IBaseModel;
        Tentity Get(Expression<Func<Tentity, bool>> predicate = null);
        TOEntity Get<TOEntity>(Expression<Func<TOEntity, bool>> predicate = null) where TOEntity : class, IBaseModel;
        Tentity GetById(long Id);
        TOEntity GetById<TOEntity>(long Id) where TOEntity : class, IBaseModel;
        Tentity GetById(long Id,bool AsNoTracking);
        TOEntity GetById<TOEntity>(long Id,bool AsNoTracking) where TOEntity : class, IBaseModel;

        int SaveChanges();
        Task<int> SaveChangesAsync();

        void Insert(Tentity entity);
        void Insert<TOEntity>(TOEntity entity) where TOEntity : class, IBaseModel;

        void InsertRange(IEnumerable<Tentity> entities);
        void InsertRange<TOEntity>(IEnumerable<TOEntity> entities) where TOEntity : class, IBaseModel;
        void Update(Tentity entity);
        void Update<TOEntity>(TOEntity entity) where TOEntity : class, IBaseModel;
        void Update<TOEntity>(TOEntity entity, params string[] fields) where TOEntity : class, IBaseModel;
        void Update(Tentity entity, params string[] fields);
        void UpdateRange(IEnumerable<Tentity> entities);
        void Delete(Tentity entity);
        void DeleteRange(IEnumerable<Tentity> entities);
    }
}
