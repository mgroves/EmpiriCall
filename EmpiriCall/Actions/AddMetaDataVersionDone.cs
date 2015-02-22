using System.Web;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    public class AddMetaDataVersionDone : IEmpiriCallAction
    {
        IRazorEngineService _razor;

        public AddMetaDataVersionDone(IRazorEngineService razor)
        {
            _razor = razor;
        }

        public void Execute(HttpContext context)
        {
            context.Response.Write(_razor.View("AddMetaDataVersionDone"));
        }
    }
}