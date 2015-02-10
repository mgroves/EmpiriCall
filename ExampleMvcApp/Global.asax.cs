using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Routing;
using EmpiriCall;
using EmpiriCall.Data.SQLServer;

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
            var dbConnection = new SqlConnection("server=(local);uid=;pwd=;Trusted_Connection=yes;database=EmpiriCallDemoDb");
            EmpiriCallConfig.LoadDbContainer(new SqlServerResolver(dbConnection));
        }

        protected void Application_EndRequest()
        {
            
        }
    }
}
