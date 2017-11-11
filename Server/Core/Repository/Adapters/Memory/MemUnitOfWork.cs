using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Repository.Adapters
{
    public class MemUnitOfWork : IUnitOfWork
    {
        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {

        }

        public void Commit()
        {
     
        }

        public void Rollback()
        {
        
        }

        public void Dispose()
        {
        
        }
    }
}
