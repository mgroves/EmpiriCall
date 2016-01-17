using System.Linq;
using System.Web;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    public class SaveFeatures : IEmpiriCallAction
    {
        readonly IRazorEngineService _razor;
        readonly IProcessor _processor;

        public SaveFeatures(IRazorEngineService razor, IProcessor processor)
        {
            _razor = razor;
            _processor = processor;
        }

        public void Execute(HttpContext context)
        {
            var command = new CommandUpdateFeatures();
            command.FeatureMap = context.Request.Form.AllKeys.Select(k => new
            {
                Id = int.Parse(k.Replace("Feature","")),
                Feature = context.Request.Form[k].ToString()
            }).ToDictionary(x => x.Id, y => y.Feature);

            _processor.Execute(command);

            context.Response.Write(_razor.View("SaveFeatures"));
        }
    }
}