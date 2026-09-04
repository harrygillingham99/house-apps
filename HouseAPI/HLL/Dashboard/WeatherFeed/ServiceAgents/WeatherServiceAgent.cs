using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using House.HLL.Dashboard.WeatherFeed.Interfaces;
using House.HLL.Dashboard.WeatherFeed.Models;
using House.Objects;
using LazyCache;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace House.HLL.Dashboard.WeatherFeed.ServiceAgents
{
    public class WeatherServiceAgent : IWeatherServiceAgent
    {
        private readonly string _apiKey;
        private readonly IRestClient _weatherClient;
        private readonly IAppCache _cache;

        public WeatherServiceAgent(IOptions<OpenWeatherApi> openWeatherApi, IOptions<ConnectionStrings> connectionStrings, IAppCache cache)
        {
            _cache = cache;
            _apiKey = openWeatherApi.Value.Key;
            _weatherClient = new RestClient(connectionStrings.Value.OpenWeather);
        }

        public Task<OpenWeatherCurrent> Get()
        {
            return _cache.GetOrAddAsync($"{GetType().FullName}_Weather", () =>
            {
                var request = new RestRequest { Method = Method.Get }
                    .AddParameter("q", "Bournemouth")
                    .AddParameter("units", "metric") // important, default is Kelvin
                    .AddParameter("appid", _apiKey);
                return Retry.Retry.DoAsync(() => GetWeatherData(request), TimeSpan.FromSeconds(1));
            }, DateTimeOffset.Now.AddHours(0.5));


        }

        private async Task<OpenWeatherCurrent> GetWeatherData(RestRequest request)
        {
            var result = await _weatherClient.ExecuteAsync(request);
            
            if (result == null)
                throw new NullReferenceException();

            if (result.StatusCode == HttpStatusCode.OK) return JsonConvert.DeserializeObject<OpenWeatherCurrent>(result.Content);

            var msg = $"Unexpected error {(int)result.StatusCode} status code from result";
            Log.Error(msg);
            throw new HttpRequestException(msg);
        }
    }
}
