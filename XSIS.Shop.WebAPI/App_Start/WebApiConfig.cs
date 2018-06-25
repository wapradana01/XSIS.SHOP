using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace XSIS.Shop.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "CustomMethodNoParam",
               routeTemplate: "api/{controller}/{action}",
               defaults: new { action = "get" }
               );

            config.Routes.MapHttpRoute(
                name: "CustomMethodApi1Param",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "get", id = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "CustomMethodApi2Param",
                routeTemplate: "api/{controller}/{action}/{id1}/{id2}",
                defaults: new { action = "get", id1 = RouteParameter.Optional, id2 = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
              name: "ProductMethodApi1Param",
              routeTemplate: "api/ProductApi/{action}/{id}",
              defaults: new { controller = "ProductApi", action = "get", id = RouteParameter.Optional }
              );

            config.Routes.MapHttpRoute(
               name: "OrderMethodApi1Param",
               routeTemplate: "api/OrderApi/{action}/{id}",
               defaults: new { controller = "OrderApi", action = "get", id = RouteParameter.Optional }
               );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
