using Bitfinex;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly RestApi _restApi;
        private readonly Socket _socket;
        private string _candleInput_pair;
        private int _candleInput_count;
        private int _candleInput_period;
        private DateTimeOffset _candleInput_from;
        private DateTimeOffset _candleInput_to;
        
        private string _candleInput_pairSocket;
        private int _candleInput_countSocket;
        private int _candleInput_periodSocket;
        private DateTimeOffset _candleInput_fromSocket;
        private DateTimeOffset _candleInput_toSocket;
        private bool _isConnected = false;

        public CandleViewModel()
        {
            _restApi = new RestApi(); 
            _socket = new Socket();
            _bitfinexConnector = new BitfinexConnector(_restApi, _socket); 
        }
        
        private ObservableCollection<Candle> _candlesSocket = new();

        public ObservableCollection<Candle> CandlesSocket
        {
            get => _candlesSocket;
            set
            {
                _candlesSocket = value;
                OnPropertyChanged(nameof(CandlesSocket));
            }
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
        
        
        
        public DateTimeOffset CandleInput_fromSocket
        {
            get => _candleInput_fromSocket;
            set
            {
                _candleInput_fromSocket = value;
                OnPropertyChanged(nameof(CandleInput_from));
            }
        }
        
        public DateTimeOffset CandleInput_toSocket
        {
            get => _candleInput_toSocket;
            set
            {
                _candleInput_toSocket = value;
                OnPropertyChanged(nameof(CandleInput_to));
            }
        }
        
        public int CandleInput_countSocket
        {
            get => _candleInput_countSocket;
            set
            {
                _candleInput_countSocket = value;
                OnPropertyChanged(nameof(CandleInput_count));
            }
        }
        
        public int CandleInput_periodSocket
        {
            get => _candleInput_periodSocket;
            set
            {
                _candleInput_periodSocket = value;
                OnPropertyChanged(nameof(CandleInput_period));
            }
        }
    
    
        public string CandleInput_pairSocket
        {
            get => _candleInput_pairSocket;
            set
            {
                _candleInput_pairSocket = value;
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
        
        public async Task ConnectCandlesAsync( string pair, int period, int count, DateTimeOffset from, DateTimeOffset? to = null)
        {
            IsLoading = true;
            try
            {
                // Проверяем, подключены ли мы уже
                if (!_isConnected)
                {
                    await _bitfinexConnector.ConnectAsync();
                    _isConnected = true; // Помечаем, что подключение установлено
                }
                _socket.CandleSeriesProcessing += candle =>
                {
                    CandlesSocket.Add(candle);
                
                    Console.WriteLine($"Валютная пара: {candle.Pair}");
                    Console.WriteLine($"Цена открытия: {candle.OpenPrice}");
                    Console.WriteLine($"Максимальная цена: {candle.HighPrice}");
                    Console.WriteLine($"Минимальная цена: {candle.LowPrice}");
                    Console.WriteLine($"Цена закрытия: {candle.ClosePrice}");
                    Console.WriteLine($"Общая сумма сделок: {candle.TotalPrice}");
                    Console.WriteLine($"Общий объем: {candle.TotalVolume}");
                    Console.WriteLine($"Время: {candle.OpenTime}");
                };
            
                _bitfinexConnector.SubscribeCandles(pair, period, count, from, to);
            }
            catch (Exception ex)
            {
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
