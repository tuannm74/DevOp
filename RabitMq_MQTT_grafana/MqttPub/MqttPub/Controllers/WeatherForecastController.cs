using DeviceGateway.MessageQueue.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MqttPub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        protected IMqttPublisher mqttPublisher;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMqttPublisher mqttPublisher)
        {
            _logger = logger;
            this.mqttPublisher = mqttPublisher;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("Publish")]
        public async Task  Get2s( )
        {
            Baterrys baterrys = new Baterrys();
            baterrys.Baterry = 80;
            string payload = JsonConvert.SerializeObject(baterrys);
            string topic = "devops/data";
            await mqttPublisher.PublishMessageAsync(topic, payload, false);

        }
    }
}