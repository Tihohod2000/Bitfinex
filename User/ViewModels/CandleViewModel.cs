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
        private string _candleInput_pair;
        private int _candleInput_count;
        private int _candleInput_period;
        private DateTimeOffset _candleInput_from;
        private DateTimeOffset _candleInput_to;

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
        
        public DateTimeOffset CandleInput_from
        {
            get => _candleInput_from;
            set
            {
                _candleInput_from = value;
                OnPropertyChanged(nameof(CandleInput_from));
            }
        }
        
        public DateTimeOffset CandleInput_to
        {
            get => _candleInput_to;
            set
            {
                _candleInput_to = value;
                OnPropertyChanged(nameof(CandleInput_to));
            }
        }
        
        public int CandleInput_count
        {
            get => _candleInput_count;
            set
            {
                _candleInput_count = value;
                OnPropertyChanged(nameof(CandleInput_count));
            }
        }
        
        public int CandleInput_period
        {
            get => _candleInput_period;
            set
            {
                _candleInput_period = value;
                OnPropertyChanged(nameof(CandleInput_period));
            }
        }
    
    
        public string CandleInput_pair
        {
            get => _candleInput_pair;
            set
            {
                _candleInput_pair = value;
                OnPropertyChanged(nameof(CandleInput_pair));
            }
        }
        
        

        public async Task LoadCandlesAsync(string pair, int count, int period, DateTimeOffset? from, DateTimeOffset? to = null)
        {
            // tBTCUSD
            IsLoading = true;
            try
            {
                // Используем BitfinexConnector для получения свечей
                var candles = await _bitfinexConnector.GetCandleSeriesAsync(pair, period, from, count, to);
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
