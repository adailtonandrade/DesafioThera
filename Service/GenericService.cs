using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Service
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null, string includeProperties = "")
        {

            return _repository.Get(filter, orderBy != null ? orderBy.Compile() : null, includeProperties);
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(int id)
        {
            return _repository.GetById(id);
        }

        public T GetByIdNoTracking(int id)
        {
            return _repository.GetByIdNoTracking(id);
        }
        public IEnumerable<T> GetAllNoTracking()
        {
            return _repository.GetAllNoTracking();
        }
        public void Insert(T obj)
        {
            _repository.Insert(obj);
        }
        public void Update(T obj)
        {
            _repository.Update(obj);
        }

        public virtual List<string> Validate(T variable)
        {
            return new List<string>();
        }

        public bool IsUniqueField(string propertyName, string name, int? id)
        {
            var domains = Expression.Parameter(typeof(T));
            Expression filter = null;

            name = name.Trim();

            if (id == null || id == 0)
            {
                if (!name.Equals(string.Empty))
                {
                    filter = Expression.Equal(Expression.Property(domains, propertyName.ToString().ToLower()), Expression.Constant(name.ToLower()));
                }
            }
            else
            {
                if (!name.Equals(string.Empty))
                {
                    var arg1 = Expression.Equal(Expression.Property(domains, propertyName.ToString().ToLower()), Expression.Constant(name.ToLower()));
                    var arg2 = Expression.NotEqual(Expression.Property(domains, "Id"), Expression.Constant(id));
                    filter = Expression.And(arg1, arg2);
                }
            }

            var filterExpression = Expression.Lambda<Func<T, bool>>(filter, domains);
            var result = Get(filterExpression).FirstOrDefault();
            return result == null ? true : false;
        }

    }
}
