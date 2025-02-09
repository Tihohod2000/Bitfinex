using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bitfinex;
using Bitfinex.data;
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
        private BitfinexConnector _connector;

        [SetUp]
        public void Setup()
        {
            _restApiMock = new Mock<IRestApi>();
            _socketMock = new Mock<ISocket>();
            _connector = new BitfinexConnector(_restApiMock.Object, _socketMock.Object);
        }

        [Test]
        public async Task GetNewTradesAsync_ReturnsTrades()
        {
            
            var expectedTrades = new List<Trade>
            {
                new Trade { Price = 12300, Amount = 0.1m, Time = DateTimeOffset.UtcNow },
                new Trade { Price = 49900, Amount = 0.2m, Time = DateTimeOffset.UtcNow.AddSeconds(-10) },
                new Trade { Price = 32, Amount = 0.9m, Time = DateTimeOffset.UtcNow.AddSeconds(-60) }
            };

            _restApiMock
                .Setup(api => api.GetNewTradesAsync("BTC/USD", 10))
                .ReturnsAsync(expectedTrades);

            
            var trades = await _connector.GetNewTradesAsync("BTC/USD", 10);

            
            Assert.NotNull(trades);
            Assert.AreEqual(3, trades.Count());
            Assert.AreEqual(12300, trades.First().Price);
        }

        [Test]
        public async Task GetCandleSeriesAsync_ShouldReturnCandles()
        {
            
            var expectedCandles = new List<Candle>
            {
                new Candle { OpenPrice = 5000, ClosePrice = 5200, HighPrice = 5300, LowPrice = 4900, TotalVolume = 1.5m, OpenTime = DateTimeOffset.UtcNow },
                new Candle { OpenPrice = 5200, ClosePrice = 5500, HighPrice = 5600, LowPrice = 5100, TotalVolume = 2.0m, OpenTime = DateTimeOffset.UtcNow.AddSeconds(-60) }
            };

            
            _restApiMock
                .Setup(api => api.GetCandleSeriesAsync("BTC/USD", 60, It.IsAny<DateTimeOffset?>(), 2, null))
                .ReturnsAsync(expectedCandles);

            
            var result = await _connector.GetCandleSeriesAsync("BTC/USD", 60, DateTimeOffset.UtcNow, 2);

            
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
        
        
        
        
    }
}