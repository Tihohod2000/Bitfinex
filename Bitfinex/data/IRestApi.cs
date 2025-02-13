namespace Bitfinex.data;
using TestHQ;

public interface IRestApi
{
    Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
    Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count, DateTimeOffset? to = null);

    Task<long> Convector(string currency_1, string currency_2);

}