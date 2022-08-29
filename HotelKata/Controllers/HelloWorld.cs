using Microsoft.AspNetCore.Mvc;

namespace HotelKata.Controllers;

[ApiController]
[Route("hello-world")]
public class HelloWorld : ControllerBase
{
    private readonly ILogger<HelloWorld> _logger;

    public HelloWorld(ILogger<HelloWorld> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetHelloWorld")]
    public string Get()
    {
        return "HelloWorld";
    }
}