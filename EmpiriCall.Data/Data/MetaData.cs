using System;
using System.Collections.Generic;

namespace EmpiriCall.Data.Data
{
    public class MetaData
    {
        public int Id { get; set; }
        public DateTime LastUpdated { get; set; }
        public virtual List<ActionInfo> ActionInfo { get; set; }
    }
}