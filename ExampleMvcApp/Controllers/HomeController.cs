using System;
using System.Linq;
using System.Web.Mvc;
using ExampleMvcApp.DataLayer;

namespace ExampleMvcApp.Controllers
{
    public class HomeController : BaseController
    {
        readonly MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var author = new Author();
            author.Name = "SomeAuthor" + Guid.NewGuid();
            _context.Authors.Add(author);
            _context.SaveChanges();

            ViewData["numauthors"] = _context.Authors.Count();

            return View();
        }

        public ActionResult Foo(int a, int? b)
        {
            ViewData["a"] = a;
            ViewData["b"] = b.HasValue ? b.ToString() : "N/A";
            ViewData["numauthors"] = _context.Authors.Count();
            return View("Index");
        }
    }
}