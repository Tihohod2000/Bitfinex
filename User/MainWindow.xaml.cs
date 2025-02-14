using System;
using System.Windows;
using User.Views;

namespace User
{
    public partial class MainWindow
    {
        private readonly MainViewModel _mainViewModel = new MainViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _mainViewModel;
        }

        private async void OnLoadCandlesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
                _mainViewModel.CandleVM.CandleInput_pair;
            int userInputCount =
                _mainViewModel.CandleVM.CandleInput_count;
            int userInputPeriod =
                _mainViewModel.CandleVM.CandleInput_period;
            DateTimeOffset userInputFrom =
                _mainViewModel.CandleVM.CandleInput_from;
            DateTimeOffset userInputTo =
                _mainViewModel.CandleVM.CandleInput_to;
            await _mainViewModel.CandleVM.LoadCandlesAsync(userInputPair, userInputCount, userInputPeriod, userInputFrom, userInputTo);
        }
        
        private async void OnLoadTradesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
            _mainViewModel.TradeVM.TradeInput_pair;
            int userInputCount =
                _mainViewModel.TradeVM.TradeInput_count;
            await  _mainViewModel.TradeVM.LoadTradesAsync(userInputPair, userInputCount);
        }
        
        
        private async void OnConnectTradesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
                _mainViewModel.TradeVM.TradeInput_pairSocket;
            int userInputCount =
                _mainViewModel.TradeVM.TradeInput_countSocket;
            await  _mainViewModel.TradeVM.ConnectTradesAsync(userInputPair, userInputCount);
        }
        
        private async void OnConnectCandlesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
                _mainViewModel.CandleVM.CandleInput_pairSocket;
            int userInputCount =
                _mainViewModel.CandleVM.CandleInput_countSocket;
            int userInputPeriod =
                _mainViewModel.CandleVM.CandleInput_periodSocket;
            DateTimeOffset from = _mainViewModel.CandleVM.CandleInput_fromSocket;
            DateTimeOffset to = _mainViewModel.CandleVM.CandleInput_toSocket;
            await  _mainViewModel.CandleVM.ConnectCandlesAsync(userInputPair, userInputPeriod, userInputCount, from, to);
        }

        private async void ConvectorCurrency(object sender, RoutedEventArgs e)
        {
            int btc = _mainViewModel.ConvectorVM.Wallet_btc;
            int xrp = _mainViewModel.ConvectorVM.Wallet_xrp;
            int xmr = _mainViewModel.ConvectorVM.Wallet_xmr;
            int dash = _mainViewModel.ConvectorVM.Wallet_dash;
            await _mainViewModel.ConvectorVM.LoadCurrencyAsync(btc, xrp, xmr, dash);
        }

    }
}