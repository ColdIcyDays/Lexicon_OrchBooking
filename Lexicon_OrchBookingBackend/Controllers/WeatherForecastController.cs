using Lexicon_OrchBookingBackend.Data;
using Lexicon_OrchBookingBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lexicon_OrchBookingBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private Lexicon_OrchBookingBackendContext? OrchContext = null;
    
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];
    
    
    public WeatherForecastController(Lexicon_OrchBookingBackendContext aContext)
    {
        OrchContext = aContext;
    }
    
    
    

    /*[HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }*/
    
    [HttpGet(Name = "GetSomethingElse")]
    public TestModel Get()
    {
        if (OrchContext != null)
        {
            
        }
        TestModel model = new TestModel();
        model.Id = 5;
        model.SomeText = "HAHAHAHA";
        return model;
    }
}