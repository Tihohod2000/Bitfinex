using System;
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
            string userInput_pair =
                _mainViewModel.CandleVM.CandleInput_pair;
            int userInput_count =
                _mainViewModel.CandleVM.CandleInput_count;
            int userInput_period =
                _mainViewModel.CandleVM.CandleInput_period;
            DateTimeOffset userInput_from =
                _mainViewModel.CandleVM.CandleInput_from;
            DateTimeOffset userInput_to =
                _mainViewModel.CandleVM.CandleInput_to;
            await _mainViewModel.CandleVM.LoadCandlesAsync(userInput_pair, userInput_count, userInput_period, userInput_from, userInput_to);
        }
        
        private async void OnLoadTradesClick(object sender, RoutedEventArgs e)
        {
            string userInput_pair =
            _mainViewModel.TradeVM.TradeInput_pair;
            int userInput_count =
                _mainViewModel.TradeVM.TradeInput_count;
            await  _mainViewModel.TradeVM.LoadTradesAsync(userInput_pair, userInput_count);
        }
    }
}