using System.Threading.Tasks;
using System.Windows;
using MyWpfApp.ViewModels;
using User.Views;

namespace MyWpfApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel = new MainViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainViewModel;
        }

        private async void OnLoadCandlesClick(object sender, RoutedEventArgs e)
        {
            await _mainViewModel.CandleVM.LoadCandlesAsync();
        }
        
        private async void OnLoadTradesClick(object sender, RoutedEventArgs e)
        {
            await  _mainViewModel.TradeVM.LoadTradesAsync();
        }
    }
}