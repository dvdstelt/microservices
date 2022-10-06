using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    public class ReviewsController : Controller
    {
        [HttpGet("/reviews/{movieurl}")]
        public ActionResult Movie(string movieurl)
        {
            return View();
        }
    }
}