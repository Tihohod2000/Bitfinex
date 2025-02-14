using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bitfinex;
using Bitfinex.data;

namespace MyWpfApp.ViewModels;

public class ConvectorViewModel : INotifyPropertyChanged
{
    private readonly BitfinexConnector _bitfinexConnector;
    private readonly RestApi _restApi;
    private readonly Socket _socket;
    private ObservableCollection<wallet> _wallet;
    private bool _isLoading;
    private int _BTC;
    private int _XRP;
    private int _XMR;
    private int _DASH;
    
    public ConvectorViewModel()
    {
        _restApi = new RestApi(); 
        _socket = new Socket();
        _bitfinexConnector = new BitfinexConnector(_restApi, _socket);
        _wallet = new ObservableCollection<wallet>();
    }
    
    
    public ObservableCollection<wallet> Wallet
    {
        get => _wallet;
        set
        {
            if (_wallet != value)
            {
                _wallet = value;
                OnPropertyChanged(nameof(Wallet));
            }
        }
    }
    
    public int Wallet_btc
    {
        get => _BTC;
        set
        {
            _BTC = value;
            OnPropertyChanged(nameof(Wallet_btc));
        }
    }
    
    public int Wallet_xmr
    {
        get => _XMR;
        set
        {
            _XMR = value;
            OnPropertyChanged(nameof(Wallet_xmr));
        }
    }
    
    public int Wallet_xrp
    {
        get => _XRP;
        set
        {
            _XRP = value;
            OnPropertyChanged(nameof(Wallet_xrp));
        }
    }
    
    public int Wallet_dash
    {
        get => _DASH;
        set
        {
            _DASH = value;
            OnPropertyChanged(nameof(Wallet_dash));
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
    
    
    public async Task LoadCurrencyAsync(int btc, int xrp, int xmr, int dash)
    {
        IsLoading = true;
        try
        {
            wallet wallet = await _bitfinexConnector.calc_wallet(btc, xrp, xmr, dash);
            Wallet.Clear(); // Очистка текущего списка перед добавлением новых данных
            Wallet.Add(wallet);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        IsLoading = false;
    }
    
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}