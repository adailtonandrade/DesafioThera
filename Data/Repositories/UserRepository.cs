using Data.Context;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private ModelContext _context;

        public UserRepository(ModelContext context): base(context)
        {
            _context = context;
        }

    }
}
