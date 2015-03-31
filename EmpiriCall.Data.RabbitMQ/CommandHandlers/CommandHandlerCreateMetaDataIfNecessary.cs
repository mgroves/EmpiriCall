using System.Linq;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.RabbitMQ.CommandHandlers
{
    public class CommandHandlerCreateMetaDataIfNecessary : ICommandHandler<CommandCreateMetaDataIfNecessary>
    {
        readonly EmpiriCallDbContext _context;

        public CommandHandlerCreateMetaDataIfNecessary(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public void Handle(CommandCreateMetaDataIfNecessary command)
        {
            var latestMeta = _context.MetaData.OrderByDescending(m => m.Version).FirstOrDefault();
            if (latestMeta != null)
                return;

            var addCommand = new CommandHandlerAddNewMetaDataVersion(_context);
            addCommand.Handle(new CommandAddNewMetaDataVersion());
        }
    }
}