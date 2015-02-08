using System.Web;

namespace EmpiriCall.Actions
{
    public interface IEmpiriCallAction
    {
        void Execute(HttpContext context);
    }
}