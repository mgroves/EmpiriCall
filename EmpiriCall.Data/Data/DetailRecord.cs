using System;
using System.Collections.Generic;

namespace EmpiriCall.Data.Data
{
    public class DetailRecord
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserName { get; set; }
        public virtual List<CustomValue> CustomValues { get; set; }
        public virtual ActionInfo ActionInfo { get; set; }
    }
}