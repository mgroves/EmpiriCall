using RazorEngine.Templating;

namespace EmpiriCall
{
    internal static class RazorExtensions
    {
        public static string View<T>(this IRazorEngineService @this, string templateName, T viewModel)
        {
            return @this.RunCompile(templateName, typeof (T), viewModel);
        }
    }
}