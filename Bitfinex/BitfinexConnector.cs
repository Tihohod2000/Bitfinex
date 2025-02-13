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
    
    private readonly IRestApi _restapi;
    private readonly ISocket _socket;

    public BitfinexConnector(IRestApi restConnector, ISocket socketConnector)
    {
        _restapi = restConnector ?? throw new ArgumentNullException(nameof(restConnector));
        _socket = socketConnector ?? throw new ArgumentNullException(nameof(socketConnector));
    }

    


    public Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount) =>
        _restapi.GetNewTradesAsync(pair, maxCount);

    public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, long? count, DateTimeOffset? to = null) =>
        _restapi.GetCandleSeriesAsync(pair, periodInSec, from, count, to);
    
    public async Task ConnectAsync()
    {
        await _socket.ConnectAsync();
    }
    
    public event Action<Trade>? NewBuyTrade;
    public event Action<Trade>? NewSellTrade;
    public void SubscribeTrades(string pair, int maxCount = 100)
    {
         _socket.SubscribeTrades(pair, maxCount);
    }

    public void UnsubscribeTrades(string pair)
    {
        _socket.UnsubscribeTrades(pair);
    }

    public event Action<Candle>? CandleSeriesProcessing;

    public void SubscribeCandles(string pair, int periodInSec,
        long? count, DateTimeOffset? from = null, DateTimeOffset? to = null)
    {
        _socket.SubscribeCandles(pair, periodInSec, count, from, to);
    }
    



    public void UnsubscribeCandles(string pair)
    {
        _socket.UnsubscribeCandles(pair);
    }
}