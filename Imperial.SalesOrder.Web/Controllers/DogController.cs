using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Imperial.SalesOrder.Web.Controllers
{
    public class DogController : Controller
    {
        private readonly DogApiService _dogApiService;
        public DogController()
        {
            _dogApiService = new DogApiService(new HttpClient());
        }
        public ActionResult Index()
        {
            try
            {
                var imageUrl = _dogApiService.GetRandomDogImageUrl();
                Console.WriteLine("Image URL: " + imageUrl);
                ViewBag.ImageUrl = imageUrl;
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Failed to fetch dog image from the API: " + ex.Message;
            }

            return View(); ;
        }
    }
}