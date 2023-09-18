using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System.Collections.Generic;

namespace Service
{
    public class ProfilePermissionService : GenericService<ProfilePermission>, IProfilePermissionService
    {
        private readonly IComposedKeyRepository<ProfilePermission> _accessRepository;

        public ProfilePermissionService(IComposedKeyRepository<ProfilePermission> accessRepository) : base(accessRepository)
        {
            _accessRepository = accessRepository;
        }

        public ProfilePermission GetByComposedKey(int param1, int param2)
        {
            return _accessRepository.GetByComposedKey(param1, param2);
        }

        public List<string> Delete(ProfilePermission obj)
        {
            _accessRepository.Delete(obj);
            return new List<string>();
        }

        public override List<string> Validate(ProfilePermission variable)
        {
            throw new System.NotImplementedException();
        }
    }
}
