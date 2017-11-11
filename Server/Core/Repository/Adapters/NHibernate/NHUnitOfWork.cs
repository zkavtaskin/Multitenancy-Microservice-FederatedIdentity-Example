using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NHibernate.Cfg;
using Server.Service;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class NHUnitOfWork : IUnitOfWork
    {
        private ISession session;
        private ITransaction transaction;
        public ISession Session { get { return this.session; } }
        private NHConfiguration nhConfiguration;

        public NHUnitOfWork(NHConfiguration nhConfiguration)
        {
            this.nhConfiguration = nhConfiguration;
            this.openSession();
        }

        private void openSession()
        {
            if (this.session == null || !this.session.IsConnected)
            {
                if (this.session != null)
                    this.session.Dispose();

                this.session = this.nhConfiguration.Config.BuildSessionFactory().OpenSession();
            }
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (this.transaction == null || !this.transaction.IsActive)
            {
                if (this.transaction != null)
                    this.transaction.Dispose();

                this.transaction = this.session.BeginTransaction(isolationLevel);
            }
        }

        public void Commit()
        {
            try
            {
                this.transaction.Commit();
            }
            catch
            {
                this.transaction.Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            this.transaction.Rollback();
        }

        public void Dispose()
        {
            if (this.transaction != null)
            {
                this.transaction.Dispose();
                this.transaction = null;
            }

            if (this.session != null)
            {
                this.session.Close();
                this.session.Dispose();
                session = null;
            }
        }
    }
}