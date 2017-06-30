using System.Web.Mvc;
using System.Web.Routing;

namespace Tipwin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static LoginCounter LoginCounter;

        public MvcApplication()
        {
            LoginCounter = new LoginCounter();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
