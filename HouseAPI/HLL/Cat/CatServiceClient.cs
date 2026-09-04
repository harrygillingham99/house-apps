using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using House.HLL.Cat.Interfaces;
using House.HLL.Helpers;
using House.Objects;
using LazyCache;
using Microsoft.Extensions.Options;
using RestSharp;

namespace House.HLL.Cat
{
    public class CatServiceClient : ICatServiceClient
    {
        private readonly RestClient _catClient;
        private readonly Random _random;
        private readonly string _getCatUrlStem;
        private readonly IAppCache _cache;

        public CatServiceClient(IOptions<ConnectionStrings> connectionStrings, IAppCache cache)
        {
            _cache = cache;
            _random = new Random();
            _catClient = new RestClient(connectionStrings.Value.CatTags);
            _getCatUrlStem = connectionStrings.Value.GetCat;
        }

        public async Task<string> GetRandomCatUrl()
        {
            var tags = (await Tags()).Where(tag => !string.IsNullOrWhiteSpace(tag)).ToList();
            var randomTag = tags.GetRandomItemFromList(_random);

            return string.Format(_getCatUrlStem, randomTag);
        }

        private Task<List<string>> Tags()
        {
            return _cache.GetOrAddAsync($"{GetType().FullName}_tags", () =>
            {
                var tagsRequest = new RestRequest();
                return _catClient.GetAsync<List<string>>(tagsRequest);
            }, DateTimeOffset.Now.AddHours(1));
        }
    }
}
