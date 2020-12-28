using NUnit.Framework;
using System;
using System.Linq;
using TestApp.Controllers;

namespace TestApp.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Console.WriteLine("Test is running");
            var weatherData = new WeatherForecastController(null).Get();
            Assert.IsTrue(weatherData.Count() == WeatherForecastController.TotalCount);
        }
    }
}