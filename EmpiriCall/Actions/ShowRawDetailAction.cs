using System.Web;
using EmpiriCall.Actions.ViewModels;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    public class ShowRawDetailAction : IEmpiriCallAction
    {
        readonly IRazorEngineService _razor;
        readonly Processor _queryProcessor;

        public ShowRawDetailAction(IRazorEngineService razor, Processor queryProcessor)
        {
            _razor = razor;
            _queryProcessor = queryProcessor;
        }

        public void Execute(HttpContext context)
        {
            var viewModel = new RawDetailView();

            viewModel.Details = _queryProcessor.Query(new QueryRawDetail());

            context.Response.Write(_razor.View("RawDetail", viewModel));
        }
    }
}