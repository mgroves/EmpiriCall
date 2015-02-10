using System.Web.Mvc;

namespace ExampleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Foo(int a, int? b)
        {
            ViewData["a"] = a;
            ViewData["b"] = b.HasValue ? b.ToString() : "N/A";
            return View();
        }
    }
}