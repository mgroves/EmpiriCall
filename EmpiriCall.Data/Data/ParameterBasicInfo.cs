using System.Collections.Generic;
using System.Linq;

namespace EmpiriCall.Data.Data
{
    public class ParameterBasicInfo
    {
        public int Id { get; set; }
        public string ParameterTypeFullName { get; set; }
        public string ParameterName { get; set; }

        public static bool AreTheSame(List<ParameterBasicInfo> plist1, List<ParameterBasicInfo> plist2)
        {
            if (plist1.Count != plist2.Count)
                return false;

            if(plist1.Count == 0 && plist2.Count == 0)
                return true;

            return plist1.All(p1 => plist2.Any(p2 => p1.ParameterName == p2.ParameterName && p1.ParameterTypeFullName == p2.ParameterTypeFullName));
        }
    }
}