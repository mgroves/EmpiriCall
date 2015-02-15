using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Services
{
    public class DefaultAnalyticEngine : IAnalyticEngine
    {
        public void Execute(ActionExecutingContext filterContext, AnalyticEngineConfig config)
        {
            // update meta data if necessary
            var queryProcessor = new Processor(EmpiriCallConfig.Resolver);
            queryProcessor.Execute(new CommandCreateMetaDataIfNecessary());

            queryProcessor.Execute(ConstructDetail(filterContext, config));
        }

        CommandAddRecord ConstructDetail(ActionExecutingContext filterContext, AnalyticEngineConfig config)
        {
            var actionName = filterContext.ActionDescriptor.ActionName;
            var controllerName = filterContext.Controller.GetType().FullName;
            var parameters = filterContext.ActionDescriptor.GetParameters();

            var commandArgs = new CommandAddRecord
            {
                ActionName = actionName,
                ControllerName = controllerName,
                TimeStamp = DateTime.Now,
                ParameterInfo = parameters.Any()
                    ? parameters.Select(p =>
                        new ParameterBasicInfo
                        {
                            ParameterName = p.ParameterName,
                            ParameterTypeFullName = p.ParameterType.FullName
                        }).ToList()
                    : new List<ParameterBasicInfo>()
            };

            if (config.GetUserName != null)
                commandArgs.UserName = config.GetUserName();
            if (config.GetCustomValues != null)
                commandArgs.CustomValues = config.GetCustomValues();

            return commandArgs;
        }
    }
}