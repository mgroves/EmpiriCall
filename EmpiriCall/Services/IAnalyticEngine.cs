using System.Web.Mvc;

namespace EmpiriCall.Services
{
    public interface IAnalyticEngine
    {
        void Execute(ActionExecutingContext filterContext, AnalyticEngineConfig config);
    }
}