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
        
        
        private async void OnConnectTradesClick(object sender, RoutedEventArgs e)
        {
            string userInput_pair =
                _mainViewModel.TradeVM.TradeInput_pairSocket;
            int userInput_count =
                _mainViewModel.TradeVM.TradeInput_countSocket;
            await  _mainViewModel.TradeVM.ConnectTradesAsync(userInput_pair, userInput_count);
        }
        
        private async void OnConnectCandlesClick(object sender, RoutedEventArgs e)
        {
            string userInput_pair =
                _mainViewModel.CandleVM.CandleInput_pairSocket;
            int userInput_count =
                _mainViewModel.CandleVM.CandleInput_countSocket;
            int userInput_period =
                _mainViewModel.CandleVM.CandleInput_periodSocket;
            DateTimeOffset from = _mainViewModel.CandleVM.CandleInput_fromSocket;
            DateTimeOffset to = _mainViewModel.CandleVM.CandleInput_toSocket;
            await  _mainViewModel.CandleVM.ConnectCandlesAsync(userInput_pair, userInput_period, userInput_count, from, to);
        }

        private async void ConvectorCurrency(object sender, RoutedEventArgs e)
        {
            await _mainViewModel.ConvectorVM.LoadCurrencyAsync("", "");
        }
    }
}