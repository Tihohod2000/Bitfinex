namespace Bitfinex.data;
using TestHQ;

public interface IRestApi
{
    Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
    Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count, DateTimeOffset? to = null);

    Task<double> Convector(string currency);

    Task<Wallet> calc_wallet(int btc, int xrp, int xmr, int dash);

}