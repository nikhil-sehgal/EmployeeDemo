using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Web.Http.Cors;

namespace EmployeeDemoWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Remove XML formatting
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Configure JSON Output
            config.Formatters.JsonFormatter.SerializerSettings.Formatting =
                            Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            //Enable CORS
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
