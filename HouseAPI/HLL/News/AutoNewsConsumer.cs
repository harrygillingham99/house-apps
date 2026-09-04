using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using House.HLL.News.Interfaces;
using House.HLL.News.Models;
using House.Objects;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RestSharp;
using static House.HLL.Retry.Retry;

namespace House.HLL.News
{
    public class AutoNewsConsumer : IAutoNewsConsumer
    {
        private readonly IAppCache _cache;
        private readonly IRestClient _newsClient;

        public AutoNewsConsumer(IOptions<NewsApi> options, IOptions<ConnectionStrings> connectionStrings,
            IAppCache cache)
        {
            _newsClient = new RestClient($"{connectionStrings.Value.NewsApi}{options.Value.Key}");
            _cache = cache;
        }

        public async Task<List<NewsMessage>> CurrentNews()
        {
            return (await GetNewsData()).Articles.Select(x => new NewsMessage
            {
                Message = x.Title,
                CreatedBy = x.Source.Name,
                PublishedAt = x.PublishedAt.DateTime
            }).ToList();
        }

        private Task<NewsRoot> GetNewsData()
        {
            return _cache.GetOrAddAsync($"{GetType().FullName}_news_data", () => DoAsync(async () =>
            {
                var request = new RestRequest { Method = Method.Get };
                var result = _newsClient.ExecuteAsync<NewsRoot>(request);
                return (await result)?.Data ?? new NewsRoot();
            }, TimeSpan.FromSeconds(1)), new MemoryCacheEntryOptions {AbsoluteExpiration = DateTime.Now.AddMinutes(5)});
        }
    }
}