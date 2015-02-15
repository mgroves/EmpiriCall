using System.Web;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Actions
{
    internal class AddMetaDataVersionAction : IEmpiriCallAction
    {
        readonly IProcessor _processor;

        public AddMetaDataVersionAction(IProcessor processor)
        {
            _processor = processor;
        }

        public void Execute(HttpContext context)
        {
            _processor.Execute(new CommandAddNewMetaDataVersion());

            context.Response.Redirect("/EmpiriCall.axd");
        }
    }
}