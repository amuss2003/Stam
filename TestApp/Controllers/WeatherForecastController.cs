using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public int TotalCount { get; private set; }

        private readonly ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect("redis-master.redis:6379,password=Fke53qee");

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            IDatabase conn = muxer.GetDatabase();
            var cachedTotalCount = conn.StringGet("totalcount");
            TotalCount = Convert.ToInt32(cachedTotalCount.HasValue ? cachedTotalCount.ToString() : Environment.GetEnvironmentVariable("TotalForecasts") ?? "20");
            return Enumerable.Range(1, TotalCount).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 50),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            
        }
    }
}
