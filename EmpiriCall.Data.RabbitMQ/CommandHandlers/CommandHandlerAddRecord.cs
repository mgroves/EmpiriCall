using EasyNetQ;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;

namespace EmpiriCall.Data.RabbitMQ.CommandHandlers
{
    public class CommandHandlerAddRecord : ICommandHandler<CommandAddRecord>
    {
        readonly string _rabbitMqHostName;

        public CommandHandlerAddRecord(string rabbitMqHostName)
        {
            _rabbitMqHostName = rabbitMqHostName;
        }

        public void Handle(CommandAddRecord command)
        {
            var record = new QueueMessage
            {
                ActionName = command.ActionName,
                ControllerName = command.ControllerName,
                ParameterInfo = command.ParameterInfo,
                CustomValues = command.CustomValues,
                UserName = command.UserName,
                TimeStamp = command.TimeStamp
            };

            var bus = RabbitHutch.CreateBus("host=" + _rabbitMqHostName);
            bus.Publish(record);
        }
    }
}