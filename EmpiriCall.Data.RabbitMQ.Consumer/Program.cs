using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using EasyNetQ;
using EmpiriCall.Data.Data;
using Newtonsoft.Json;

namespace EmpiriCall.Data.RabbitMQ.Consumer
{
    class Program
    {
        static EmpiriCallDbContext _context;

        // TODO: convert this to a service/console app
        // http://stackoverflow.com/a/15493790/40015

        static void Main(string[] args)
        {
            var sqlConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
            var rabbitMqHostName = ConfigurationManager.AppSettings["RabbitMqHostName"];

            _context = new EmpiriCallDbContext(new SqlConnection(sqlConnectionString));

            using (var bus = RabbitHutch.CreateBus("host=" + rabbitMqHostName))
            {
                bus.Subscribe<QueueMessage>("EmpiriCallQueueMessage", SaveRecordToDatabase);
                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                Console.ReadLine();
            }
        }

        static void SaveRecordToDatabase(QueueMessage record)
        {
            var metaData = _context.MetaData
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
            _context.SaveChanges();
            Console.WriteLine("Wrote a message:" + JsonConvert.SerializeObject(record));
        }
    }
}
