using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces.Repositories
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        List<Permission> GetPermissions(int idProfile);
    }
}
