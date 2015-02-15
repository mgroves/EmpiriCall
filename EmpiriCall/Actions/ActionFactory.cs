using System.IO;
using System.Reflection;
using System.Web;
using EmpiriCall.Data.DataAccess;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace EmpiriCall.Actions
{
    public static class ActionFactory
    {
        public static IEmpiriCallAction GetCommand(string commandString, HttpContext context)
        {
            var razor = EmbeddedRazor();
            var processor = new Processor(EmpiriCallConfig.Resolver);

            if (commandString != null)
            {
                switch (commandString)
                {
                    case "rawdetail": return new ShowRawDetailAction(razor, processor);
                    case "addmetaversion": return new AddMetaDataVersionAction(processor);
                    case "showmetadata": return new ShowMetaDataAction(razor, processor);
                    case "showcalldata": return new ShowCallDataAction(razor, processor);
                    case "loadfeature": return new LoadFeatureMaps();
                    default: return new ShowMainMenuAction(razor, processor);
                }
            }
            return new ShowMainMenuAction(razor, processor);
        }

        static IRazorEngineService EmbeddedRazor()
        {
            var config = new TemplateServiceConfiguration();
            config.TemplateManager = new DelegateTemplateManager(s =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "EmpiriCall.Templates." + s + ".cshtml";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            });
            config.Debug = true;
            return RazorEngineService.Create(config);
        }
    }
}