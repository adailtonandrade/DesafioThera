using Application;
using Application.Interfaces;
using Application.Mapper;
using AutoMapper;
using Data.Context;
using Data;
using Domain.Interfaces.Data;
using Domain.Interfaces.Services;
using Identity.Configuration;
using Identity.Context;
using Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Ninject;
using Ninject.Modules;
using Ninject.Extensions.NamedScope;
using Service;
using System.Web;
using Data.Repositories;
using Domain.Interfaces.Repositories;

namespace CrossCutting.IoC
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            var kernel = this.Kernel;
            #region Identity
            Bind<ApplicationDbContext>().ToSelf().When(x => HttpContext.Current == null); //.InCallScope();
            Bind<IRoleStore<IdentityRole, string>>().To<RoleStore<IdentityRole>>();
            Bind<ApplicationUserManager>().ToSelf();
            Bind<ApplicationRoleManager>().ToSelf();
            Bind<ApplicationSignInManager>().ToSelf();
            Bind<IUserStore<ApplicationUser, int>>().ToMethod(
                 x => new CustomUserStore(kernel.Get<ApplicationDbContext>()));
            Bind(typeof(UserManager<>)).ToMethod(
                x => new CustomUserStore(Kernel.Get<ApplicationDbContext>()));

            Bind<IAuthenticationManager>().ToMethod(c =>
            {
                if (HttpContext.Current != null && HttpContext.Current.Items["owin.Environment"] == null)
                {
                    return new OwinContext().Authentication;
                }
                //return HttpContext.Current.;
                return HttpContext.Current.GetOwinContext().Authentication;
            });
            #endregion

            #region App
            Bind<IProfilePermissionAppService>().To<ProfilePermissionAppService>();
            Bind<ITalentAppService>().To<TalentAppService>();
            Bind<IPermissionAppService>().To<PermissionAppService>();
            Bind<IProfileAppService>().To<ProfileAppService>();
            Bind<IUserAppService>().To<UserAppService>();
            #endregion

            #region Service
            Bind(typeof(IGenericService<>)).To(typeof(GenericService<>));
            Bind<IProfileService>().To<ProfileService>();
            Bind<ITalentService>().To<TalentService>();
            Bind<IProfilePermissionService>().To<ProfilePermissionService>();
            Bind<IUserService>().To<UserService>();
            #endregion

            #region AutoMapper
            Bind<IMapper>().ToMethod(ctx => new Mapper(AutoMapperConfig.RegisterMappings())).InSingletonScope();
            #endregion

            #region Repository
            kernel.Bind<IProfileRepository>().To<ProfileRepository>();
            kernel.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind(typeof(IComposedKeyRepository<>)).To(typeof(ComposedKeyRepository<>));
            kernel.Bind<IUserRepository>().To<UserRepository>();
            #endregion

            #region Universal
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<ModelContext>().ToSelf().InCallScope();
            #endregion
        }
    }
}
