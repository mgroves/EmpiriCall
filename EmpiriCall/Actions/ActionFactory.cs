﻿using System.Web;
using EmpiriCall.Data.DataAccess;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    internal static class ActionFactory
    {
        public static IEmpiriCallAction GetCommand(string commandString, HttpContext context)
        {
            var razor = EmbeddedRazor();
            var processor = new Processor(EmpiriCallConfig.Resolver);

            switch (commandString)
            {
                case "rawdetail": return new ShowRawDetailAction(razor, processor);
                case "addmetaversion": return new AddMetaDataVersionAction(processor);
                case "showmetadata": return new ShowMetaDataAction(razor, processor);
                case "showcalldata": return new ShowCallDataAction(razor, processor);
                case "loadfeature": return new LoadFeatureMaps(razor);
                case "savefeatures": return new SaveFeatures(razor, processor);
                default:
                    return new ViewOrShowMainMenuAction(commandString, razor, processor);
            }
        }

        static IRazorEngineService EmbeddedRazor()
        {
            var config = new TemplateServiceConfiguration();
            config.TemplateManager = new DelegateTemplateManager(s => ResourceHelper.TextOfResource(s + ".cshtml"));
            config.Debug = true;
            config.BaseTemplateType = typeof (RazorCustomTemplateBase<>);
            return RazorEngineService.Create(config);
        }
    }
}