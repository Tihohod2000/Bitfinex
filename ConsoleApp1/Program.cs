using Bitfinex;
using Bitfinex.data;
using ConnectorTest;

var restApi = new RestApi(); 
// var restApi = new RestApi("https://api-pub.bitfinex.com/v2/"); 
var socket = new Socket();
ITestConnector connector = new BitfinexConnector(restApi, socket);

var response_trade = await connector.GetNewTradesAsync("tBTCUSD", 24);
Console.WriteLine(response_trade);


DateTimeOffset? from = DateTimeOffset.UtcNow.AddHours(-1);
DateTimeOffset? to = DateTimeOffset.UtcNow.AddHours(2);

var responseCandle = await connector.GetCandleSeriesAsync("tBTCUSD", 60, from, 13, to );
Console.WriteLine(responseCandle);


// var responseWallet = await connector.calc_wallet(10, 10, 10, 10);
// Console.WriteLine(responseWallet);
//
//
// await connector.ConnectAsync();
//
// socket.NewBuyTrade += trade =>
// {
//      Console.WriteLine($"Время: {trade.Time}");
//      Console.WriteLine($"Id: {trade.Id}");
//      Console.WriteLine($"Объём: {trade.Amount}");
//      Console.WriteLine($"Цена: {trade.Price}");
//      Console.WriteLine($"Направление: {trade.Side}");
// };
//
// socket.NewSellTrade += trade =>
// {
//      Console.WriteLine($"Время: {trade.Time}");
//      Console.WriteLine($"Id: {trade.Id}");
//      Console.WriteLine($"Объём: {trade.Amount}");
//      Console.WriteLine($"Цена: {trade.Price}");
//      Console.WriteLine($"Направление: {trade.Side}");
// };
//
// connector.SubscribeTrades("tBTCUSD", 101);
//
//
// await Task.Delay(20 * 1000);
//
// connector.UnsubscribeTrades("tBTCUSD");
//
// connector.SubscribeCandles("tBTCUSD", 60,  100, from, to);
// await Task.Delay(20 * 1000);
// connector.UnsubscribeCandles("tBTCUSD");





