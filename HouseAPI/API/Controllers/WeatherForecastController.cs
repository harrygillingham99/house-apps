namespace House.API.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using HLL.Dashboard.WeatherFeed.Interfaces;
    using HLL.Dashboard.WeatherFeed.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private readonly IWeatherProvider _weatherProvider;

        public WeatherForecastController(IWeatherProvider weatherProvider)
        {
            _weatherProvider = weatherProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(OpenWeatherCurrent), (int)HttpStatusCode.OK)]
        public Task<IActionResult> Get()
        {
            return ExecuteAndMapToActionResult(() => _weatherProvider.Get());
        }
    }
}
