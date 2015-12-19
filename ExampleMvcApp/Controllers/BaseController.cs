using System.Web.Mvc;

namespace ExampleMvcApp.Controllers
{
	public class BaseController : Controller
	{
		// this property should not be picked up for meta data
		// since it is not an MVC action
		public string Test { get; set; }
	}
}