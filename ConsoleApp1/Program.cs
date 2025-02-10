using Bitfinex;
using Bitfinex.data;

var restApi = new RestApi(); 
var socket = new Socket();
var connector = new BitfinexConnector(restApi, socket);

var response_trade = await connector.GetNewTradesAsync("tBTCUSD", 24);
Console.WriteLine(response_trade);


DateTimeOffset? from = DateTimeOffset.UtcNow.AddHours(-1);
DateTimeOffset? to = DateTimeOffset.UtcNow.AddHours(2);

var response_candle = await connector.GetCandleSeriesAsync("tBTCUSD", 60, from, 13, to );
Console.WriteLine(response_candle);

await connector.ConnectAsync();

connector.SubscribeTrades("tBTCUSD", 101);
await Task.Delay(10 * 1000);
connector.UnsubscribeTrades("tBTCUSD");





