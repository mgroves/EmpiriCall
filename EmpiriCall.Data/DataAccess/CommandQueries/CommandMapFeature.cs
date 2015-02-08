using System.Collections.Generic;
using EmpiriCall.Data.Data;

namespace EmpiriCall.Data.DataAccess.CommandQueries
{
    public class CommandMapFeature : ICommand
    {
        public string FeatureName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public virtual List<ParameterBasicInfo> ParameterBasicInfos { get; set; }
    }
}