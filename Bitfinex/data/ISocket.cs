using TestHQ;

namespace Bitfinex.data;

public interface ISocket
{
    event Action<Trade> NewBuyTrade;
    event Action<Trade> NewSellTrade;
    void SubscribeTrades(string pair, int maxCount = 100);
    void UnsubscribeTrades(string pair);

    event Action<Candle> CandleSeriesProcessing;
    void SubscribeCandles(string pair, int periodInSec, long? count, DateTimeOffset? from = null, DateTimeOffset? to = null);
    void UnsubscribeCandles(string pair);
    
    Task ConnectAsync();
}