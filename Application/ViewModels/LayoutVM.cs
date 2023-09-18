using Domain.Enum;
using Domain.Util;
using System.Linq;
using System.Security.Claims;

namespace Application.ViewModels
{
    public static class LayoutVM
    {
        public static bool VerifyClaimExists(TypePermissionEnum _claimType, ValuePermissionEnum _claimValue, ClaimsIdentity ClaimsUser)
        {
            string claimType = PropertyDescription.GetEnumDescription(_claimType);
            string claimValue = PropertyDescription.GetEnumDescription(_claimValue);
            var claim = ClaimsUser.Claims.FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);
            return claim != null;
        }
    }
}
