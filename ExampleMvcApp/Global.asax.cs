using System.Data.Common;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EmpiriCall;
using EmpiriCall.Data.RabbitMQ;
using EmpiriCall.Data.SQLServer;
using ExampleMvcApp.Controllers;
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

            var recordUserConfig = new AnalyticEngineConfig
            {
                GetUserName = () =>
                {
                    // TODO: work "CurrentUserName" into demo app for testing
                    if (HttpContext.Current == null)
                        return null;
                    if (HttpContext.Current.Session == null)
                        return null;
                    if (HttpContext.Current.Session["CurrentUserName"] == null)
                        return null;
                    return HttpContext.Current.Session["CurrentUserName"].ToString();
                }
            };

            GlobalFilters.Filters.Add(new EmpiriCallActionFilter(analyticConfig: recordUserConfig));
        }

        protected void Application_BeginRequest()
        {
            // use this to write to SQL directly:
            EmpiriCallConfig.LoadDbContainer(new SqlServerResolver(DependencyResolver.Current.GetService<DbConnection>()));

            // use this to write to rabbitMQ queue, of which the consumer will write to SQL
//            EmpiriCallConfig.LoadDbContainer(new RabbitMqResolver(
//                DependencyResolver.Current.GetService<DbConnection>(),  // still need a SQL db connection for reporting
//                "host=localhost")                                            // the rabbit MQ connection string
//            );

            //EmpiriCallConfig.LoadFeatureMapper(new MyFeatureMapper());

            // specify a feature map (optional)
            //EmpiriCallConfig.LoadFeatureMapper(new ExampleMvcFeatureMap());
        }

        protected void Application_EndRequest()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }

    public class MyFeatureMapper : IFeatureMapper
    {
        public void Map(MapFeature map)
        {
            map.Of<HomeController>(x => x.Index(), "Home Feature");
            map.Of<HomeController>(x => x.Foo(0, null), "Home Feature");
            map.Of<OtherController>(x => x.Bar(null), "Other Feature");
            map.Of<OtherController>(x => x.Baz(null), "Other Feature");
        }
    }
}
