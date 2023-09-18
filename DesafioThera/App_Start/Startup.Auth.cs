using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Identity.Configuration;
using Identity.Model;
using System.Web.Mvc;

namespace DesafioThera
{
    public partial class Startup
    {
        // Para obter mais informações sobre a autenticação de configuração, visite https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure o contexto db, gerenciador de usuários e gerenciador de login para usar uma única instância por solicitação
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());

            // Habilitar o aplicativo a usar um cookie para armazenar informações do usuário logado
            // e para usar um cookie para armazenar temporariamente informações sobre um usuário fazendo logon com um provedor de logon de terceiros
            // Configurar o cookie de logon
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Permite que o aplicativo valide o carimbo de segurança quando o usuário efetuar login.
                    // Este é um recurso de segurança que é usado quando você altera uma senha ou adiciona um login externo à sua conta.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                    validateInterval: TimeSpan.FromMinutes(30), regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (id) => (id.GetUserId<int>()))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Permite que o aplicativo armazene temporariamente as informações do usuário quando ele estiver verificando o segundo fator no processo de autenticação de dois fatores.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Permite que o aplicativo lembre segundo fator de verificação de login, como telefone ou email.
            // Assim que você marcar esta opção, sua segunda etapa de verificação durante o processo de login será lembrada no dispositivo no qual você efetuou login.
            // Isso é semelhante à opção RememberMe (Lembre-me) quando você efetua login.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}