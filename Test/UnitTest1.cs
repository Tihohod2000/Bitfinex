using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bitfinex;
using Bitfinex.data;
using ConnectorTest;
using Moq;
using NUnit.Framework;
using TestHQ;

namespace Test;

public class Tests
{
    public class BitfinexConnectorTests
    {
        private Mock<IRestApi> _restApiMock;
        private Mock<ISocket> _socketMock;

        [SetUp]
        public void Setup()
        {
            _restApiMock = new Mock<IRestApi>();
            _socketMock = new Mock<ISocket>();
        }

        [Test]
        public async Task GetNewTradesAsync_ReturnsTrades()
        {
            
            var mockConnector = new Mock<ITestConnector>();
            
            var expectedTrades = new List<Trade>
            {
                new Trade { Price = 12300, Amount = 0.1m, Time = DateTimeOffset.UtcNow },
                new Trade { Price = 49900, Amount = 0.2m, Time = DateTimeOffset.UtcNow.AddSeconds(-10) },
                new Trade { Price = 32, Amount = 0.9m, Time = DateTimeOffset.UtcNow.AddSeconds(-60) }
            };

            mockConnector
                .Setup(api => api.GetNewTradesAsync("BTC/USD", 10))
                .ReturnsAsync(expectedTrades);

            
            var trades = await mockConnector.Object.GetNewTradesAsync("BTC/USD", 10);

            
            Assert.NotNull(trades);
            Assert.AreEqual(3, trades.Count());
            Assert.AreEqual(12300, trades.First().Price);
        }

        [Test]
        public async Task GetCandleSeriesAsync_ShouldReturnCandles()
        {
            var mockConnector = new Mock<ITestConnector>();
            var expectedCandles = new List<Candle>
            {
                new Candle { OpenPrice = 5000, ClosePrice = 5200, HighPrice = 5300, LowPrice = 4900, TotalVolume = 1.5m, OpenTime = DateTimeOffset.UtcNow },
                new Candle { OpenPrice = 5200, ClosePrice = 5500, HighPrice = 5600, LowPrice = 5100, TotalVolume = 2.0m, OpenTime = DateTimeOffset.UtcNow.AddSeconds(-60) }
            };

            
            mockConnector
                .Setup(api => api.GetCandleSeriesAsync("BTC/USD", 60, It.IsAny<DateTimeOffset?>(), 2, null))
                .ReturnsAsync(expectedCandles);

            
            var result = await mockConnector.Object.GetCandleSeriesAsync("BTC/USD", 60, DateTimeOffset.UtcNow, 2);

            
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            
            // Проверка TotalPrice
            for (int i = 0; i < result.Count(); i++)
            {
                var candle = result.ElementAt(i);
                var expectedCandle = expectedCandles[i];
                Assert.AreEqual(expectedCandle.TotalPrice, candle.TotalPrice, $"TotalPrice свечи {i + 1} не совпадает.");
            }

            // Проверка всех полей
            for (int i = 0; i < result.Count(); i++)
            {
                var candle = result.ElementAt(i);
                var expectedCandle = expectedCandles[i];

                Assert.AreEqual(expectedCandle.OpenPrice, candle.OpenPrice, $"OpenPrice для свечи {i + 1} не совпадает.");
                Assert.AreEqual(expectedCandle.ClosePrice, candle.ClosePrice, $"ClosePrice для свечи {i + 1} не совпадает.");
                Assert.AreEqual(expectedCandle.HighPrice, candle.HighPrice, $"HighPrice для свечи {i + 1} не совпадает.");
                Assert.AreEqual(expectedCandle.LowPrice, candle.LowPrice, $"LowPrice для свечи {i + 1} не совпадает.");
                Assert.AreEqual(expectedCandle.TotalVolume, candle.TotalVolume, $"TotalVolume для свечи {i + 1} не совпадает.");
                Assert.AreEqual(expectedCandle.OpenTime, candle.OpenTime, $"OpenTime для свечи {i + 1} не совпадает.");
            }
            
            CollectionAssert.AreEqual(expectedCandles, result);
        }


