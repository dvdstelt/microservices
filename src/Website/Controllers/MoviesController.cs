using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class MoviesController : Controller
    {
        [HttpGet("/movies")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("/movies/{movieurl}")]
        public ActionResult Movie(string movieurl)
        {
            // var movie = db.Query<Movie>()
            //                 .Where(s => s.UrlTitle == movieurl)
            //                 .SingleOrDefault();
            //
            // var vm = new MovieViewModel
            // {
            //     Movie = movie,
            //     Theaters = TheatersContext.GetTheaters()
            // };

            return View();
        }

        [HttpPost]
        public string Movie(IFormCollection collection)
        {
            var sb = new StringBuilder();
            foreach (var item in collection)
            {
                sb.Append(item + "|");
            }

            return sb.ToString();
        }
    }
}