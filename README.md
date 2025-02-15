# Решение тестового задания

## Библиотека и GUI FPW MVVM

Библиотека коннектор для работы с биржей Bitfinex.

Реализован коннектор под исходный интерфейс на C# (Class Library). Сделан в отдельном проекте простой вывод на GUI Framework (WPF) на основе паттерна MVVM

### Пример использования программы User

![image](https://github.com/user-attachments/assets/d21bd276-0a78-4bf4-8631-f2873f5050d2)

Для расчёта общего баланса в различных валютах требуется ввести данные о количестве валюты и нажать кнопку "Расчёт". После выполнения процесса конвертиции, данные отобразятся в таблице.
![image](https://github.com/user-attachments/assets/52b87b3c-61d5-4803-8d71-09411a0e78a8)

Для получения информации о свечах через RestApi требуется ввести валютую пару (например: "tBTCUSD"), количество необходимых свечей, период в секундах и даты и нажать кнопку "Запрос".
После получения ответа, данные отобразятся ниже.

![image](https://github.com/user-attachments/assets/d4546635-a185-466f-8c31-948448837a8e)

Для трейдов требуется валютная пара и количество.

![image](https://github.com/user-attachments/assets/b011859d-0b8d-49c6-8c35-1fb78a41ae3b)

Для подписок аналогично.

![image](https://github.com/user-attachments/assets/62de1178-45cf-4a19-885b-f54cd51bd1ad)

Чтобы отписаться, нужно ввести валютную пару в соответствующем окне и нать кнопку "Отписаться".

![image](https://github.com/user-attachments/assets/3212923d-11bb-466c-a961-2f8fa36d6861)


## Пример использования коннектора
Сначала нужно указать зависимости и создать коннектор

```using Bitfinex;
using Bitfinex.data;
using ConnectorTest;

var restApi = new RestApi();
var socket = new Socket();
ITestConnector connector = new BitfinexConnector(restApi, socket);
```

RestApi запрос получения трейдов
```
var response_trade = await connector.GetNewTradesAsync("tBTCUSD", 24);
```

RestApi запрос получения свечей
```
DateTimeOffset? from = DateTimeOffset.UtcNow.AddHours(-1);
DateTimeOffset? to = DateTimeOffset.UtcNow.AddHours(2);

var responseCandle = await connector.GetCandleSeriesAsync("tBTCUSD", 60, from, 13, to );
```

Конверктор валют, работает через RestApi
```
var responseWallet = await connector.calc_wallet(10, 10, 10, 10);
```

Socket подписка на трейды
```
await connector.ConnectAsync();

socket.NewBuyTrade += trade =>
{
     Console.WriteLine($"Время: {trade.Time}");
     Console.WriteLine($"Id: {trade.Id}");
     Console.WriteLine($"Объём: {trade.Amount}");
     Console.WriteLine($"Цена: {trade.Price}");
     Console.WriteLine($"Направление: {trade.Side}");
};

socket.NewSellTrade += trade =>
{
     Console.WriteLine($"Время: {trade.Time}");
     Console.WriteLine($"Id: {trade.Id}");
     Console.WriteLine($"Объём: {trade.Amount}");
     Console.WriteLine($"Цена: {trade.Price}");
     Console.WriteLine($"Направление: {trade.Side}");
};

connector.SubscribeTrades("tBTCUSD", 101);
await Task.Delay(100 * 1000);
```
Отписка от трейды
```
connector.UnsubscribeTrades("tBTCUSD");
```

аналогичная подписка на свечи
```
DateTimeOffset? from = DateTimeOffset.UtcNow.AddHours(-1);
DateTimeOffset? to = DateTimeOffset.UtcNow.AddHours(2);

connector.SubscribeCandles("tBTCUSD", 60,  100, from, to);
await Task.Delay(20 * 1000);
```

Отписка от свечей
```
connector.UnsubscribeCandles("tBTCUSD");
```
