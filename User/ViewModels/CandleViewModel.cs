using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bitfinex;
using Bitfinex.data;
using ConnectorTest;
using TestHQ;

namespace User.ViewModels
{
    public class CandleViewModel : INotifyPropertyChanged
    {
        private IEnumerable<Candle> _candles;
        private bool _isLoading;
        private readonly ITestConnector _bitfinexConnector;
        private readonly RestApi _restApi;
        private readonly Socket _socket;
        private string _candleInputPair;
        private int _candleInputCount;
        private int _candleInputPeriod;
        private DateTimeOffset _candleInputFrom;
        private DateTimeOffset _candleInputTo;

        private string _candleInputPairSocket;
        private int _candleInputCountSocket;
        private int _candleInputPeriodSocket;
        private DateTimeOffset _candleInputFromSocket;
        private DateTimeOffset _candleInputToSocket;
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
                if (!Equals(_candles, value))
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


        public DateTimeOffset CandleInputFrom
        {
            get => _candleInputFrom;
            set
            {
                _candleInputFrom = value;
                OnPropertyChanged(nameof(CandleInputFrom));
            }
        }

        public DateTimeOffset CandleInputTo
        {
            get => _candleInputTo;
            set
            {
                _candleInputTo = value;
                OnPropertyChanged(nameof(CandleInputTo));
            }
        }

        public int CandleInputCount
        {
            get => _candleInputCount;
            set
            {
                _candleInputCount = value;
                OnPropertyChanged(nameof(CandleInputCount));
            }
        }

        public int CandleInputPeriod
        {
            get => _candleInputPeriod;
            set
            {
                _candleInputPeriod = value;
                OnPropertyChanged(nameof(CandleInputPeriod));
            }
        }


        public string CandleInputPair
        {
            get => _candleInputPair;
            set
            {
                _candleInputPair = value;
                OnPropertyChanged(nameof(CandleInputPair));
            }
        }


        public DateTimeOffset CandleInputFromSocket
        {
            get => _candleInputFromSocket;
            set
            {
                _candleInputFromSocket = value;
                OnPropertyChanged(nameof(CandleInputFrom));
            }
        }

        public DateTimeOffset CandleInputToSocket
        {
            get => _candleInputToSocket;
            set
            {
                _candleInputToSocket = value;
                OnPropertyChanged(nameof(CandleInputTo));
            }
        }

        public int CandleInputCountSocket
        {
            get => _candleInputCountSocket;
            set
            {
                _candleInputCountSocket = value;
                OnPropertyChanged(nameof(CandleInputCount));
            }
        }

        public int CandleInputPeriodSocket
        {
            get => _candleInputPeriodSocket;
            set
            {
                _candleInputPeriodSocket = value;
                OnPropertyChanged(nameof(CandleInputPeriod));
            }
        }


        public string CandleInputPairSocket
        {
            get => _candleInputPairSocket;
            set
            {
                _candleInputPairSocket = value;
                OnPropertyChanged(nameof(CandleInputPair));
            }
        }


        public async Task LoadCandlesAsync(string pair, int count, int period, DateTimeOffset? from,
            DateTimeOffset? to = null)
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

        public async Task ConnectCandlesAsync(string pair, int period, int count, DateTimeOffset from,
            DateTimeOffset? to = null)
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

        public async Task UnConnectCandlesAsync(string pair)
        {
            _bitfinexConnector.UnsubscribeCandles(pair);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}