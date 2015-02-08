using RazorEngine.Templating;

namespace EmpiriCall
{
    public static class RazorExtensions
    {
        public static string View<T>(this IRazorEngineService @this, string templateName, T viewModel)
        {
            return @this.RunCompile(templateName, typeof (T), viewModel);
        }
    }
}