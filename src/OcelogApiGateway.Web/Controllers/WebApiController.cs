using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OcelogApiGateway.Web.Options;

namespace OcelogApiGateway.Web.Controllers
{
    public class WebApiController : Controller
    {
        private readonly ILogger<WebApiController> _logger;
        private readonly IOptions<ApiConfigOptions> _config;

        public WebApiController(ILogger<WebApiController> logger, IOptions<ApiConfigOptions> config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Connect()
        {
            return View();
        }
    }
}
