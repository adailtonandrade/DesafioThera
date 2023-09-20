using ApiMultiPartFormData;
using ApiMultiPartFormData.Services.Interfaces;
using DesafioApi.ModelBind;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

namespace DesafioApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Add(new MultipartFormDataFormatter(
                new IModelBinderService[]
                {
                    new HttpPostedFileBaseModelBinderService()
                }));
        }
    }
}