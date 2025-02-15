using System.Text.Json;
using RestSharp;
using TestHQ;

namespace Bitfinex.data;

public class RestApi: IRestApi
{
    private readonly RestClient _client;
    private static readonly Dictionary<int, string> AvailablePeriods = new()
    {
        { 60, "1m" }, { 300, "5m" }, { 900, "15m" }, { 1800, "30m" }, { 3600, "1h" },
        { 10800, "3h" }, { 21600, "6h" }, { 43200, "12h" }, { 86400, "1D" }, { 604800, "1W" },
        { 1209600, "14D" }, { 2592000, "1M" }
    };

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
    
    private static string ConvertPeriod(int periodInSec)
    {
        int closestPeriod = AvailablePeriods.Keys.OrderBy(p => Math.Abs(p - periodInSec)).First();
        return AvailablePeriods[closestPeriod];
    }
    
    

    public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count = 120, DateTimeOffset? to = null)
    {
        // var request = new RestRequest($"trades/t{pair}/hist?limit={maxCount}&sort=-1");
        
        long fromMs = from?.ToUnixTimeMilliseconds() ?? 0;
        long toMs = to?.ToUnixTimeMilliseconds() ?? 0;
        string requestLink;
        string period = ConvertPeriod(periodInSec);

        if (to == null)
        {
            requestLink = $"candles/trade:{period}:{pair}/hist?start={fromMs}&limit={count}";
        }
        else
        {
            requestLink = $"candles/trade:{period}:{pair}/hist?start={fromMs}&end={toMs}&limit={count}";
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
            var candles = doc.RootElement.EnumerateArray()
                .Select(t => new Candle
                {
                    Pair = pair,
                    OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(t[0].GetInt64()),
                    OpenPrice = t[1].GetDecimal(),
                    ClosePrice = t[2].GetDecimal(),
                    HighPrice = t[3].GetDecimal(),
                    LowPrice = t[4].GetDecimal(),
                    TotalVolume = t[5].GetDecimal(),
                    TotalPrice = t[5].GetDecimal() * ((t[1].GetDecimal() + t[2].GetDecimal()) / 2)
                })
                .ToList();

            return candles;
        }
        catch (Exception ex)
        {
            throw new Exception("Error candle data: " + ex.Message);
        }
    }

    
    public async Task<double> Convector(string currency)
    {
        RestResponse response;
        
        try
        {
            var request = new RestRequest($"ticker/t{currency}USD");
            request.AddHeader("accept", "application/json");

             response = await _client.GetAsync(request);
            if (!response.IsSuccessful || response.Content == null)
            {
                throw new Exception("Error request " + response.ErrorException);
            }
        }
        catch (Exception)
        {
            try
            {
                var request = new RestRequest($"ticker/tUST{currency}");
                request.AddHeader("accept", "application/json");

                response = await _client.GetAsync(request);
                if (!response.IsSuccessful || response.Content == null)
                {
                    throw new Exception("Error request " + response.ErrorException);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error CONVECTOR data: " + ex.Message);
            }
        }
        
        
        try
        {
            using var doc = JsonDocument.Parse(response.Content);
            var currencyInt = doc.RootElement[6].GetDecimal();

            return (long)currencyInt;
        }
        catch (Exception ex)
        {
            throw new Exception("Error CONVECTOR data: " + ex.Message);
        }
    }
    
    public async Task<Wallet> calc_wallet(int btc, int xrp, int xmr, int dash)
    {
        var valueWallet = new Dictionary<string, int>
        {
            { "BTC", btc },
            { "XRP", xrp },
            { "XMR", xmr },
            { "DSH", dash }
        };

        double AllCurrenciesInDollars = 0;

        foreach (var x in valueWallet)
        {
            var responseConvector = await Convector(x.Key);  
            AllCurrenciesInDollars += responseConvector * x.Value;
        }

        var btcRatio = await Convector("BTC");
        var xrpRatio = await Convector("XRP");
        var xmrRatio = await Convector("XMR");
        var dashRatio = await Convector("DSH");
        var ustRatio = await Convector("UST");

        var wallet = new Wallet()
        {
            Btc = AllCurrenciesInDollars / btcRatio,
            Xrp = AllCurrenciesInDollars / xrpRatio,
            Xmr = AllCurrenciesInDollars / xmrRatio,
            Dash = AllCurrenciesInDollars / dashRatio,
            Usdt = AllCurrenciesInDollars / ustRatio
        };

        return wallet;
    }
}

