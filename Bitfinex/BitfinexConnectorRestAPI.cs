using ConnectorTest;
using TestHQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Bitfinex.data;
using RestSharp;

namespace Bitfinex;

public class BitfinexConnector : ITestConnector
{
    private readonly RestClient _client;
    private readonly IRestApi _restapi;
    private readonly ISocket _socket;

    public BitfinexConnector(IRestApi restConnector, ISocket socketConnector)
    {
        _restapi = restConnector ?? throw new ArgumentNullException(nameof(restConnector));
        _socket = socketConnector ?? throw new ArgumentNullException(nameof(socketConnector));
    }


    public Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount) =>
        _restapi.GetNewTradesAsync(pair, maxCount);

    // public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count, DateTimeOffset? to = null) =>
    //     _restapi.GetCandleSeriesAsync(pair, periodInSec, from, count, to);




    // public BitfinexConnector()
    // {
    //     var options = new RestClientOptions("https://api-pub.bitfinex.com/v2/");
    //     _client = new RestClient(options);
    // }
    //
    // public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
    // {
    //     var request = new RestRequest($"trades/t{pair}/hist?limit={maxCount}&sort=-1");
    //     request.AddHeader("accept", "application/json");
    //
    //     var response = await _client.GetAsync(request);
    //     if (!response.IsSuccessful || response.Content == null)
    //     {
    //         throw new Exception("Error request " + response.ErrorException);
    //     }
    //
    //     var trades = JsonSerializer.Deserialize<List<List<object>>>(response.Content);
    //
    //     if (trades != null)
    //     {
    //         return trades.Select(t => new Trade
    //         {
    //             Id = Convert.ToInt64(t[0]),
    //             Pair = pair,
    //             Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(t[1])),
    //             Side = Convert.ToInt64(t[2]) > 0 ? "Buy" : "Sell",
    //             Amount = Convert.ToDecimal(t[2]),
    //             Price = Convert.ToDecimal(t[3])
    //         });
    //     }
    //
    //     
    //     throw new Exception("Error data " + trades);
    //
    //
    // }

    public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from,
        long? count, DateTimeOffset? to = null)
    {
        throw new NotImplementedException();
    }

    public event Action<Trade>? NewBuyTrade;
    public event Action<Trade>? NewSellTrade;
    public void SubscribeTrades(string pair, int maxCount = 100)
    {
        throw new NotImplementedException();
    }

    public void UnsubscribeTrades(string pair)
    {
        throw new NotImplementedException();
    }

    public event Action<Candle>? CandleSeriesProcessing;

    public void SubscribeCandles(string pair, int periodInSec,
        long? count, DateTimeOffset? from = null, DateTimeOffset? to = null)
    {
        throw new NotImplementedException();
    }

    public void UnsubscribeCandles(string pair)
    {
        throw new NotImplementedException();
    }
}