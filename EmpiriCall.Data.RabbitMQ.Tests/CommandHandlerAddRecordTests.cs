using System;
using System.Collections.Generic;
using EmpiriCall.Data.Data;
using EmpiriCall.Data.DataAccess.CommandQueries;
using EmpiriCall.Data.RabbitMQ.CommandHandlers;
using NUnit.Framework;

namespace EmpiriCall.Data.RabbitMQ.Tests
{
    [TestFixture]
    public class CommandHandlerAddRecordTests
    {
        CommandHandlerAddRecord CommandHandlerAddRecord;

        [SetUp]
        public void Setup()
        {
            CommandHandlerAddRecord = new CommandHandlerAddRecord();
        }

        [Test]
        public void HitRabbit()
        {
            // setup a command
            var command = new CommandAddRecord
            {
                ActionName = "FooAction",
                ControllerName = "FooController",
                CustomValues = new List<CustomValue>
                {
                    new CustomValue
                    {
                        Key = "FooKey",
                        Value = "ValueKey"
                    }
                },
                ParameterInfo = new List<ParameterBasicInfo>
                {
                    new ParameterBasicInfo
                    {
                        ParameterName = "FooParameter",
                        ParameterTypeFullName = "FooNamespace.FooParameter"
                    },
                    new ParameterBasicInfo
                    {
                        ParameterName = "BarParameter",
                        ParameterTypeFullName = "BarNamespace.BarParameter"
                    }
                },
                TimeStamp = DateTime.Now,
                UserName = "FooUsername"
            };

            // send it through the handler
            CommandHandlerAddRecord.Handle(command);

            // it should be in the queue now
        }
    }
}