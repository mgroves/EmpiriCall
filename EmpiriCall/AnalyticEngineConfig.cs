using System;
using System.Collections.Generic;
using EmpiriCall.Data.Data;

namespace EmpiriCall
{
    public class AnalyticEngineConfig
    {
        public Func<string> GetUserName { get; set; }
        public Func<List<CustomValue>> GetCustomValues { get; set; }
    }
}