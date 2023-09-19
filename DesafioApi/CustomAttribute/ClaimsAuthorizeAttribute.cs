using Domain.Enum;
using Domain.Util;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DesafioApi.CustomAttribute
{
    public class ClaimsAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private TypePermissionEnum _claimType;
        private ValuePermissionEnum _claimValue;

        public ClaimsAuthorizationAttribute(TypePermissionEnum claimType, ValuePermissionEnum claimValue)
        {
            this._claimType = claimType;
            this._claimValue = claimValue;
        }

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Token expirado ou inválido");
                return Task.FromResult<object>(null);
            }

            if (!(principal.HasClaim(x => x.Type == PropertyDescription.GetEnumDescription(_claimType) && x.Value == PropertyDescription.GetEnumDescription(_claimValue))))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Você não tem acesso para realizar essa ação");
                return Task.FromResult<object>(null);
            }

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);

        }
    }
}