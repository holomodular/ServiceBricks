using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ServiceBricks;

//using ServiceBricks.Security.Api;
using WebApp.ViewModel.Home;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly IBusinessRuleService _businessRuleService;

        //private readonly IUserManagerService _userManagerService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILoggerFactory loggerFactory,
            IBusinessRuleService businessRuleService)
        //IUserManagerService userManagerService)
        {
            _logger = loggerFactory.CreateLogger<HomeController>();
            _businessRuleService = businessRuleService;
            //_userManagerService = userManagerService;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            _logger.LogInformation("HOME");

            // Check if admin users are found
            //var respAdminUsers = await _userManagerService.GetUsersInRoleAsync(SecurityApiConstants.ROLE_ADMIN_NAME);
            //if (respAdminUsers.List == null || respAdminUsers.List.Count == 0)
            //    return LocalRedirect("/RegisterAdmin");

            HomeViewModel model = new HomeViewModel();
            return View(model);
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

        [HttpPost]
        [Route("SetLanguage")]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            var feature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if (feature != null && feature.HasConsent)
            {
                // Consented, use cookie
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

                if (string.IsNullOrEmpty(returnUrl))
                    return LocalRedirect("/");
                else
                    return LocalRedirect(returnUrl);
            }
            else
            {
                // Not consented, use querystring
                if (string.IsNullOrEmpty(returnUrl))
                    return LocalRedirect("/?culture=" + culture);
                else
                {
                    if (returnUrl.Contains('?'))
                        return LocalRedirect(returnUrl + "&culture=" + culture);
                    else
                        return LocalRedirect(returnUrl + "?culture=" + culture);
                }
            }
        }

        [HttpPost]
        [Route("WithdrawCookiesConsent")]
        [ValidateAntiForgeryToken]
        public IActionResult WithdrawCookiesConsent()
        {
            var feature = HttpContext.Features.Get<ITrackingConsentFeature>();
            feature?.WithdrawConsent();

            //Delete all other cookies used in the site
            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

            return LocalRedirect("/");
        }

        //[HttpGet]
        //[Route("RegisterAdmin")]
        //public async Task<IActionResult> RegisterAdmin()
        //{
        //    // Check if admin users are found
        //    var respAdminUsers = await _userManagerService.GetUsersInRoleAsync(SecurityApiConstants.ROLE_ADMIN_NAME);
        //    if (respAdminUsers.List == null || respAdminUsers.List.Count == 0)
        //        return View("RegisterAdmin");
        //    return NotFound();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("RegisterAdmin")]
        //public async Task<IActionResult> RegisterAdmin(RegisterAdminViewModel model)
        //{
        //    // Check if admin users are found
        //    var respAdminUsers = await _userManagerService.GetUsersInRoleAsync(SecurityApiConstants.ROLE_ADMIN_NAME);
        //    if (respAdminUsers.List != null && respAdminUsers.List.Count > 0)
        //        return NotFound();

        //    // Create admin user process
        //    UserRegisterAdminProcess processRegisterAdmin = new UserRegisterAdminProcess(
        //        model.Email,
        //        model.Password);

        //    // Execute the process
        //    var respEvent = await _businessRuleService.ExecuteProcessAsync(processRegisterAdmin);
        //    if (respEvent.Error)
        //    {
        //        ModelState.CopyFromResponse(respEvent);
        //        return View("RegisterAdmin");
        //    }

        //    // Find the user
        //    var respUser = await _userManagerService.FindByEmailAsync(model.Email);
        //    if (respUser.Item != null)
        //    {
        //        //Log in the user automatically
        //        await _userManagerService.SignInAsync(respUser.Item.StorageKey, true);
        //    }

        //    // Go back to home page
        //    return LocalRedirect("/");
        //}
    }
}