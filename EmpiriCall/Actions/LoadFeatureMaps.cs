using System.Web;

namespace EmpiriCall.Actions
{
    public class LoadFeatureMaps : IEmpiriCallAction
    {
        public void Execute(HttpContext context)
        {
            EmpiriCallConfig.FeatureMapper.Map(new MapFeature());
            context.Response.Redirect("/EmpiriCall.axd");
        }
    }
}