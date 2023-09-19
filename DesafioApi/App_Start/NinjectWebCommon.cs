[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DesafioApi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DesafioApi.App_Start.NinjectWebCommon), "Stop")]

namespace DesafioApi.App_Start
{
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
    using Service;
    using System.Web;
    using Data.Repositories;
    using Domain.Interfaces.Repositories;
    using System.Web.Mvc;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using System;
    using CrossCutting.JWTConfig;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application.
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            #region Identity
            kernel.Bind<ApplicationDbContext>().ToSelf().When(x => HttpContext.Current == null); //.InCallScope();
            kernel.Bind<IRoleStore<IdentityRole, string>>().To<RoleStore<IdentityRole>>();
            kernel.Bind<ApplicationUserManager>().ToSelf();
            kernel.Bind<ApplicationRoleManager>().ToSelf();
            kernel.Bind<ApplicationSignInManager>().ToSelf();
            kernel.Bind<IUserStore<ApplicationUser, int>>().ToMethod(
                 x => new CustomUserStore(kernel.Get<ApplicationDbContext>()));
            kernel.Bind(typeof(UserManager<>)).ToMethod(
                x => new CustomUserStore(kernel.Get<ApplicationDbContext>()));

            kernel.Bind<IAuthenticationManager>().ToMethod(c =>
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
            kernel.Bind<IProfilePermissionAppService>().To<ProfilePermissionAppService>();
            kernel.Bind<ITalentAppService>().To<TalentAppService>();
            kernel.Bind<IPermissionAppService>().To<PermissionAppService>();
            kernel.Bind<IProfileAppService>().To<ProfileAppService>();
            kernel.Bind<IUserAppService>().To<UserAppService>();
            #endregion

            #region Service
            kernel.Bind(typeof(IGenericService<>)).To(typeof(GenericService<>));
            kernel.Bind<IProfileService>().To<ProfileService>();
            kernel.Bind<ITalentService>().To<TalentService>();
            kernel.Bind<IProfilePermissionService>().To<ProfilePermissionService>();
            kernel.Bind<IUserService>().To<UserService>();
            #endregion

            #region AutoMapper
            kernel.Bind<IMapper>().ToMethod(ctx => new Mapper(AutoMapperConfig.RegisterMappings())).InSingletonScope();
            #endregion

            #region Repository
            kernel.Bind<IProfileRepository>().To<ProfileRepository>();
            kernel.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind(typeof(IComposedKeyRepository<>)).To(typeof(ComposedKeyRepository<>));
            kernel.Bind<IUserRepository>().To<UserRepository>();
            #endregion

            #region Universal
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            kernel.Bind<ModelContext>().ToSelf().InTransientScope();
            kernel.Bind<JWTService>().ToSelf().InTransientScope();
            #endregion
            kernel.Unbind<ModelValidatorProvider>();
        }
    }
}