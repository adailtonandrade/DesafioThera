using System;
using Domain.Interfaces.Repositories;
using Data.Context;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;

namespace Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ModelContext _context;
        protected DbSet<T> _dbSet;

        public GenericRepository(ModelContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T obj)
        {
            try
            {
                if (obj == null)
                    throw new NullReferenceException("O Objeto a ser cadastrado não pode ser nulo");
                _dbSet.Add(obj);
            }
            catch (SqlException e)
            {
                e.Message.ToString();
                throw new Exception("Não foi possível realizar a transação na base de dados, contate o administrador do sistema ou tente novamente mais tarde", e);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAllNoTracking()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T GetByIdNoTracking(int id)
        {
            var obj = _dbSet.Find(id);
            if (obj != null)
                _context.Entry(obj).State = EntityState.Detached;
            return obj;
        }

        public void Update(T obj)
        {
            try
            {
                if (obj == null)
                    throw new Exception("O Objeto a ser atualizado não pode ser nulo");
                _context.Entry(obj).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                e.Message.ToString();
                throw new Exception("Não foi possível realizar a transação na base de dados, contate o administrador do sistema ou tente novamente mais tarde", e);
            }
        }

    }
}
