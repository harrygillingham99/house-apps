using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using House.HLL.Dashboard.Bindicator.Interfaces;
using House.HLL.Dashboard.Bindicator.Models;
using House.Objects;
using LazyCache;
using Microsoft.Extensions.Options;
using RestSharp;

namespace House.HLL.Dashboard.Bindicator.ServiceAgents
{
    public class BinLookupServiceAgent : IBinLookupServiceAgent
    {
        private readonly IAppCache _cache;
        private readonly IRestClient _lookupClient;

        public BinLookupServiceAgent(IOptions<ConnectionStrings> connectionStrings, IAppCache cache)
        {
            _cache = cache;
            _lookupClient = new RestClient(connectionStrings.Value.BCPCouncil);
        }

        public async Task<BinLookup> Lookup(string uprn)
        {
            try
            {
                var request = new RestRequest { Method = Method.Get }
                    .AddQueryParameter(nameof(uprn), uprn);
                return await Retry.Retry.DoAsync(() => GetBinData(request), TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"Error occurred in {nameof(BinLookupServiceAgent)}.{nameof(Lookup)}");
                return new BinLookup([]);
            }
        }

        private Task<BinLookup> GetBinData(RestRequest request)
        {
            return _cache.GetOrAddAsync($"{GetType().FullName}_BinLookup", async () =>
            {
                var result = await _lookupClient.GetAsync<List<BinLookupDto>>(request);
                return new BinLookup(result);
            }, DateTimeOffset.Now.AddHours(1));
        }
    }
}