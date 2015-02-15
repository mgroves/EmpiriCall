using System;
using System.Linq;
using Db4objects.Db4o;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.Db4o.CommandHandlers
{
    public class CommandHandlerMapFeature : ICommandHandler<CommandMapFeature>
    {
        readonly string _db4oFilename;

        public CommandHandlerMapFeature(string db4oFilename)
        {
            _db4oFilename = db4oFilename;
        }

        public void Handle(CommandMapFeature command)
        {
            var config = Db4oEmbedded.NewConfiguration();
            config.Common.UpdateDepth = 5;
            using (var db = Db4oEmbedded.OpenFile(config, _db4oFilename))
            {
                var meta = db.Query<MetaData>().OrderByDescending(m => m.Version).FirstOrDefault();
                if (meta == null)
                    throw new Exception("Load the meta data first!");
            
                var actionInfo = db.Query<MetaData>().OrderByDescending(m => m.Version)
                    .First()
                    .ActionInfo
                    .Where(a => a.ControllerName == command.ControllerName)
                    .Where(a => a.ActionName == command.ActionName)
                    .Where(a => ParameterBasicInfo.AreTheSame(a.ParameterInfo, command.ParameterBasicInfos))
                    .SingleOrDefault();
            
                if (actionInfo == null)
                {
                    throw new Exception(string.Format("Action not found. Meta data need reloaded? [{0},{1},{2}]",
                        command.ControllerName,
                        command.ActionName,
                        string.Join("|", command.ParameterBasicInfos.Select(p => p.ParameterTypeFullName + " " + p.ParameterName))));
                }
            
                actionInfo.Feature = command.FeatureName;
                db.Store(meta);
            }
        }
    }
}