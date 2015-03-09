using System;
using System.Linq;
using System.Web.Mvc;
using ExampleMvcApp.DataLayer;
using ExampleMvcApp.Models;

namespace ExampleMvcApp.Controllers
{
    public class OtherController : Controller
    {
        readonly MyDbContext _context;

        public OtherController(MyDbContext context)
        {
            _context = context;
        }

        public ActionResult Bar(Guid? id)
        {
            ViewData["id"] = id.HasValue ? id.ToString() : "N/A";
            return View();
        }

        [HttpPost]
        public ActionResult Baz(BazPostModel model)
        {
            var blogPost = new BlogPost();
            blogPost.Title = model.Keyword;
            blogPost.Body = Guid.NewGuid().ToString();
            blogPost.Author = _context.Authors.OrderBy(x => Guid.NewGuid()).First();
            _context.BlogPosts.Add(blogPost);
            _context.SaveChanges();

            return View(model);
        }
    }
}