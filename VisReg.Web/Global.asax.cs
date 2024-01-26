using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace VisReg.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Dependecy Injection via Unity.
            UnityConfig.RegisterComponents();
            // Automapping via AutoMapper.
            MappingConfig.Configure();

            //
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimsIdentity.DefaultNameClaimType;

            //
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(RequiredAttributeAdapter));
        }
    }
}
