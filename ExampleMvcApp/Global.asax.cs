using System.Data.Common;
using System.Web.Mvc;
using System.Web.Routing;
using EmpiriCall;
using EmpiriCall.Data.SQLServer;
using StructureMap.Web.Pipeline;

namespace ExampleMvcApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFilters.Filters.Add(new EmpiriCallActionFilter());
        }

        protected void Application_BeginRequest()
        {
            EmpiriCallConfig.LoadDbContainer(new SqlServerResolver(DependencyResolver.Current.GetService<DbConnection>()));
            EmpiriCallConfig.LoadFeatureMapper(new ExampleMvcFeatureMap());
        }

        protected void Application_EndRequest()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}
