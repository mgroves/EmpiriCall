using System.Text;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EmpiriCall.Data.RabbitMQ.CommandHandlers
{
    public class CommandHandlerAddRecord : ICommandHandler<CommandAddRecord>
    {
        // "EmpiriCallRawRecord"
        readonly string _rabbitMqHostName;
        readonly string _rabbitMqQueueName;

        public CommandHandlerAddRecord(string rabbitMqHostName, string rabbitMqQueueName)
        {
            _rabbitMqHostName = rabbitMqHostName;
            _rabbitMqQueueName = rabbitMqQueueName;
        }

        public void Handle(CommandAddRecord command)
        {
            var factory = new ConnectionFactory() { HostName = _rabbitMqHostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(_rabbitMqQueueName, false, false, false, null);

                    var record = new QueueMessage
                    {
                        ActionName = command.ActionName,
                        ControllerName = command.ControllerName,
                        ParameterInfo = command.ParameterInfo,
                        CustomValues = command.CustomValues,
                        UserName = command.UserName,
                        TimeStamp = command.TimeStamp
                    };
                    var recordJson = JsonConvert.SerializeObject(record);
                    var recordBytes = Encoding.UTF8.GetBytes(recordJson);

                    channel.BasicPublish("", _rabbitMqQueueName, null, recordBytes);
                }
            }
        }
    }
}