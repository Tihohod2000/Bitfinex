using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Bitfinex;
using Bitfinex.data;

namespace MyWpfApp.ViewModels;

public class ConvectorViewModel
{
    private readonly BitfinexConnector _bitfinexConnector;
    private readonly RestApi _restApi;
    private readonly Socket _socket;
    private int _BTC;
    private int _XRP;
    private int _XMR;
    private int _DASH;
    
    public ConvectorViewModel()
    {
        _restApi = new RestApi(); 
        _socket = new Socket();
        _bitfinexConnector = new BitfinexConnector(_restApi, _socket); 
    }
    
    
    public async Task LoadCurrencyAsync( string currency_1, string currency_2)
    {
        
        try
        {
            var ratio = await _bitfinexConnector.Convector(currency_1, currency_2);
            //Переписать!!!!!!!!!!!
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
    }
}