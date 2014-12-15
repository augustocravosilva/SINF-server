using FirstREST.Lib_Primavera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using System.Web.Routing;

namespace FirstREST
{

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            //e necessario instalar package -> ir a Tools->NuGet->Console
            //correr "Install-Package Microsoft.AspNet.WebApi -IncludePrerelease"
            //apagar a pasta Areas se for gerada (se for gerada nao conseguem fazer build)
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
              name: "PostWithAction",
              routeTemplate: "api/{controller}/{action}",
              defaults: new {action = "post"},
              constraints: new { httpMethod = new HttpMethodConstraint("POST","OPTIONS") }
          );

            config.Routes.MapHttpRoute(
              name: "PutWithAction",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional },
              constraints: new { httpMethod = new HttpMethodConstraint("PUT") }
          );

            config.Routes.MapHttpRoute(
            name: "GetWithoutAny",
            routeTemplate: "api/{controller}",
            defaults: new { },
            constraints: new { httpMethod = new HttpMethodConstraint("GET") }
        );

            config.Routes.MapHttpRoute(
            name: "Get",
            routeTemplate: "api/{controller}/{id}",
            defaults: new {action = "get", id = RouteParameter.Optional },
            constraints: new { httpMethod = new HttpMethodConstraint("GET") }
        );

            config.Routes.MapHttpRoute(
             name: "GetWithAction",
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { id = RouteParameter.Optional },
             constraints: new { httpMethod = new HttpMethodConstraint("GET") }
         );

//            config.Filters.Add(new AddCustomHeaderFilter());
            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();
        }
    }
}
