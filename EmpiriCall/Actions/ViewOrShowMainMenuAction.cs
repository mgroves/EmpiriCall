using System.Web;
using EmpiriCall.Data.DataAccess;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    public class ViewOrShowMainMenuAction : IEmpiriCallAction
    {
        readonly IRazorEngineService _razor;
        readonly Processor _processor;
        readonly string _commandName;

        public ViewOrShowMainMenuAction(string commandName, IRazorEngineService razor, Processor processor)
        {
            _razor = razor;
            _processor = processor;
            _commandName = commandName;
        }

        public void Execute(HttpContext context)
        {
            if (ResourceHelper.ResourceExists(_commandName + ".cshtml"))
                context.Response.Write(_razor.View(_commandName));
            else
                new ShowMainMenuAction(_razor, _processor).Execute(context);
        }
    }
}