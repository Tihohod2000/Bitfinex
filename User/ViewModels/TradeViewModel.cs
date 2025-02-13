using Bitfinex;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Bitfinex.data;
using TestHQ;

namespace MyWpfApp.ViewModels;

public class TradeViewModel : INotifyPropertyChanged
{
    private IEnumerable<Trade> _trades;
    private bool _isLoading;
    private readonly BitfinexConnector _bitfinexConnector;
    private readonly RestApi _restApi;
    private readonly Socket _socket;
    private int _tradeInput_count;
    private int _tradeInput_countSocket;
    private string _tradeInput_pair;
    private string _tradeInput_pairSocket;
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
            if (_trades != value)
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
    
    
    public int TradeInput_count
    {
        get => _tradeInput_count;
        set
        {
            _tradeInput_count = value;
            OnPropertyChanged(nameof(TradeInput_count));
        }
    }
    
    
    public string TradeInput_pair
    {
        get => _tradeInput_pair;
        set
        {
            _tradeInput_pair = value;
            OnPropertyChanged(nameof(TradeInput_pair));
        }
    }
    
    
    public int TradeInput_countSocket
    {
        get => _tradeInput_countSocket;
        set
        {
            _tradeInput_countSocket = value;
            OnPropertyChanged(nameof(TradeInput_countSocket));
        }
    }
    
    
    public string TradeInput_pairSocket
    {
        get => _tradeInput_pairSocket;
        set
        {
            _tradeInput_pairSocket = value;
            OnPropertyChanged(nameof(TradeInput_pairSocket));
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
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}