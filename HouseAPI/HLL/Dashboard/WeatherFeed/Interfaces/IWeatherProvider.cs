using System.Threading.Tasks;
using House.HLL.Dashboard.WeatherFeed.Models;

namespace House.HLL.Dashboard.WeatherFeed.Interfaces
{
    public interface IWeatherProvider
    {
        Task<OpenWeatherCurrent> Get();
    }
}
