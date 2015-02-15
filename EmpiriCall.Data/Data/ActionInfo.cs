using System.Collections.Generic;

namespace EmpiriCall.Data.Data
{
    public class ActionInfo
    {
        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set;}
        public virtual List<ParameterBasicInfo> ParameterInfo { get; set; }
        public string Feature { get; set; }
        public virtual List<DetailRecord> CallRecords { get; set; }
        public virtual MetaData MetaData { get; set; }
    }
}