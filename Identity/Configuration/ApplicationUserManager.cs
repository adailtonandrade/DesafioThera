using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Identity.Model;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Linq.Expressions;
using System.Linq;
using Application.ViewModels;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;
using Application.Interfaces;

namespace Identity.Configuration
{
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {

        private IProfileAppService _profileAppService;
        private IUserAppService _userAppService;
        public static Dictionary<string, string> dicErrors;

        //FIND PROFILE
        public override Task<IList<string>> GetRolesAsync(int userId)
        {
            var user = GetUser(userId);
            return Task.FromResult((IList<string>)new List<string>() { _profileAppService.GetById(user.ProfileId).Name });
        }

        private UserVM GetUser(int userId)
        {
            return _userAppService.GetById(userId);
        }

        public override Task<IList<Claim>> GetClaimsAsync(int userId)
        {
            var Claims = GetClaims(userId);
            return Task.FromResult((IList<Claim>)Claims);
        }

        private List<Claim> GetClaims(int userId)
        {
            var user = GetUser(userId);
            var PermissionList = _profileAppService.GetPermissions(user.ProfileId);
            return BindClaims(PermissionList);
        }

        private List<Claim> BindClaims(List<PermissionVM> Access)
        {
            List<Claim> Claims = new List<Claim>();
            foreach (var item in Access)
            {
                Claims.Add(new Claim(item.ClaimType, item.ClaimValue));
            }
            return Claims;
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            dicErrors = new Dictionary<string, string>();
            List<string> errors = new List<string>();
            Expression<Func<UserVM, bool>> filterUser;
            filterUser = (UserVM p) => (p.Cpf.Equals(user.Cpf));
            var resultCpf = _userAppService.Get(filterUser);
            filterUser = (UserVM p) => (p.Email.ToLower().Equals(user.Email.ToLower()));
            var resultEmail = _userAppService.Get(filterUser);
            if (resultEmail.Count() > 0)
                dicErrors.Add(nameof(user.Email), "O E-mail informado já se encontra cadastrado para outro usuário.");
            if (dicErrors.Count > 0)
                return Task.FromResult((IdentityResult)IdentityResult.Failed());
            return base.CreateAsync(user, password);
        }
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store, IProfileAppService profileAppService, IUserAppService userAppService)
            : base(store)
        {
            _profileAppService = profileAppService;
            _userAppService = userAppService;

            //Username validation config
            UserValidator = new UserValidator<ApplicationUser, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            PasswordValidator = new CustomPasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            //Lockout Configuaration
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Providers to Two Factor Autentication
            RegisterTwoFactorProvider("Código via SMS", new PhoneNumberTokenProvider<ApplicationUser, int>
            {
                MessageFormat = "Seu código de segurança é: {0}"
            });

            RegisterTwoFactorProvider("Código via E-mail", new EmailTokenProvider<ApplicationUser, int>
            {
                Subject = "Código de Segurança",
                BodyFormat = "Seu código de segurança é: {0}"
            });

            //E-mail service class Config
            EmailService = new MailService();


            var provider = new DpapiDataProtectionProvider("MeuSite");
            var dataProtector = provider.Create("EmailConfirmation");

            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtector);
        }
    }
}