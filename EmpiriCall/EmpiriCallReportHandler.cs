using System.Web;
using EmpiriCall.Actions;

namespace EmpiriCall
{
    public class EmpiriCallReportHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var commandString = context.Request.QueryString["command"];
            var command = ActionFactory.GetCommand(commandString, context);
            command.Execute(context);
        }

        public bool IsReusable { get { return true;} }
    }
}