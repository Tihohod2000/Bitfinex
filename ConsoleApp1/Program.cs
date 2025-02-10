using Bitfinex;
using Bitfinex.data;

var restApi = new RestApi(); 
var socket = new Socket();
var connector = new BitfinexConnector(restApi, socket);

var response_trade = await connector.GetNewTradesAsync("tBTCUSD", 10);
Console.WriteLine(response_trade);


DateTimeOffset? from = DateTimeOffset.UtcNow.AddDays(-20);
DateTimeOffset? to = DateTimeOffset.UtcNow.AddDays(2);

var response_candle = await connector.GetCandleSeriesAsync("tBTCUSD", 60, from, 100 );
Console.WriteLine(response_candle);