using Domain.Enum;
using Domain.Util;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DesafioThera.Controllers
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private TypePermissionEnum _claimType;
        private ValuePermissionEnum _claimValue;

        public ClaimsAuthorizeAttribute(TypePermissionEnum claimType, ValuePermissionEnum claimValue)
        {
            this._claimType = claimType;
            this._claimValue = claimValue;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var identity = (ClaimsIdentity)httpContext.User.Identity;
            string claimType = PropertyDescription.GetEnumDescription(_claimType);
            string claimValue = PropertyDescription.GetEnumDescription(_claimValue);
            var claim = identity.Claims.FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);
            if (claim != null)
            {
                return true;
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            bool user = HttpContext.Current.User.Identity.IsAuthenticated;
            if (user)
            {
                filterContext.Result =
                     new RedirectToRouteResult(
                         new RouteValueDictionary(new { action = "AccessDenied", controller = "Error" }));
            }
        }

        public static string GetUserProfile()
        {
            var claims = ((ClaimsIdentity)HttpContext.Current.User.Identity).Claims;
            return claims.FirstOrDefault(c => c.Type.Equals("PROFILE")).Value;
        }
    }
}