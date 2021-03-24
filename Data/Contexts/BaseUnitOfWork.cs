using Data.Contexts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public abstract class BaseUnitOfWork<TContext> : IBaseUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext context;
        private IsolationLevel UsedIsolationLevel = IsolationLevel.ReadCommitted;
        private IDbContextTransaction transaction;

        public BaseUnitOfWork(TContext context)
        {
            this.context = context;
        }

        public void BeginTransaction()
        {
            transaction = context.Database.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel level)
        {
            transaction = context.Database.BeginTransaction(level);
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void RollBackTransaction()
        {
            transaction.Rollback();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void CloseConnection()
        {
            if (context.Database.CanConnect())
                context.Database.CloseConnection();
        }

        public void OpenConnection()
        {
            if (!context.Database.CanConnect())
                context.Database.OpenConnection();
        }

        public IDbContextTransaction UseTransaction(DbTransaction transaction)
        {
            return context.Database.UseTransaction(transaction);
        }

        public string GenerateCreateScript()
        {
            return context.Database.GenerateCreateScript();
        }

        public DbConnection GetDbConnection()
        {
            return context.Database.GetDbConnection();
        }

        public void SetIsolationLevel(IsolationLevel level)
        {
            if (UsedIsolationLevel != level)
            {
                UsedIsolationLevel = level;

                var str = "SET TRANSACTION ISOLATION LEVEL ";
                switch (level)
                {
                    case IsolationLevel.ReadCommitted:
                        str += "READ COMMITTED";
                        break;
                    case IsolationLevel.ReadUncommitted:
                        str += "READ UNCOMMITTED";
                        break;
                    case IsolationLevel.RepeatableRead:
                        str += "REPEATABLE READ";
                        break;
                    case IsolationLevel.Serializable:
                        str += "SERIALIZABLE";
                        break;
                    case IsolationLevel.Snapshot:
                        str += "SNAPSHOT";
                        break;
                    default:
                        str += "READ COMMITTED";
                        break;
                }
                ExecuteSqlRaw(str);
            }
        }

        public int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            return context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public int ExecuteSqlRaw(string sql, IEnumerable<object> parameters)
        {
            return context.Database.ExecuteSqlRaw(sql, parameters);
        }

    }
}
