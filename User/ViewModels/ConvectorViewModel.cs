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
    
    
    public async Task LoadCurrencyAsync(int btc, int xrp, int xmr, int dash)
    {
        
        try
        {
            var ratio = await _bitfinexConnector.calc_wallet( btc, xrp, xmr, dash);
            //Переписать!!!!!!!!!!!
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
    }
}