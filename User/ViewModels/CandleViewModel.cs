using Bitfinex;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Bitfinex.data;
using TestHQ;

namespace MyWpfApp.ViewModels
{
    public class CandleViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Candle> _candles;
        private bool _isLoading;
        private readonly BitfinexConnector _bitfinexConnector;

        public CandleViewModel()
        {
            _bitfinexConnector = new BitfinexConnector(new RestApi(), new Socket()); 
        }

        public IEnumerable<Candle> Candles
        {
            get => _candles;
            set
            {
                if (_candles != value)
                {
                    _candles = value;
                    OnPropertyChanged(nameof(Candles));
                }
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

        public async Task LoadCandlesAsync()
        {
            IsLoading = true;
            try
            {
                // Используем BitfinexConnector для получения свечей
                var candles = await _bitfinexConnector.GetCandleSeriesAsync("tBTCUSD", 60, DateTimeOffset.Now.AddHours(-1), 100);
                Candles = candles;
            }
            catch (Exception ex)
            {
                // Обработка ошибок (выводить в лог или показывать пользователю)
                Console.WriteLine($"Error: {ex.Message}");
            }
            IsLoading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
