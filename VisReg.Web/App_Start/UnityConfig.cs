using System.Web.Mvc;
using System.Web;
using Unity;
using Unity.Mvc5;
using Unity.Injection;
using Microsoft.Owin.Security;
using VisReg.Srv;
using VisReg.Srv.Interfaces;

namespace VisReg.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<ISrvUser, SrvUser>();
            container.RegisterType<ISrvPassword, SrvPassword>();
            container.RegisterType<ISrvAuthentication, SrvAuthentication>();

            container.RegisterType<ISrvVisitor, SrvVisitor>();
            container.RegisterType<ISrvVisit, SrvVisit>();
            container.RegisterType<ISrvCommon, SrvCommon>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}