using System.IO;
using System.Reflection;
using System.Text;

namespace EmpiriCall
{
    public static class RazorHelper
    {
        public static string Css(string filename)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<style type=\"text/css\">");
            sb.AppendLine(TextOfResource(filename));
            sb.AppendLine("</style>");
            return sb.ToString();
        }

        public static string Js(string filename)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine(TextOfResource(filename));
            sb.AppendLine("</script>");
            return sb.ToString();
        }

        public static string TextOfResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("EmpiriCall.Templates." + resourceName))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}