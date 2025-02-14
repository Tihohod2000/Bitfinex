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
                _mainViewModel.CandleVm.CandleInputPair;
            int userInputCount =
                _mainViewModel.CandleVm.CandleInputCount;
            int userInputPeriod =
                _mainViewModel.CandleVm.CandleInputPeriod;
            DateTimeOffset userInputFrom =
                _mainViewModel.CandleVm.CandleInputFrom;
            DateTimeOffset userInputTo =
                _mainViewModel.CandleVm.CandleInputTo;
            await _mainViewModel.CandleVm.LoadCandlesAsync(userInputPair, userInputCount, userInputPeriod, userInputFrom, userInputTo);
        }
        
        private async void OnLoadTradesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
            _mainViewModel.TradeVm.TradeInputPair;
            int userInputCount =
                _mainViewModel.TradeVm.TradeInputCount;
            await  _mainViewModel.TradeVm.LoadTradesAsync(userInputPair, userInputCount);
        }
        
        
        private async void OnConnectTradesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
                _mainViewModel.TradeVm.TradeInputPairSocket;
            int userInputCount =
                _mainViewModel.TradeVm.TradeInputCountSocket;
            await  _mainViewModel.TradeVm.ConnectTradesAsync(userInputPair, userInputCount);
        }
        
        
        private async void UnconnectTradesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
                _mainViewModel.TradeVm.TradeInputPairSocket;

            await  _mainViewModel.TradeVm.UnConnectTradesAsync(userInputPair);
        }
        
        private async void OnConnectCandlesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
                _mainViewModel.CandleVm.CandleInputPairSocket;
            int userInputCount =
                _mainViewModel.CandleVm.CandleInputCountSocket;
            int userInputPeriod =
                _mainViewModel.CandleVm.CandleInputPeriodSocket;
            DateTimeOffset from = _mainViewModel.CandleVm.CandleInputFromSocket;
            DateTimeOffset to = _mainViewModel.CandleVm.CandleInputToSocket;
            await  _mainViewModel.CandleVm.ConnectCandlesAsync(userInputPair, userInputPeriod, userInputCount, from, to);
        }
        
        private async void UnconnectCandlesClick(object sender, RoutedEventArgs e)
        {
            string userInputPair =
                _mainViewModel.CandleVm.CandleInputPairSocket;

            await  _mainViewModel.CandleVm.UnConnectCandlesAsync(userInputPair);
        }
        
        

        private async void ConvectorCurrency(object sender, RoutedEventArgs e)
        {
            int btc = _mainViewModel.ConvectorVm.WalletBtc;
            int xrp = _mainViewModel.ConvectorVm.WalletXrp;
            int xmr = _mainViewModel.ConvectorVm.WalletXmr;
            int dash = _mainViewModel.ConvectorVm.WalletDash;
            await _mainViewModel.ConvectorVm.LoadCurrencyAsync(btc, xrp, xmr, dash);
        }
        
        

    }
}