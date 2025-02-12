using System.Threading.Tasks;
using System.Windows;
using MyWpfApp.ViewModels;

namespace MyWpfApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly CandleViewModel _candleviewModel = new CandleViewModel();
        private readonly TradeViewModel _tradeviewModel = new TradeViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _candleviewModel;
            DataContext =  _tradeviewModel;
        }

        private async void OnLoadCandlesClick(object sender, RoutedEventArgs e)
        {
            await _candleviewModel.LoadCandlesAsync();
        }
        
        private async void OnLoadTradesClick(object sender, RoutedEventArgs e)
        {
            await  _tradeviewModel.LoadTradesAsync();
        }
    }
}