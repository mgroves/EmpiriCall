using System.IO;
using System.Linq;
using System.Reflection;

namespace EmpiriCall
{
    public static class ResourceHelper
    {
        public static string TextOfResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var name = assembly.GetManifestResourceNames()
                .SingleOrDefault(r => r.ToLower() == "empiricall.templates." + resourceName.ToLower());

            if(name == null)
                throw new FileNotFoundException("No resource found: " + resourceName);

            using (Stream stream = assembly.GetManifestResourceStream(name))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        public static bool ResourceExists(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Any(r => r.ToLower() == ("empiricall.templates." + resourceName.ToLower()));
        }
    }
}