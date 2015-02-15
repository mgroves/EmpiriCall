using System.Web;
using EmpiriCall.Actions.ViewModels;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    public class ShowCallDataAction : IEmpiriCallAction
    {
        readonly IRazorEngineService _razor;
        readonly IProcessor _processor;

        public ShowCallDataAction(IRazorEngineService razor, IProcessor processor)
        {
            _razor = razor;
            _processor = processor;
        }

        public void Execute(HttpContext context)
        {
            var viewModel = new CallDataView();

            viewModel.MetaData = _processor.Query(new QueryGetLatestMetaData());
            
            context.Response.Write(_razor.View("CallData", viewModel));
        }
    }
}