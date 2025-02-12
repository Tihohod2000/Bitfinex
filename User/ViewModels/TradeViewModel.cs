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
    
    
    public async Task LoadTradesAsync()
    {
        IsLoading = true;
        try
        {
            // Используем BitfinexConnector для получения свечей
            var trades = await _bitfinexConnector.GetNewTradesAsync("tBTCUSD", 24);
            Trades = trades;
        }
        catch (Exception ex)
        {
            // Обработка ошибок (выводить в лог или показывать пользователю)
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