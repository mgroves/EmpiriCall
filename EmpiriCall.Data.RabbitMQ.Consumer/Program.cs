using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EmpiriCall.Data.Data;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmpiriCall.Data.RabbitMQ.Consumer
{
    class Program
    {
        static EmpiriCallDbContext Context;
        static ConnectionFactory Factory;

        // TODO: convert this to a service/console app
        // http://stackoverflow.com/a/15493790/40015

        static void Main(string[] args)
        {
            var queueName = ConfigurationManager.AppSettings["RabbitMqQueueName"] ?? "EmpiriCallRawRecord";
            var sqlConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
            var rabbitMqHostName = ConfigurationManager.AppSettings["RabbitMqHostName"];

            Context = new EmpiriCallDbContext(new SqlConnection(sqlConnectionString));
            Factory = new ConnectionFactory() { HostName = rabbitMqHostName };

            StartReadingFromQueue(queueName);
        }

        static void StartReadingFromQueue(string queueName)
        {
            using (var connection = Factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine(" [*] Waiting for messages.  To exit press CTRL+C");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs) consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var record = JsonConvert.DeserializeObject<QueueMessage>(message);
                        Console.WriteLine(" [x] Received '{0}...'", message.Substring(0, 30));

                        SaveRecordToDatabase(record);

                        Console.WriteLine(" [x] Saved to database");
                    }
                }
            }
        }


        static void SaveRecordToDatabase(QueueMessage record)
        {
            var metaData = Context.MetaData
                    .OrderByDescending(m => m.Version)
                    .First();
            
            var action = metaData.ActionInfo
                    .Where(a => a.ActionName == record.ActionName)
                    .Where(a => a.ControllerName == record.ControllerName)
                    .Where(a => ParameterBasicInfo.AreTheSame(record.ParameterInfo, a.ParameterInfo))
                    .FirstOrDefault();

            if (action == null)
                return;
                        
            if (action.CallRecords == null)
                action.CallRecords = new List<DetailRecord>();
            
            action.CallRecords.Add(new DetailRecord
            {
                CustomValues = record.CustomValues,
                UserName = record.UserName,
                TimeStamp = record.TimeStamp
            });
            Context.SaveChanges();
        }
    }
}
