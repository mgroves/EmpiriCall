using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall
{
    public class MapFeature
    {
        internal List<CommandMapFeature> Log { get; private set; }

        internal MapFeature()
        {
            Log = new List<CommandMapFeature>();
        }

        public MapFeature Of<T>(Expression<Func<T, ActionResult>> exp, string featureName) where T : Controller
        {
            var controllerName = typeof (T).FullName;

            var methodExpression = (MethodCallExpression) exp.Body;

            var methodName = methodExpression.Method.Name;
            var parameterBasicInfos = methodExpression.Method.GetParameters()
                .Select(p => new ParameterBasicInfo
                {
                    ParameterTypeFullName = p.ParameterType.FullName,
                    ParameterName = p.Name
                }).ToList();

            MapToMetaData(controllerName, methodName, parameterBasicInfos, featureName);

            return this;
        }

        void MapToMetaData(string controllerName, string actionName, List<ParameterBasicInfo> parameterBasicInfos, string featureName)
        {
            var processor = new Processor(EmpiriCallConfig.Resolver);
            var cmf = new CommandMapFeature
            {
                FeatureName = featureName,
                ControllerName = controllerName,
                ActionName = actionName,
                ParameterBasicInfos = parameterBasicInfos
            };
            processor.Execute(cmf);
            Log.Add(cmf);
        }
    }
}