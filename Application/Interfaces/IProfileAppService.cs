using Application.ViewModels;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IProfileAppService : IGenericAppService<ProfileVM>
    {
        List<PermissionVM> GetPermissions(int idProfile);
    }
}
