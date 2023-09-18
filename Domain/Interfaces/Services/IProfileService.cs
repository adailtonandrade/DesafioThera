using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces.Services
{
    public interface IProfileService : IGenericService<Profile>
    {
        bool VerifyProfileExists(Profile user);
        List<Permission> GetPermissions(int idProfile);
    }
}
