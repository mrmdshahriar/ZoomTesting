using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZoomTesting.Models;

namespace ZoomTesting.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ZoomMeetingService _service;

    public HomeController(ILogger<HomeController> logger, ZoomMeetingService service)
    {
        _logger =  logger;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        UserToken token = await _service.GetAccessToken(null!);

        var access_token = token.access_token;
        var token_type = token.token_type;
        var scope = token.scope;
        var expires_in = token.expires_in;

        Request? request = new Request();
        request.Token = token.access_token;
        request.MeetingRequest = new();


        var mr = await _service.CreateMeeting(request);

        //var data = dashboardDAL.InsertTELS154(mr.id, mr.topic, mr.join_url, request.MeetingRequest.start_time, mr.password, request.MeetingRequest.duration, HttpContext.Session.GetString("EMP_CODE"));



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

/*
        static readonly string _apiKey = "3Bj209jIQLqbmiD1xV_w5w";
        static readonly string _apiSecret = "QhOiJjHYzZ4p3YQx1yweKtZL9kyGDptTeDog";

        static readonly string ZOOM_OAUTH_ENDPOINT = "https://zoom.us/oauth/token";
        static readonly string ZOOM_API_BASE_URL = "https://api.zoom.us/v2";

        static readonly string ZOOM_ACCOUNT_ID = "BoMQjc7jQAuA8DLJrjyFLQ";
        static readonly string ZOOM_CLIENT_ID = "dvjIuLMTdyb4_pFQoFvQQ";
        static readonly string ZOOM_CLIENT_SECRET = "wco3GnCK2TibJ4KPG5Giba5n0y0U6DeR";
*/