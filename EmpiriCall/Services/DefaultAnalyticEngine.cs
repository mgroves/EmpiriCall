using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Mvc;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Services
{
    internal class DefaultAnalyticEngine : IAnalyticEngine
    {
        public void Execute(ActionExecutingContext filterContext, AnalyticEngineConfig config)
        {
            var queryProcessor = new Processor(EmpiriCallConfig.Resolver);
            // update meta data if necessary
            if (!MetaDataCheckCached(filterContext)) {
                queryProcessor.Execute(new CommandCreateMetaDataIfNecessary());
                SetMetaDataCheckCached(filterContext);
            }

            queryProcessor.Execute(ConstructDetail(filterContext, config));
        }

        bool MetaDataCheckCached(ActionExecutingContext filterContext)
        {
            var cache = filterContext.HttpContext.Cache;
            var cacheval = cache["EMPIRICALL_META_DATA_CHECKED"] as string;
            if (cacheval == null)
                return false;
            return cacheval == "CHECKED";
        }

        void SetMetaDataCheckCached(ActionExecutingContext filterContext)
        {
            var cache = filterContext.HttpContext.Cache;
            cache["EMPIRICALL_META_DATA_CHECKED"] = "CHECKED";
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