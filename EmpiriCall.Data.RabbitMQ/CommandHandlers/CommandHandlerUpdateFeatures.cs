using System.Linq;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.RabbitMQ.CommandHandlers
{
    public class CommandHandlerUpdateFeatures : ICommandHandler<CommandUpdateFeatures>
    {
        readonly EmpiriCallDbContext _context;

        public CommandHandlerUpdateFeatures(EmpiriCallDbContext context)
        {
            _context = context;
        }

        public void Handle(CommandUpdateFeatures command)
        {
            var allActionInfo = _context.MetaData.SelectMany(a => a.ActionInfo);
            foreach (var kvp in command.FeatureMap)
            {
                var actionInfo = allActionInfo.SingleOrDefault(a => a.Id == kvp.Key);

                if (actionInfo != null)
                {
                    actionInfo.Feature = kvp.Value;
                    _context.SaveChanges();
                }
            }
        }
    }
}