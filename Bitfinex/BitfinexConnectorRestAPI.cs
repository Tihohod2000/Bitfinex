using ConnectorTest;
using TestHQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;

namespace Bitfinex;

public class BitfinexConnectorRestAPI : ITestConnector
{
    private readonly RestClient _client;
    
    public BitfinexConnectorRestAPI()
    {
        var options = new RestClientOptions("https://api-pub.bitfinex.com/v2/");
        _client = new RestClient(options);
    }
    
    public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
    {
        var request = new RestRequest($"trades/t{pair}/hist?limit={maxCount}&sort=-1");
        request.AddHeader("accept", "application/json");

        var response = await _client.GetAsync(request);
        if (!response.IsSuccessful || response.Content == null)
        {
            throw new Exception("Error request " + response.ErrorException);
        }
        
        var trades = JsonSerializer.Deserialize<List<List<object>>>(response.Content);

        return trades.Select(t => new Trade
        {
            Id = Convert.ToInt64(t[0]),
            Pair = pair,
            Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(t[1])),
            Amount = Convert.ToDecimal(t[2]),
            Price = Convert.ToDecimal(t[3])
        });
    }

    public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count)
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

    public void SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null,
        long? count)
    {
        throw new NotImplementedException();
    }

    public void UnsubscribeCandles(string pair)
    {
        throw new NotImplementedException();
    }
}