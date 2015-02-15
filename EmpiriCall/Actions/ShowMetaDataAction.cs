using System.Web;
using EmpiriCall.Actions.ViewModels;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    internal class ShowMetaDataAction : IEmpiriCallAction
    {
        readonly IRazorEngineService _razor;
        readonly Processor _processor;

        public ShowMetaDataAction(IRazorEngineService razor, Processor processor)
        {
            _razor = razor;
            _processor = processor;
        }

        public void Execute(HttpContext context)
        {
            var viewModel = new MetaDataView();

            viewModel.Meta = _processor.Query(new QueryGetLatestMetaData());
            context.Response.Write(_razor.View("MetaData", viewModel));
        }
    }
}