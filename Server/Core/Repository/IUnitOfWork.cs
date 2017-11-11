using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Server.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
        void Rollback();
    }
}