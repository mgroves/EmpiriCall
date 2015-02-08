using System.Web.Mvc;
using EmpiriCall.Services;

namespace EmpiriCall
{
    public class EmpiriCallActionFilter : ActionFilterAttribute
    {
        readonly IAnalyticEngine _analyticEngine;
        readonly AnalyticEngineConfig _config;

        public EmpiriCallActionFilter(IAnalyticEngine analyticEngine = null, AnalyticEngineConfig analyticConfig = null)
        {
            _analyticEngine = analyticEngine ?? new DefaultAnalyticEngine();
            _config = analyticConfig ?? new AnalyticEngineConfig();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _analyticEngine.Execute(filterContext, _config);

            base.OnActionExecuting(filterContext);
        }
    }
}
