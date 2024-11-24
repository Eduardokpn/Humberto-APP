using Microsoft.AspNetCore.Mvc;

namespace HumbertoMVC.Controllers;

public class gambi : Controller
{
    
    public class LocationController : Controller
    {

        [Route("indexx")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("Views/Home/Index.cshtml");
        }
    }
}