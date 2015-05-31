using System.Data.Common;
using System.Web.Mvc;
using System.Web.Routing;
using EmpiriCall;
using EmpiriCall.Data.RabbitMQ;
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
            // use this to write to SQL directly:
            //EmpiriCallConfig.LoadDbContainer(new SqlServerResolver(DependencyResolver.Current.GetService<DbConnection>()));

            // use this to write to rabbitMQ queue, of which the consumer will write to SQL
            EmpiriCallConfig.LoadDbContainer(new RabbitMqResolver(
                DependencyResolver.Current.GetService<DbConnection>(),  // still need a SQL db connection for reporting
                "localhost")                                            // the rabbit MQ hostname
            );

            // specify a feature map (optional)
            //EmpiriCallConfig.LoadFeatureMapper(new ExampleMvcFeatureMap());
        }

        protected void Application_EndRequest()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}
