using Bitfinex;
using System;
using System.Collections.Generic;
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
    private int _tradeInput_count;
    private string _tradeInput_pair;
   
    
    public TradeViewModel()
    {
        _bitfinexConnector = new BitfinexConnector(new RestApi(), new Socket()); 
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
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}