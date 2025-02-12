using System.ComponentModel;
using System.Runtime.CompilerServices;
using MyWpfApp.ViewModels;

namespace User.Views;

public class MainViewModel : INotifyPropertyChanged
{
    private CandleViewModel _candleVM = new CandleViewModel();
    private TradeViewModel _tradeVM = new TradeViewModel();
    
    public CandleViewModel CandleVM
    {
        get => _candleVM;
        set
        {
            _candleVM = value;
            OnPropertyChanged();
        }
    }

    public TradeViewModel TradeVM
    {
        get => _tradeVM;
        set
        {
            _tradeVM = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}