        [Test]
        public async Task CalcWaller()
        {
            var mockConnector = new Mock<ITestConnector>();
            var expeWallet = new Wallet() { Btc = 1, Xrp = 2, Xmr = 2, Dash = 1 };

            mockConnector.Setup(c => c.calc_wallet(1, 2, 2, 1)).ReturnsAsync(expeWallet);


            var result = await mockConnector.Object.calc_wallet(1, 2, 2, 1);
            
            Assert.AreEqual(expeWallet, result);

        }


        [Test]
        public async Task ConnectAsync()
        {
            var mockConnector = new Mock<ITestConnector>();
            
            mockConnector
                .Setup(c => c.ConnectAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            await mockConnector.Object.ConnectAsync();
            
            mockConnector.Verify(c => c.ConnectAsync(), Times.Once);
        }

        [Test]

        public void SubscribeTrades()
        {
            var mockConnector = new Mock<ITestConnector>();
            Trade receTrade = null;
            var expeTrade = new Trade { Price = 50000, Amount = 0.1m };

            mockConnector
                .Setup(c => c.SubscribeTrades("BTCUSD", 100))
                .Raises(c => c.NewBuyTrade += null, expeTrade);

            mockConnector.Object.NewBuyTrade += trade => receTrade = trade;
            
            mockConnector.Object.SubscribeTrades("BTCUSD", 100);
            
            Assert.AreEqual(expeTrade, receTrade);
        }


        [Test]
        public void UnsubscribeTrades()
        {
            var mockConnector = new Mock<ITestConnector>();
            Trade receivedTrade = null;
            void Handler(Trade trade) => receivedTrade = trade;
            
            mockConnector.Object.NewBuyTrade += Handler;
            
            mockConnector.Setup(c => c.UnsubscribeTrades("BTCUSD"))
                .Callback(() => mockConnector.Object.NewBuyTrade -= Handler);
            
            mockConnector.Object.UnsubscribeTrades("BTCUSD");
            
            mockConnector.Raise(c => c.NewBuyTrade += null, new Trade { Price = 50000, Amount = 0.1m });
            
            Assert.IsNull(receivedTrade, "Событие не должно было сработать");
            
        }
        
        
        [Test]

        public void SubscribeCandles()
        {
            var mockConnector = new Mock<ITestConnector>();
            Candle  receTrade = null;
            var expeTrade = new Candle {OpenPrice  = 50000, ClosePrice  = 0.1m };

            mockConnector
                .Setup(c => c.SubscribeCandles("BTCUSD", 60, null, null, null))
                .Raises(c => c.CandleSeriesProcessing += null, expeTrade);

            mockConnector.Object.CandleSeriesProcessing += candle  => receTrade = candle;
            
            mockConnector.Object.SubscribeCandles("BTCUSD", 60,null);
            
            Assert.AreEqual(expeTrade, receTrade);
        }


        [Test]
        public void UnsubscribeCandles()
        {
            var mockConnector = new Mock<ITestConnector>();
            Candle receCandle = null;
            
            void Handler(Candle candle) => receCandle = candle;
            
            mockConnector.Object.CandleSeriesProcessing += Handler;
            
            mockConnector.Setup(c => c.UnsubscribeCandles("BTCUSD"))
                .Callback(() => mockConnector.Object.CandleSeriesProcessing -= Handler);
            
            mockConnector.Object.UnsubscribeCandles("BTCUSD");
            
            mockConnector.Raise(c => c.CandleSeriesProcessing += null, new Candle { OpenPrice = 50000, ClosePrice = 51000, HighPrice = 52000, LowPrice = 49000, TotalVolume = 0.1m, OpenTime = DateTimeOffset.UtcNow });
            
            Assert.IsNull(receCandle, "Событие не должно было сработать");
            
        }
        
    }
}