using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OcelogApiGateway.Web.Models;
using OcelogApiGateway.Web.Options;
using System.Diagnostics;
using System.Net.WebSockets;

namespace OcelogApiGateway.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiConfigOptions _config;
        public HomeController(ILogger<HomeController> logger, IOptions<ApiConfigOptions> options)
        {
            _logger = logger;
            _config = options.Value;
        }

        public async Task<IActionResult> Index()
        {
            var url = $"{_config.BaseUrl}/ws/Websockets";

            var token = _config.Token;

            //ViewBag["WSInfo"] = url;
            //ViewBag["WSConnectedtate"] = null;

            var ws = new ClientWebSocket();
            
            await ws.ConnectAsync(new Uri($"{url}?access_token={token}"), CancellationToken.None);

            //ViewBag["WSConnectedtate"] = ws.State;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}