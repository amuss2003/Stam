using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using System.Text.Json;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public int TotalCount { get; set; }

        private static readonly Lazy<IDatabase> RedisDb = new Lazy<IDatabase>(() => 
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
            var cachedTotalCount = RedisDb.Value.StringGet("totalcount");
            var count = Convert.ToInt32(cachedTotalCount.HasValue ? cachedTotalCount.ToString() : Environment.GetEnvironmentVariable("TotalForecasts") ?? "20");
            return count;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            if (TotalCount == 0)
                TotalCount = GetTotalCount();
            
            var rng = new Random();
            var forecast = Enumerable.Range(1, TotalCount).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 50),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });

            if (RedisDb.IsValueCreated)
            {
                foreach (var day in forecast)
                    RedisDb.Value.StringSet(day.Date.ToString("yyyy-MM-dd"), JsonSerializer.Serialize(day));

                RedisDb.Value.SortedSetAdd("temps2", forecast.Select(f => new SortedSetEntry(new RedisValue(f.Date.ToString("yyyy-MM-dd")), f.TemperatureC)).ToArray());
            }

            return forecast;
        }
    }
}
