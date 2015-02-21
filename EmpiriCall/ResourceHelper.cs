using System.IO;
using System.Reflection;

namespace EmpiriCall
{
    public static class ResourceHelper
    {
        public static string TextOfResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("EmpiriCall.Templates." + resourceName))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}