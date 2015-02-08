using System.Collections.Generic;

namespace EmpiriCall.Data.Data
{
    public class CallDataRecord
    {
        public ActionInfo ActionInfo { get; set; }
        public List<DetailRecord> DetailRecords { get; set; }
    }
}