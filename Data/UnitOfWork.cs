using Data.Context;
using Domain.Interfaces.Data;
using System;
using System.Data.Entity;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ModelContext _context;
        private bool _disposed = false;
        private DbContextTransaction _transaction;
        public UnitOfWork(ModelContext context)
        {
            _context = context;
        }
        public void BeginTransaction()
        {
            if (!_disposed)
                this._transaction = _context.Database.BeginTransaction();
            _disposed = false;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Commit(bool dispose = true)
        {
            if (_transaction != null)
                _transaction.Commit();
            _transaction = null;
            if (dispose)
                Dispose();
        }

        public void Rollback(bool dispose = true)
        {
            if (_transaction != null)
                _transaction.Rollback();
            _transaction = null;
            if (dispose)
                Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
