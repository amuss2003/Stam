using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public int TotalCount { get; set; }

        private Lazy<IDatabase> redisDb = new Lazy<IDatabase>(() => 
            ConnectionMultiplexer.Connect("redis-master.redis:6379,password=Fke53qee").GetDatabase());

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        private int GetTotalCount()
        {
            var cachedTotalCount = redisDb.Value.StringGet("totalcount");
            var count = Convert.ToInt32(cachedTotalCount.HasValue ? cachedTotalCount.ToString() : Environment.GetEnvironmentVariable("TotalForecasts") ?? "20");
            return count;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            if (TotalCount == 0)
                TotalCount = GetTotalCount();
            
            var rng = new Random();
            return Enumerable.Range(1, TotalCount).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 50),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }
    }
}
