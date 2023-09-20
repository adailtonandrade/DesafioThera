using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Identity.Model;
using Identity.Configuration;
using Identity.ViewModels;
using Domain.Enum;
using System.Collections.Generic;
using Application.Interfaces;
using System.Security.Claims;
using CrossCutting.JWTConfig;
using System.Web.Http;
using System.Web;

namespace DesafioApi.Controllers
{

    public class AccountController : ApiController
    {
        List<string> errors = new List<string>();
        private string activeStatus = ((int)GenericStatusEnum.Active).ToString();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IProfileAppService _profileAppService;
        private readonly JWTService _jwtService;


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IProfileAppService profileAppService,
            JWTService jwtService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _profileAppService = profileAppService;
            _jwtService = jwtService;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // POST: /Account/Login
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [Route("api/users/login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginVM model)
        {
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            if (result == SignInStatus.Success)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var userClaims = GetUserClaims(user);
                userClaims.Add(new Claim("UserId", user.Id.ToString()));
                var response = _jwtService.GenerateToken(model, userClaims);
                return Ok(new { message = "Autenticado com Sucesso", Token = response });
            }
            return Unauthorized();
        }

        private List<Claim> GetUserClaims(ApplicationUser user)
        {
            var permissions = _profileAppService.GetPermissions(user.ProfileId);

            var userClaims = new List<Claim>();

            foreach (var permission in permissions)
                userClaims.Add(new Claim(permission.ClaimType, permission.ClaimValue));

            userClaims.Add(new Claim("NickName", user.NickName));

            return userClaims;
        }
    }
}