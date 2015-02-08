using System.Web;
using EmpiriCall.Actions.ViewModels;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    public class ShowMainReportAction : IEmpiriCallAction
    {
        readonly IRazorEngineService _razor;
        readonly Processor _processor;

        public ShowMainReportAction(IRazorEngineService razor, Processor processor)
        {
            _razor = razor;
            _processor = processor;
        }

        public void Execute(HttpContext context)
        {
            var viewModel = new MainReportView();
            var metaData = _processor.Query(new QueryGetMetaData());
            viewModel.LastMetaDataUpdateDate = metaData == null ? "Never" : metaData.LastUpdated.ToString();
            context.Response.Write(_razor.View("MainReport", viewModel));
        }
    }
}