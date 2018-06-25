using System.Web.Http;
using WebActivatorEx;
using XSIS.Shop.WebAPI;
using System.Linq;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace XSIS.Shop.WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "XSIS.Shop.WebAPI");
                        c.ResolveConflictingActions(apiDescription => apiDescription.First());
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
