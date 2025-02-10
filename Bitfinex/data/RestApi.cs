using System.Text.Json;
using RestSharp;
using TestHQ;

namespace Bitfinex.data;

public class RestApi: IRestApi
{
    private readonly RestClient _client;
    
    public RestApi(string baseUrl = "https://api.bitfinex.com/v2/")
    {
        _client = new RestClient(baseUrl);
    }
    
    public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
    {
        var request = new RestRequest($"trades/{pair}/hist?limit={maxCount}&sort=-1");
        request.AddHeader("accept", "application/json");

        var response = await _client.GetAsync(request);
        if (!response.IsSuccessful || response.Content == null)
        {
            throw new Exception("Error request " + response.ErrorException);
        }

        try
        {
            using var doc = JsonDocument.Parse(response.Content);
            var trades = doc.RootElement.EnumerateArray()
                .Select(t => new Trade
                {
                    Id = t[0].GetInt64(),
                    Pair = pair,
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(t[1].GetInt64()),
                    Side = t[2].GetDecimal() > 0 ? "buy" : "sell",
                    Amount = t[2].GetDecimal(),
                    Price = t[3].GetDecimal()
                })
                .ToList();

            return trades;
        }
        catch (Exception ex)
        {
            throw new Exception("Error trade data: " + ex.Message);
        }


    }

    public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count = 120, DateTimeOffset? to = null)
    {
        // var request = new RestRequest($"trades/t{pair}/hist?limit={maxCount}&sort=-1");
        

        string requestLink;

        if (to == null)
        {
            requestLink = $"candles/trade:{periodInSec/60}m:{pair}:p{from.Value.Day}/hist?limit={count}";
        }
        else
        {
            requestLink = $"candles/trade:{periodInSec/60}m:{pair}:p{from.Value.Day}:p{to.Value.Day}/hist?limit={count}";
        }

        var request = new RestRequest(requestLink);
        request.AddHeader("accept", "application/json");
        
        Console.WriteLine(request.Resource);

        var response = await _client.GetAsync(request);
        if (!response.IsSuccessful || response.Content == null)
        {
            throw new Exception("Error request " + response.ErrorException);
        }

        try
        {
            using var doc = JsonDocument.Parse(response.Content);
            var Candles = doc.RootElement.EnumerateArray()
                .Select(t => new Candle
                {
                    // Id = t[0].GetInt64(),
                    Pair = pair,
                    OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(t[0].GetInt64()),
                    // Side = t[2].GetDecimal() > 0 ? "Buy" : "Sell",
                    OpenPrice = t[1].GetDecimal(),
                    ClosePrice = t[2].GetDecimal(),
                    HighPrice = t[3].GetDecimal(),
                    LowPrice = t[4].GetDecimal(),
                    TotalVolume = t[5].GetDecimal(),
                    TotalPrice = t[5].GetDecimal() * ((t[1].GetDecimal() + t[2].GetDecimal()) / 2)
                })
                .ToList();

            return Candles;
        }
        catch (Exception ex)
        {
            throw new Exception("Error candle data: " + ex.Message);
        }
    }
}