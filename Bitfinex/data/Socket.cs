using TestHQ;
using System;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

namespace Bitfinex.data;

public class Socket : ISocket
{
    private ClientWebSocket _webSocket = new ClientWebSocket();
    private Uri _webSocketUri = new Uri("wss://api-pub.bitfinex.com/ws/2");
    
    private readonly Dictionary<string, int> _subscriptions = new();
    
    
    public event Action<Trade>? NewBuyTrade;
    public event Action<Trade>? NewSellTrade;
    public event Action<Candle>? CandleSeriesProcessing;
    
    public async Task ConnectAsync()
    {
        await _webSocket.ConnectAsync(_webSocketUri, CancellationToken.None);
        StartReceiving();
    }
    
    private async void StartReceiving()
    {
        // Получение данных
        var buffer = new byte[8192];

        try
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                var fullMessage = new StringBuilder();
                
                WebSocketReceiveResult result;
                do
                {
                    result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    fullMessage.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                } while (!result.EndOfMessage);


                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = fullMessage.ToString();
                    ProcessMessage(message);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
       
    }
    
    
   private void ProcessMessage(string message)
        {
            Console.WriteLine($"Received message: {message}");
            
            try
            {
                var jsonDocument = JsonDocument.Parse(message);

                try
                {

                    if (jsonDocument.RootElement.ValueKind == JsonValueKind.Object)
                    {

                        if (jsonDocument.RootElement.TryGetProperty("event", out var eventProperty))
                        {

                            // Проверяем событие
                            if (eventProperty.GetString() == "subscribed")
                            {
                                var channel = jsonDocument.RootElement.GetProperty("channel").GetString();
                                string pair;
                                if (channel == "candles")
                                {
                                    var key = jsonDocument.RootElement.GetProperty("key").GetString();
                                    pair = key.Substring(key.LastIndexOf(':') + 1);
                                }
                                else
                                {
                                    pair = jsonDocument.RootElement.GetProperty("symbol").GetString();
                                }
                                
                                var chanId = jsonDocument.RootElement.GetProperty("chanId").GetInt32();
                                _subscriptions[pair] = chanId;
                                Console.WriteLine($"Subscribed to {channel} for pair {pair}");
                                return;
                            }

                            Console.WriteLine(eventProperty);


                            if (eventProperty.GetString() == "info")
                            {
                                // var channel = jsonDocument.RootElement.GetProperty("channel").GetString();
                                var serverId = jsonDocument.RootElement.GetProperty("serverId").GetString();
                                var platform = jsonDocument.RootElement.GetProperty("platform");
                                Console.WriteLine($"Info: serverId {serverId} for platfom {platform}");
                                return;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // throw;
                }
                
                

                if (jsonDocument.RootElement.ValueKind == JsonValueKind.Array &&
                    jsonDocument.RootElement.GetArrayLength() > 2)
                {
                    var eventType = jsonDocument.RootElement[1].ValueKind == JsonValueKind.String
                        ? jsonDocument.RootElement[1].GetString()
                        : null;
                    var data = jsonDocument.RootElement[2];




                    if (eventType == "te" || eventType == "tu")
                    {
                        if (data.ValueKind != JsonValueKind.Array || data.GetArrayLength() < 4)
                        {
                            Console.WriteLine($"Error trade data: {message}");
                            return;
                        }

                        var trade = new Trade
                        {
                            Time = DateTimeOffset.FromUnixTimeMilliseconds(data[0].GetInt64()),
                            Id = data[1].GetInt64(),
                            Amount = data[2].GetDecimal(),
                            Price = data[3].GetDecimal(),
                            Side = data[2].GetDecimal() > 0 ? "buy" : "sell"
                        };

                        if (trade.Side == "buy")
                            NewBuyTrade?.Invoke(trade);
                        else
                            NewSellTrade?.Invoke(trade);

                        return;
                    }

                    if (eventType == "candles")
                    {
                        if (data.ValueKind != JsonValueKind.Array || data.GetArrayLength() < 6)
                        {
                            Console.WriteLine($"Ошибка свечей данных: {message}");
                            return;
                        }

                        var candle = new Candle
                        {
                            OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(data[0].GetInt64()),
                            OpenPrice = data[1].GetDecimal(),
                            HighPrice = data[2].GetDecimal(),
                            LowPrice = data[3].GetDecimal(),
                            ClosePrice = data[4].GetDecimal(),
                            TotalVolume = data[5].GetDecimal()
                        };
                        
                        CandleSeriesProcessing?.Invoke(candle);

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }
   
    
   
    public void SubscribeTrades(string pair, int maxCount = 100)
    {
        var subscribeMessage = $"{{\"event\":\"subscribe\",\"channel\":\"trades\",\"symbol\":\"{pair}\",\"maxCount\":{maxCount}}}";
        SendMessage(subscribeMessage);
    }
    
    public void UnsubscribeTrades(string pair)
    {

        if (_subscriptions.TryGetValue(pair, out int chanId))
        {
            var unsubscribeMessage = $"{{\"event\":\"unsubscribe\",\"chanId\":{chanId}}}";
            SendMessage(unsubscribeMessage);
            Console.WriteLine($"Unsubscribed: from {pair} chanId: {chanId}");
        }
        
        
    }
    
    
    public void SubscribeCandles(string pair, int periodInSec, long? count, DateTimeOffset? from = null, DateTimeOffset? to = null)
    {
        var subscribeMessage = $"{{\"event\":\"subscribe\",\"channel\":\"candles\",\"key\":\"trade:{periodInSec/60}m:{pair}\",\"maxCount\":{count}}}";
        SendMessage(subscribeMessage);
    }

    public void UnsubscribeCandles(string pair)
    {
        if (_subscriptions.TryGetValue(pair, out int chanId))
        {
            var unsubscribeMessage = $"{{\"event\":\"unsubscribe\",\"chanId\":{chanId}}}";
            SendMessage(unsubscribeMessage);
            Console.WriteLine($"Unsubscribed: from {pair} chanId: {chanId}");
        }
    }
    
    
    private async void SendMessage(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var buffer = new ArraySegment<byte>(bytes);
        try
        {
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"Sent message: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }
    
    
}