using Microsoft.AspNetCore.Mvc;
using OcelogApiGateway.Web.Models;
using System.Diagnostics;
using System.Net.WebSockets;

namespace OcelogApiGateway.Web.Controllers;

public sealed class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;

    public HomeController(/*ILogger<HomeController> logger*/)
    {
        //_logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        //_logger.LogInformation("Test log");

        var url = $"wss://localhost:7280/api/ws/Websockets";

        var token = "eyJraWQiOiJTR2RkakZsNFkwQ2FYblFLS0VUNHY4Y1RJdm9GYWQ5SkNPV0ZRXC9HUDU5OD0iLCJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJlOWQyODc3ZS1jNjE5LTQzZTMtYmM1My05MTk5NDlhM2I5Y2UiLCJjb2duaXRvOmdyb3VwcyI6WyJTdXBlckFkbWluIl0sImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJpc3MiOiJodHRwczpcL1wvY29nbml0by1pZHAuYXAtc291dGgtMS5hbWF6b25hd3MuY29tXC9hcC1zb3V0aC0xX0JUeThGQTZjSCIsImNvZ25pdG86dXNlcm5hbWUiOiJhZG1pbiIsIm9yaWdpbl9qdGkiOiI5MDkwNmEyYi0xNDAyLTQyZDYtYWE3Zi00ODQ4NGFkNWYwYTAiLCJhdWQiOiIyanVscTRqMzVtYmJvNzhsbnBrY2lwM2c1ciIsImV2ZW50X2lkIjoiYzcyYjk4MGQtYjJmYS00NGQwLTg4MTctMTc5NWI3ZDNhODMwIiwidG9rZW5fdXNlIjoiaWQiLCJhdXRoX3RpbWUiOjE2OTc3OTY1NjksImV4cCI6MTY5Nzg4Mjk2OSwiaWF0IjoxNjk3Nzk2NTY5LCJqdGkiOiI1NmE4YWQ5ZC1lZDE3LTRiOGUtODZjMi0zYjBiNDBmMGY4NmIiLCJlbWFpbCI6ImFkbWluQHNvbWV0aGluZy5jb20ifQ.aDbMNf7vpxFtirSsEoMhzPi-XZAcN03uS3B_nvYC1IglBvq8CtNl94KviLhOZNAEADrEYHohtwC13nztNBX9Xgou5pZY0r8IQpYEXMAfa2xr5eT4Okkxn_jplNgti1wd6o7mppAvyT4iNBfjA5ogB5sgoMobUJW8Bf66YPTlgsvndotS2-zwfsROpPde3C8N5EZsdU29K4ZOf8_17mTo32Av7iwp7-bpPmO4yiyZ9mImQ0TOvusHK1kkGcQ-U68_rWd1hV78LlvNLVobY-xkrmlbBWZ4Oyn-xKVTHMU_Gf32MT7lZJr06YWnLoqPWR41m8X1H61axIhpNxv8c6u3rA";

        //ViewBag["WSInfo"] = url;
        //ViewBag["WSConnectedtate"] = null;

        var ws = new ClientWebSocket();

        await ws.ConnectAsync(new Uri($"{url}?access_token={token}"), CancellationToken.None).ConfigureAwait(false);

        //ViewBag["WSConnectedtate"] = ws.State;
        ws.Dispose();

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
