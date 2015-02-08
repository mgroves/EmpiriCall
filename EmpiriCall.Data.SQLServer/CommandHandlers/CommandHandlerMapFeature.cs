using System;
using System.Linq;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.SQLServer.CommandHandlers
{
    public class CommandHandlerMapFeature : ICommandHandler<CommandMapFeature>
    {
        readonly EmpiriCallDbContext _context;

        public CommandHandlerMapFeature(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public void Handle(CommandMapFeature command)
        {
            var meta = _context.MetaData.SingleOrDefault();
            if (meta == null)
                throw new Exception("Load the meta data first!");
            
            var actionInfo = meta.ActionInfo
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
            _context.SaveChanges();
        }
    }
}