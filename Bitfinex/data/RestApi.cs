using System.Text.Json;
using RestSharp;
using TestHQ;

namespace Bitfinex.data;

public class RestApi: IRestApi
{
    private readonly RestClient _client;
    
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

        if (trades != null)
        {
            return trades.Select(t => new Trade
            {
                Id = Convert.ToInt64(t[0]),
                Pair = pair,
                Time = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(t[1])),
                Side = Convert.ToInt64(t[2]) > 0 ? "Buy" : "Sell",
                Amount = Convert.ToDecimal(t[2]),
                Price = Convert.ToDecimal(t[3])
            });
        }

        
        throw new Exception("Error data " + trades);


    }

    public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count, DateTimeOffset? to = null)
    {
        throw new NotImplementedException();
    }
}