using System;
using System.Collections.Generic;
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
        // TODO: how is this configured (SQL connection string, RabbitMq stuff)?
        // TODO: queue name configured? or should not be a magic string at least?

        static EmpiriCallDbContext Context;


        static void Main(string[] args)
        {
            Context = new EmpiriCallDbContext(new SqlConnection("server=(local);uid=;pwd=;Trusted_Connection=yes;database=EmpiriCallDemoDb"));
            
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("EmpiriCallRawRecord", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("EmpiriCallRawRecord", true, consumer);

                    Console.WriteLine(" [*] Waiting for messages.  To exit press CTRL+C");
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var record = JsonConvert.DeserializeObject<QueueMessage>(message);
                        Console.WriteLine(" [x] Received '{0}...'", message.Substring(0,30));

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
