using Data.Context;
using Domain.Interfaces.Repositories;

namespace Data.Repositories
{
    public class ComposedKeyRepository<T> : GenericRepository<T>, IComposedKeyRepository<T> where T : class
    {
        private ModelContext _context;

        public ComposedKeyRepository(ModelContext context) : base(context)
        {
            _context = context;
        }

        public void Delete(T obj)
        {
            _dbSet.Attach(obj);
            _dbSet.Remove(obj);
        }

        public T GetByComposedKey(int param1, int param2)
        {
            return _dbSet.Find(param1, param2);
        }
    }
}