using System.Web;
using EmpiriCall.Actions.ViewModels;
using EmpiriCall.Data.DataAccess;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    internal class LoadFeatureMaps : IEmpiriCallAction
    {
        readonly IRazorEngineService _razor;

        public LoadFeatureMaps(IRazorEngineService razor)
        {
            _razor = razor;
        }

        public void Execute(HttpContext context)
        {
            var viewModel = new LoadFeatureMapViewModel();
            if (EmpiriCallConfig.FeatureMapper == null)
            {
                context.Response.Write(_razor.View("LoadFeatureMaps", viewModel));
                return;
            }

            var mapFeature = new MapFeature();
            EmpiriCallConfig.FeatureMapper.Map(mapFeature);

            viewModel.MapFeatureLog = mapFeature.Log;
            context.Response.Write(_razor.View("LoadFeatureMaps", viewModel));
        }
    }
}