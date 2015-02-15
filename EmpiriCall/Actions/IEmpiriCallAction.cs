using System.Web;

namespace EmpiriCall.Actions
{
    internal interface IEmpiriCallAction
    {
        void Execute(HttpContext context);
    }
}