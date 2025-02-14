using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bitfinex;
using Bitfinex.data;
using ConnectorTest;
using TestHQ;

namespace User.ViewModels;

public class TradeViewModel : INotifyPropertyChanged
{
    private IEnumerable<Trade> _trades;
    private bool _isLoading;
    private readonly ITestConnector _bitfinexConnector;
    private readonly RestApi _restApi;
    private readonly Socket _socket;
    private int _tradeInputCount;
    private int _tradeInputCountSocket;
    private string _tradeInputPair;
    private string _tradeInputPairSocket;
    private bool _isConnected = false;
    
   
    
    public TradeViewModel()
    {
        _restApi = new RestApi(); 
        _socket = new Socket();
        _bitfinexConnector = new BitfinexConnector(_restApi, _socket); 
    }
    
    private ObservableCollection<Trade> _tradesSocket = new();

    public ObservableCollection<Trade> TradesSocket
    {
        get => _tradesSocket;
        set
        {
            _tradesSocket = value;
            OnPropertyChanged(nameof(TradesSocket));
        }
    }
    
    public IEnumerable<Trade> Trades
    {
        get => _trades;
        set
        {
            if (!Equals(_trades, value))
            {
                _trades = value;
                OnPropertyChanged(nameof(Trades));
            }
        }
    }
    
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
    }
    
    
    public int TradeInputCount
    {
        get => _tradeInputCount;
        set
        {
            _tradeInputCount = value;
            OnPropertyChanged(nameof(TradeInputCount));
        }
    }
    
    
    public string TradeInputPair
    {
        get => _tradeInputPair;
        set
        {
            _tradeInputPair = value;
            OnPropertyChanged(nameof(TradeInputPair));
        }
    }
    
    
    public int TradeInputCountSocket
    {
        get => _tradeInputCountSocket;
        set
        {
            _tradeInputCountSocket = value;
            OnPropertyChanged(nameof(TradeInputCountSocket));
        }
    }
    
    
    public string TradeInputPairSocket
    {
        get => _tradeInputPairSocket;
        set
        {
            _tradeInputPairSocket = value;
            OnPropertyChanged(nameof(TradeInputPairSocket));
        }
    }
    
    
    public async Task LoadTradesAsync( string pair, int count)
    {
        IsLoading = true;
        try
        {
            var trades = await _bitfinexConnector.GetNewTradesAsync(pair, count);
            Trades = trades;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        IsLoading = false;
    }
    
    
    public async Task ConnectTradesAsync( string pair, int count)
    {
        IsLoading = true;
        try
        {
            // Проверяем, подключены ли мы уже
            if (!_isConnected)
            {
                await _bitfinexConnector.ConnectAsync();
                _isConnected = true; // Помечаем, что подключение установлено
            }
            _socket.NewBuyTrade += trade =>
            {
                TradesSocket.Add(trade);
                
                Console.WriteLine($"Время: {trade.Time}");
                Console.WriteLine($"Id: {trade.Id}");
                Console.WriteLine($"Объём: {trade.Amount}");
                Console.WriteLine($"Цена: {trade.Price}");
                Console.WriteLine($"Направление: {trade.Side}");
            };
            
            _socket.NewSellTrade += trade =>
            {
                TradesSocket.Add(trade);
                
                Console.WriteLine($"Время: {trade.Time}");
                Console.WriteLine($"Id: {trade.Id}");
                Console.WriteLine($"Объём: {trade.Amount}");
                Console.WriteLine($"Цена: {trade.Price}");
                Console.WriteLine($"Направление: {trade.Side}");
            };
            
            _bitfinexConnector.SubscribeTrades(pair, count);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        IsLoading = false;
    }

    public async Task UnConnectTradesAsync(string pair)
    {
        _bitfinexConnector.UnsubscribeTrades(pair);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}