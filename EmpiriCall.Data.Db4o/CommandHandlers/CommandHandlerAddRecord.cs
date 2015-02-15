using System.Collections.Generic;
using System.Linq;
using Db4objects.Db4o;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.Db4o.CommandHandlers
{
    public class CommandHandlerAddRecord : ICommandHandler<CommandAddRecord>
    {
        readonly string _db4oFilePath;

        public CommandHandlerAddRecord(string db4oFilePath)
        {
            _db4oFilePath = db4oFilePath;
        }

        public void Handle(CommandAddRecord command)
        {
            var config = Db4oEmbedded.NewConfiguration();
            config.Common.UpdateDepth = 5;
            using (var db = Db4oEmbedded.OpenFile(config, _db4oFilePath))
            {
                var action = db.Query<MetaData>()
                    .OrderByDescending(m => m.Version)
                    .First()
                    .ActionInfo
                    .Where(a => a.ActionName == command.ActionName)
                    .Where(a => a.ControllerName == command.ControllerName)
                    .Where(a => ParameterBasicInfo.AreTheSame(command.ParameterInfo, a.ParameterInfo))
                    .First();
                
                if(action.CallRecords == null)
                    action.CallRecords = new List<DetailRecord>();

                action.CallRecords.Add(new DetailRecord
                {
                    CustomValues = command.CustomValues,
                    UserName = command.UserName,
                    TimeStamp = command.TimeStamp
                });

                db.Store(action);
            }
        }
    }
}