using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

namespace VisReg.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user.
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                // The UseCookieAuthentication extension tells the ASP.NET Identity framework to use cookie based authentication & we need to set 2 properties.
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Account/Login"),
                CookieName = "VisRegWeB",

                // For Automatic LogOff, after a period time of inactivity
                //SlidingExpiration = false,
                //ExpireTimeSpan = TimeSpan.FromMinutes(2),

                Provider = new CookieAuthenticationProvider()
                {
                    OnApplyRedirect = ctx =>
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                }

            });
        }
    }
}