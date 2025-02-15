using System.ComponentModel;
using System.Runtime.CompilerServices;
using User.ViewModels;

namespace User.Views;

public class MainViewModel : INotifyPropertyChanged
{
    private CandleViewModel _candleVm = new CandleViewModel();
    private TradeViewModel _tradeVm = new TradeViewModel();
    private ConvectorViewModel _convectorVm = new ConvectorViewModel();
    

    public CandleViewModel CandleVm
    {
        get => _candleVm;
        set
        {
            _candleVm = value;
            OnPropertyChanged();
        }
    }

    public TradeViewModel TradeVm
    {
        get => _tradeVm;
        set
        {
            _tradeVm = value;
            OnPropertyChanged();
        }
    }
    
    public ConvectorViewModel ConvectorVm
    {
        get => _convectorVm;
        set
        {
            _convectorVm = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}