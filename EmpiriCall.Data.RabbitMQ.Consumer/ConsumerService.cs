using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using EasyNetQ;
using EmpiriCall.Data.Data;
using Newtonsoft.Json;

namespace EmpiriCall.Data.RabbitMQ.Consumer
{
    public class ConsumerService : ServiceBase
    {
        EmpiriCallDbContext _context;
        bool _showConsole;

        public void Start(string[] args)
        {
            _showConsole = Environment.UserInteractive;
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            var sqlConnectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
			var rabbitMqConnectionString = ConfigurationManager.AppSettings["RabbitMqConnectionString"];

            _context = new EmpiriCallDbContext(new SqlConnection(sqlConnectionString));

            using (var bus = RabbitHutch.CreateBus(rabbitMqConnectionString))
            {
                bus.Subscribe<QueueMessage>("EmpiriCallQueueMessage", SaveRecordToDatabase);
                if (_showConsole)
                {
                    Console.WriteLine("Now listening for messages of type: " + typeof (QueueMessage).FullName);
                    Console.WriteLine("Press ENTER to stop.");
	                Console.ReadLine();
                }
            }

            base.OnStart(args);
        }

        private void SaveRecordToDatabase(QueueMessage record)
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

            if (_showConsole)
                Console.WriteLine("Wrote a message:" + JsonConvert.SerializeObject(record));
        }

        protected override void OnStop()
        {
            _context.Database.Connection.Close();
            _context.Dispose();
        }
    }
}