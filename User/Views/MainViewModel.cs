using MyWpfApp.ViewModels;

namespace User.Views;

public class MainViewModel
{
    public CandleViewModel Candle { get; set; }
    public TradeViewModel Trade { get; set; }
    
    public MainViewModel()
    {
        Candle = new CandleViewModel();
        Trade = new TradeViewModel();
    }
}