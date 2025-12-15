using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceBricks;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        IApiClient<ExampleDto> _apiClient;
        public HomeController(IApiClient<ExampleDto> apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            return View(model);
        }


        [Route("Test")]
        public IActionResult Test()
        {

            var respCreate = _apiClient.Create(new ExampleDto()
            {
                Name = "test"
            });

            

            var respQuery = _apiClient.Query(new ServiceQuery.ServiceQueryRequest());

            respCreate.Item.Name = "test2";
            var respUpdate = _apiClient.Update(respCreate.Item);

            var respDelete = _apiClient.Delete(respCreate.Item.StorageKey);

            HomeViewModel model = new HomeViewModel();
            return View("Index", model);
        }


        [HttpGet]
        [Route("Error")]
        public IActionResult Error(string message = null)
        {
            var model = new ErrorViewModel()
            {
                Message = message
            };
            return View("Error", model);
        }
    }
}