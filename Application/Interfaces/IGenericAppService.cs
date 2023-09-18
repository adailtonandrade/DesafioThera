using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Interfaces
{
    public interface IGenericAppService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
        string includeProperties = "");
        List<string> Insert(T obj);
        List<string> Update(T obj);
        List<string> Delete(int id);
    }
}
