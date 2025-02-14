using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bitfinex;
using Bitfinex.data;
using ConnectorTest;

namespace User.ViewModels;

public class ConvectorViewModel : INotifyPropertyChanged
{
    private readonly ITestConnector _bitfinexConnector;
    private ObservableCollection<Wallet> _wallet;
    private bool _isLoading;
    private int _btc;
    private int _xrp;
    private int _xmr;
    private int _dash;
    
    public ConvectorViewModel()
    {
        var restApi = new RestApi(); 
        var socket = new Socket();
        _bitfinexConnector = new BitfinexConnector(restApi, socket);
        _wallet = new ObservableCollection<Wallet>();
    }
    
    
    public ObservableCollection<Wallet> Wallet
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
    
    public int WalletBtc
    {
        get => _btc;
        set
        {
            _btc = value;
            OnPropertyChanged(nameof(WalletBtc));
        }
    }
    
    public int WalletXmr
    {
        get => _xmr;
        set
        {
            _xmr = value;
            OnPropertyChanged(nameof(WalletXmr));
        }
    }
    
    public int WalletXrp
    {
        get => _xrp;
        set
        {
            _xrp = value;
            OnPropertyChanged(nameof(WalletXrp));
        }
    }
    
    public int WalletDash
    {
        get => _dash;
        set
        {
            _dash = value;
            OnPropertyChanged(nameof(WalletDash));
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
            Wallet wallet = await _bitfinexConnector.calc_wallet(btc, xrp, xmr, dash);
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