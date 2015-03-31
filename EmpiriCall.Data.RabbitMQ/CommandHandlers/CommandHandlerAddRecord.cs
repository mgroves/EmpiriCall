using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess;
using EmpiriCall.Data.DataAccess.CommandQueries;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EmpiriCall.Data.RabbitMQ.CommandHandlers
{
    public class CommandHandlerAddRecord : ICommandHandler<CommandAddRecord>
    {
        

        public CommandHandlerAddRecord()
        {
            // TODO: What should be passed into this object? a channel? a connection?
            // TODO: What should the user be configuring? a channel? a connection?
        }

        public void Handle(CommandAddRecord command)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("EmpiriCallRawRecord", false, false, false, null);

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

                    channel.BasicPublish("", "EmpiriCallRawRecord", null, recordBytes);
                }
            }
        }
    }
}