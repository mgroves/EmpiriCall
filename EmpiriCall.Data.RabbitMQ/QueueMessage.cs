using System;
using System.Collections.Generic;
using EmpiriCall.Data.Data;

namespace EmpiriCall.Data.RabbitMQ
{
    public class QueueMessage
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public List<ParameterBasicInfo> ParameterInfo { get; set; }
        public List<CustomValue> CustomValues { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}