using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Data.Contexts.Interfaces
{
    public interface IBaseUnitOfWork<TContext> : IDisposable
    {
        void BeginTransaction();

        void BeginTransaction(IsolationLevel level);

        void CommitTransaction();

        void RollBackTransaction();

        IDbContextTransaction UseTransaction(DbTransaction transaction);

        void SetIsolationLevel(IsolationLevel level);

        int ExecuteSqlRaw(string sql, params object[] parameters);
        int ExecuteSqlRaw(string sql, IEnumerable<object> parameters);

        string GenerateCreateScript();

        int SaveChanges();

        Task<int> SaveChangesAsync();

        DbConnection GetDbConnection();
        void OpenConnection();
        void CloseConnection();
    }
}
