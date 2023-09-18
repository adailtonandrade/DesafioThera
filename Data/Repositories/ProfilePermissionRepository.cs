using Domain.Interfaces.Repositories;
using Domain.Entities;
using Data.Context;

namespace Data.Repositories
{
    public class ProfilePermissionRepository : GenericRepository<ProfilePermission>, IGenericRepository<ProfilePermission>
    {
        private ModelContext _context;


        public ProfilePermissionRepository(ModelContext context) : base(context)
        {
            _context = context;
        }

        public void Delete(ProfilePermission obj)
        {
            _dbSet.Attach(obj);
            _dbSet.Remove(obj);
        }

        public ProfilePermission GetByComposedKey(int param1, int param2)
        {
            return _dbSet.Find(param1, param2);
        }
    }
}
