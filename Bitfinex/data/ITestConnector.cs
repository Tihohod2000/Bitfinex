using Bitfinex.data;
using TestHQ;

namespace ConnectorTest
{
    public interface ITestConnector
    {
        #region Rest

        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);

        Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count,
            DateTimeOffset? to = null);

        // Добавил метод для расчёта валют пользователя
        Task<Wallet> calc_wallet(int btc, int xrp, int xmr, int dash);

        #endregion

        #region Socket

        event Action<Trade> NewBuyTrade;
        event Action<Trade> NewSellTrade;
        void SubscribeTrades(string pair, int maxCount = 100);
        void UnsubscribeTrades(string pair);

        event Action<Candle> CandleSeriesProcessing;

        void SubscribeCandles(string pair, int periodInSec, long? count, DateTimeOffset? from = null,
            DateTimeOffset? to = null);

        void UnsubscribeCandles(string pair);

        // Добавил метод подключения
        Task ConnectAsync();

        #endregion
    }
}