using System;
using System.Web.Mvc;
using ExampleMvcApp.Models;

namespace ExampleMvcApp.Controllers
{
    public class OtherController : Controller
    {
        public ActionResult Bar(Guid? id)
        {
            ViewData["id"] = id.HasValue ? id.ToString() : "N/A";
            return View();
        }

        [HttpPost]
        public ActionResult Baz(BazPostModel model)
        {
            return View(model);
        }
    }
}