using System.Web;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Actions
{
    public class ForceMetaUpdateAction : IEmpiriCallAction
    {
        readonly IProcessor _processor;

        public ForceMetaUpdateAction(IProcessor processor)
        {
            _processor = processor;
        }

        public void Execute(HttpContext context)
        {
            _processor.Execute(new CommandMetaDataUpdate { ForceUpdate = true});

            context.Response.Redirect("/EmpiriCall.axd");
        }
    }
}