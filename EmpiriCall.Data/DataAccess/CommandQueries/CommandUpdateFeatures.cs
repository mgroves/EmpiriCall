using System.Collections.Generic;

namespace EmpiriCall.Data.DataAccess.CommandQueries
{
    public class CommandUpdateFeatures : ICommand
    {
        public Dictionary<int, string> FeatureMap { get; set; } 
    }
}