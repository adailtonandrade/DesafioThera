using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Interfaces.Services
{
    public interface IGenericService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetByIdNoTracking(int id);

        IEnumerable<T> GetAllNoTracking();
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
        string includeProperties = "");
        void Insert(T obj);
        void Update(T obj);
        List<string> Validate(T variable);
        bool IsUniqueField(string propertyName, string name, int? id);
    }
}
