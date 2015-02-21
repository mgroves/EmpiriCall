using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using RazorEngine.Templating;

namespace EmpiriCall
{
    public class RazorCustomTemplateBase<T> : TemplateBase<T>
    {
        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string Css(string filename)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<style type=\"text/css\">");
            sb.AppendLine(ResourceHelper.TextOfResource(filename));
            sb.AppendLine("</style>");
            return sb.ToString();
        }

        public string Js(string filename)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine(ResourceHelper.TextOfResource(filename));
            sb.AppendLine("</script>");
            return sb.ToString();
        }
    }
}
