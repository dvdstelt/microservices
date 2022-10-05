using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace EventualConsistencyDemo.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly LiteRepository db;

        public ReviewsController(LiteRepository db)
        {
            this.db = db;
        }

        // GET: Reviews
        public ActionResult Index()
        {
            return View();
            // return View(db.Query<Movie>().ToList());
        }

        // GET: Reviews/gameofthrones
        public ActionResult Movie(string movieurl)
        {
            // var vm = new ReviewViewModel();
            //
            // vm.Movie = db.Query<Movie>().Where(s => s.UrlTitle == movieurl).Single();
            // vm.Reviews = db.Query<Review>().Where(s => s.MovieIdentifier == vm.Movie.Id).ToEnumerable();

            return View();
        }
    }
}