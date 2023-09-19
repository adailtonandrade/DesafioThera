using Data.Context;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Data.Repositories
{
    public class TalentRepository : GenericRepository<Talent>, ITalentRepository
    {
        private ModelContext _context;

        public TalentRepository(ModelContext context) : base(context)
        {
            _context = context;
        }

    }
}
