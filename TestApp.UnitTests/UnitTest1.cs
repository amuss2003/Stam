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
            var controller = new WeatherForecastController(null) { TotalCount = 10 };
            var weatherData = controller.Get();
            Assert.IsTrue(weatherData.Count() == controller.TotalCount);
        }
    }
}