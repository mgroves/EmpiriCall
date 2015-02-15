using System.Linq;
using Db4objects.Db4o;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.Db4o.CommandHandlers
{
    public class CommandHandlerCreateMetaDataIfNecessary : ICommandHandler<CommandCreateMetaDataIfNecessary>
    {
        readonly string _db4oFilePath;

        public CommandHandlerCreateMetaDataIfNecessary(string db4oFilePath)
        {
            _db4oFilePath = db4oFilePath;
        }

        public void Handle(CommandCreateMetaDataIfNecessary command)
        {
            using (var db = Db4oEmbedded.OpenFile(_db4oFilePath))
            {
                var latestMeta = db.Query<MetaData>().OrderByDescending(m => m.Version).FirstOrDefault();
                if (latestMeta != null)
                    return;
            }

            var addCommand = new CommandHandlerAddNewMetaDataVersion(_db4oFilePath);
            addCommand.Handle(new CommandAddNewMetaDataVersion());
        }
    }
}